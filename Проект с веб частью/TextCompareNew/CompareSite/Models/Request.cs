using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Globalization;
using System.Threading.Tasks;


namespace CompareSite.Models
{
    public static class Request
    {

        /// Отправка строки POST
        public static string POST(string url, string data)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        /// Отправка файла POST
        public static string POSTnew(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            string response = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
                var content = new FormUrlEncodedContent(parameters);

                /*(new[]
                {
                    new KeyValuePair<string, string>("text", text)
                });*/
                using (var message = client.PostAsync(url, content).Result)
                {
                    response = message.Content.ReadAsStringAsync().Result;
                }
            }

            return response;
        }

        private static Stream generateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// Отправка файла POST
        public static string POST_Stream(string url, KeyValuePair<string, string>[] parameters)
        {
            string response = null;

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    using (Stream stream1 = generateStreamFromString(parameters[0].Value))
                    {
                        using (Stream stream2 = generateStreamFromString(parameters[1].Value))
                        {
                            StreamContent streamContent1 = new StreamContent(stream1);
                            streamContent1.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                            StreamContent streamContent2 = new StreamContent(stream2);
                            streamContent2.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                            content.Add(streamContent1, parameters[0].Key);
                            content.Add(streamContent2, parameters[1].Key);

                            using (var message = client.PostAsync(url, content).Result)
                            {
                                response = message.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                }
            }

            return response;
        }


        /// Отправка файла POST
        public static string POST(string url, IFormFile file)
        {
            string response = null;

            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        StreamContent streamContent = new StreamContent(stream);
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                        content.Add(streamContent, "file", file.FileName);

                        using (var message = client.PostAsync(url, content).Result)
                        {
                            response = message.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }

            return response;
        }

        /// Отправка строки GET
        public static string GET(string url, string data)
        {
            string responseString = null;
            WebRequest request = WebRequest.Create(url + "/" + data);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            response.Close();

            return responseString;
        }


        /// Отправка строки DELETE
        public static string DELETE(string url, string data)
        {
            string response = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");

                using (var message = client.DeleteAsync(url + "/" + data).Result)
                {
                    response = message.Content.ReadAsStringAsync().Result;
                }
            }

            return response;
        }

        /// Отправка строки PUT
        public static string PUT1(string url, string data)
        {
            string response = "";
            var httpContent = new StringContent(data);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");

                using (var message = client.PutAsync(url, httpContent).Result)
                {
                    response = message.Content.ReadAsStringAsync().Result;
                }
            }

            return response;
        }

        public static string PUT(string url, string data)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "PUT";
            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
        public static void PUT2(string url, string data)
        {
            using (var client = new HttpClient())
            {
                var answer = client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            }

            //return answer;
        }

        /*public static void POST2(string url, IFormFile file)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/file/index");

                var fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(file.OpenReadStream())
                    {
                        Headers =
                    {
            ContentLength = file.Length,
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType)
                    }
                    }, "File", fileName);

                    var response = client.PostAsync(url, content);
                }

            }

        }*/

        /*public static string POST1(string url, IFormFile file)
        {
            string result = null;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(file);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }*/

        /* public static string JsonPost(string url, string json)
         {
             var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
             httpWebRequest.ContentType = "application/json";
             httpWebRequest.Method = "POST";

             string result = null;

             using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
             {
                 streamWriter.Write(json);
                 streamWriter.Flush();
                 streamWriter.Close();
             }

             var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
             using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
             {
                 result = streamReader.ReadToEnd();
             }

             return result;
         }*/


        /*public static string postXMLData(string url, Dictionary<string,string> data)
        {
            XElement el = new XElement("root",data.Select(kv => new XElement(kv.Key, kv.Value)));

            string requestXml = el.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }


        private static string PostRequest3(string url, Dictionary<string,string> data)
        {
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";    

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{ \"text1\" : \"1111111\", \"params\" : [ \"Guru\" ], \"text2\" : \"55555\" }";
                    //string json = "";

                    //json = JsonConvert.SerializeObject(data);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);

                    return responseText;

                    //Now you have your response.
                    //or false depending on information in the response     
                }
            }
            catch(WebException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        private static string PostRequest2(string url, string data)
        {
            var cookies = new CookieContainer();
            ServicePointManager.Expect100Continue = false;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var requestStream = request.GetRequestStream())
            using (var writer = new StreamWriter(requestStream))
            {
                writer.Write(data);
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var result = reader.ReadToEnd();
                return result;
            }
        }*/
    }
}