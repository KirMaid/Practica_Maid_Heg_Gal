using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CompareServer.Models
{
    public interface IDbProvider<T>
    {
        /// Добавление файла в таблицу
        int Add(T table);

        /// Удаление файла из БД по id
        void Delete(int id);

        /// Получение файла из базы по id.
        /// mode = 1: Id, FileName, Path
        /// mode = 2: Id, Name, Description, Date, Size. 
        T Get(int id, int mode);

        //List<T> GetList(int mode);

    }

    public class DbProvider : IDisposable /*: IDbProvider<TextFile>*/
    {
        private string ConnectionString;

        //private string connectionString = "Data Source=localhost;Initial Catalog=StudentWork;Password=andrey1505;User ID =root;";

        private IDbConnection Connection;


        public DbProvider(string strConn)
        {
            if (string.IsNullOrWhiteSpace(strConn))
                throw new Exception("Не передана строка подключения");
            ConnectionString = strConn;

            Connect();
        }

        public void Dispose()
        {
            Disconnect();
        }

        /// Подключение к БД
        private void Connect()
        {
            try
            {
                Connection = new MySqlConnection(ConnectionString);
                /*Connection = new SqlConnection();
                Connection.ConnectionString = connectionString;*/
                Connection.Open();

                //SetUtf8();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// Отключение от БД
        private void Disconnect()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
        /// Проверка состояния подключения
        private void CheckConnect()
        {
            if (Connection != null)
                // Если подключение открыто
                if (Connection.State == ConnectionState.Open)
                    return;

            Connect();
        }

        /// Сохранение кодировки utf-8 для распознавания русских символов (без этого в БД добавляются "?" вместо символов)
        private void SetUtf8()
        {
            CheckConnect();
            IDbCommand dbCommand = null;
            try
            {
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "SET NAMES utf8;";
                //dbCommand.CommandText = "SET NAMES 'cp1251';";

                dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }
        }

        /// Создание БД с именем name
        public void CreateDataBase(string name)
        {
            CheckConnect();
            IDbCommand dbCommand = null;

            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "CREATE DATABASE @name;";

                IDbDataParameter parameter = dbCommand.CreateParameter();
                parameter.ParameterName = "@name";
                parameter.Value = name;
                dbCommand.Parameters.Add(parameter);

                /*dbCommand.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
                dbCommand.Parameters["@name"].Value = name;*/

                dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }
        }

        /// Создание таблицы TextFile
        public void CreateTableTextFile()
        {
            CheckConnect();
            IDbCommand dbCommand = null;
            string command =
                "create table TextFile(" +
                    "Id INT UNSIGNED NOT NULL AUTO_INCREMENT," +
                    "Name NVARCHAR(100)," +
                    "FileName NVARCHAR(100) NOT NULL," +
                    "Description NVARCHAR(200)," +
                    "Date DATE," +
                    "Content TEXT," +
                    "Shingles TEXT," +
                    "Size MEDIUMTEXT," +
                    "Path NVARCHAR(100)," +
                    "PRIMARY KEY(Id) )" +
                "CHARACTER SET utf8 COLLATE utf8_general_ci," +
                "ENGINE=MyISAM;";

            try
            {
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = command;
                dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }
        }

        /// Получение файла из базы по id.
        /// mode = 1: Id, FileName, Path.
        /// mode = 2: Id, FileName, Name, Description, Date, Size.
        /// mode = 3: Id, FileName, Shingles.
        public TextFile Get(int id, int mode)
        {
            CheckConnect();
            TextFile file = new TextFile();
            IDbCommand dbCommand = null;
            IDataReader reader = null;
            string command = null;

            switch (mode)
            {
                case 1:
                    command = "SELECT Id, FileName, Path FROM TextFile WHERE Id = @id;";
                    break;
                case 2:
                    command = "SELECT Id, FileName, Name, Description, Date, Size FROM TextFile WHERE Id = @id;";
                    break;
                case 3:
                    command = "SELECT Id, FileName, Shingles FROM TextFile WHERE Id = @id;";
                    break;
                default:
                    throw new Exception("Некорректный mode");
            }

            try
            {
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = command;

                IDbDataParameter parameter = dbCommand.CreateParameter();
                parameter.ParameterName = "@id";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = id;
                dbCommand.Parameters.Add(parameter);

                /*dbCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
                dbCommand.Parameters["@id"].Value = id;*/

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    switch (mode)
                    {
                        case 1:
                            file = new TextFile
                            (
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToString(reader["FileName"]).Trim(),
                                null,
                                null,
                                DateTime.MinValue,
                                null,
                                null,
                                Convert.ToString(reader["Path"]).Trim(),
                                0
                            );
                            break;
                        case 2:
                            file = new TextFile
                            (
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToString(reader["FileName"]).Trim(),
                                Convert.ToString(reader["Name"]).Trim(),
                                Convert.ToString(reader["Description"]).Trim(),
                                Convert.ToDateTime(reader["Date"]),
                                null,
                                null,
                                null,
                                Convert.ToInt64(reader["Size"])
                            );
                            break;
                        case 3:
                            file = new TextFile
                            (
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToString(reader["FileName"]).Trim(),
                                null,
                                null,
                                DateTime.MinValue,
                                null,
                                Convert.ToString(reader["Shingles"]).Trim(),
                                null,
                                0
                            );
                            break;
                        default:
                            throw new Exception("Некорректный mode");
                    }


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
                reader = null;
                dbCommand.Dispose();
                dbCommand = null;
            }

            if (file == null)
                throw new Exception("Файл c id = " + id + " не найден");

            return file;
        }

        /// Добавление файла в таблицу
        public int Add(TextFile file)
        {
            int id = 0;

            CheckConnect();
            IDbCommand dbCommand = null;
            try
            {
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "INSERT INTO TextFile(FileName, Name, Description, Date, Shingles, Size, Path) " +
                   "VALUES (@fileName, @name, @description, @date, @shingles, @size, @path);";

                //dbCommand.CommandText = "INSERT INTO TextFile(FileName, Name, Description, Date, Content, Shingles, Size, Path)" +
                //"VALUES ('test-file.txt', 'testfile', null, '12.11.17 22:57:03', null, null, 640, 'testpath/path');";

                IDbDataParameter parameter1 = dbCommand.CreateParameter();
                parameter1.ParameterName = "@fileName";
                //parameter1.DbType = DbType.String;
                parameter1.Value = file.FileName;
                dbCommand.Parameters.Add(parameter1);

                IDbDataParameter parameter2 = dbCommand.CreateParameter();
                parameter2.ParameterName = "@name";
                parameter2.DbType = DbType.String;
                parameter2.Value = file.Name;
                dbCommand.Parameters.Add(parameter2);

                IDbDataParameter parameter3 = dbCommand.CreateParameter();
                parameter3.ParameterName = "@description";
                parameter3.DbType = DbType.String;
                parameter3.Value = file.Description;
                dbCommand.Parameters.Add(parameter3);

                IDbDataParameter parameter4 = dbCommand.CreateParameter();
                parameter4.ParameterName = "@date";
                parameter4.DbType = DbType.DateTime;
                parameter4.Value = file.Date;
                dbCommand.Parameters.Add(parameter4);

                IDbDataParameter parameter6 = dbCommand.CreateParameter();
                parameter6.ParameterName = "@shingles";
                parameter6.DbType = DbType.String;
                parameter6.Value = file.ShinglesString;
                dbCommand.Parameters.Add(parameter6);

                IDbDataParameter parameter7 = dbCommand.CreateParameter();
                parameter7.ParameterName = "@size";
                parameter7.DbType = DbType.Int64;
                parameter7.Value = file.Size;
                dbCommand.Parameters.Add(parameter7);

                IDbDataParameter parameter8 = dbCommand.CreateParameter();
                parameter8.ParameterName = "@path";
                parameter8.DbType = DbType.String;
                parameter8.Value = file.Path;
                dbCommand.Parameters.Add(parameter8);

                dbCommand.ExecuteNonQuery();

                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "SELECT LAST_INSERT_ID();";
                id = Convert.ToInt32(dbCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return id;
        }
        /// Удаление файла из БД по id
        public int Delete(int id)
        {
            CheckConnect();

            IDbCommand dbCommand = null;
            int count = -1;

            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "DELETE FROM TextFile WHERE id = @id;";

                IDbDataParameter parameter = dbCommand.CreateParameter();
                parameter.ParameterName = "@id";
                parameter.Value = id;
                dbCommand.Parameters.Add(parameter);

                count = dbCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return count;
        }

        /// Обновление информации о файле
        public int Update(TextFile textFile)
        {
            CheckConnect();

            IDbCommand dbCommand = null;
            int count = -1;

            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "UPDATE LOW_PRIORITY TextFile SET Name = @name, Description = @description, Path = @path WHERE id = @id;";

                IDbDataParameter parameter1 = dbCommand.CreateParameter();
                parameter1.ParameterName = "@id";
                parameter1.Value = textFile.Id;
                dbCommand.Parameters.Add(parameter1);

                IDbDataParameter parameter2 = dbCommand.CreateParameter();
                parameter2.ParameterName = "@name";
                parameter2.DbType = DbType.String;
                parameter2.Value = textFile.Name;
                dbCommand.Parameters.Add(parameter2);

                IDbDataParameter parameter3 = dbCommand.CreateParameter();
                parameter3.ParameterName = "@description";
                parameter3.DbType = DbType.String;
                parameter3.Value = textFile.Description;
                dbCommand.Parameters.Add(parameter3);

                IDbDataParameter parameter4 = dbCommand.CreateParameter();
                parameter4.ParameterName = "@path";
                parameter4.DbType = DbType.String;
                parameter4.Value = textFile.Path;
                dbCommand.Parameters.Add(parameter4);

                count = dbCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return count;
        }

        /// Поиск файла в БД по совпадению в названии, имени файла или описании
        public List<TextFile> Find(string select)
        {
            CheckConnect();

            IDbCommand dbCommand = null;
            IDataReader reader = null;
            List<TextFile> files = new List<TextFile>();

            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "SELECT Id, FileName, Name, Description, Date, Size FROM TextFile" +
                " WHERE FileName like '%" + select + "%' or " +
                " Name like '%" + select + "%' or" +
                " Description like '%" + select + "%' or" +
                " Date like '%" + select + "%';";
                //" WHERE FileName like '%@select%';";

                IDbDataParameter parameter = dbCommand.CreateParameter();
		parameter.DbType = DbType.String;
                parameter.ParameterName = "@select";
                parameter.Value = select;

                dbCommand.Parameters.Add(parameter);

                /*dbCommand.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
                dbCommand.Parameters["@name"].Value = name;*/

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {

                    files.Add(new TextFile
                    (
                        Convert.ToInt32(reader["Id"]),
                        Convert.ToString(reader["FileName"]).Trim(),
                        Convert.ToString(reader["Name"]).Trim(),
                        Convert.ToString(reader["Description"]).Trim(),
                        Convert.ToDateTime(reader["Date"]),
                        null,
                        null,
                        null,
                        Convert.ToInt64(reader["Size"])
                    ));

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return files;
        }

        /// Получить список файлов для сравнения
        public List<TextFile> GetShingles()
        {
            CheckConnect();

            IDbCommand dbCommand = null;
            IDataReader reader = null;
            List<TextFile> files = new List<TextFile>();

		List<int> ids = new List<int>();
		List<string> names = new List<string>();

            List<int[]> array = new List<int[]>();
            List<string> serializeShaingles = new List<string>();
            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "SELECT Id, FileName, Shingles FROM TextFile;";

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    //array.Add(JsonConvert.DeserializeObject<int[]>(reader["Shingles"].ToString()));
                    serializeShaingles.Add(reader["Shingles"].ToString());
			ids.Add(Convert.ToInt32(reader["Id"]));
			names.Add(Convert.ToString(reader["FileName"]));
                }


                //Parallel.ForEach(serializeShaingles, shingles =>
		Parallel.For(0, ids.Count, (i) =>
                {
			files.Add(new TextFile
		            (
		                ids[i],
		                names[i],
		                JsonConvert.DeserializeObject<int[]>(serializeShaingles[i])
		            ));
                    //array.Add(JsonConvert.DeserializeObject<int[]>(shingles));
                });

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return files;
        }
        /// Получить список файлов для сравнения
        public List<int[]> GetShinglesArray()
        {
            CheckConnect();


            List<int[]> array = new List<int[]>();
            List<string> serializeShaingles = new List<string>();
            IDbCommand dbCommand = null;
            IDataReader reader = null;
            List<TextFile> files = new List<TextFile>();

            try
            {
                //dbCommand.Connection = Connection;
                dbCommand = Connection.CreateCommand();
                dbCommand.CommandText = "SELECT Id, FileName, Shingles FROM TextFile;";

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    //array.Add(JsonConvert.DeserializeObject<int[]>(reader["Shingles"].ToString()));
                    serializeShaingles.Add(reader["Shingles"].ToString());
		
                }

                Parallel.ForEach(serializeShaingles, shingles =>
                {
                    array.Add(JsonConvert.DeserializeObject<int[]>(shingles));
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                dbCommand = null;
            }

            return array;
        }



        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
