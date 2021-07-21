using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompareServer.Models;
using Microsoft.AspNetCore.Http;

namespace CompareServer.Controllers
{

    [Route("api/")]
    public class FilesController : Controller
    {
        /// GET api/file/
        [HttpGet]
        [Route("[controller]/{id:int}")]
        public JsonResult Get(int id)
        {
            TextFile textFile = null;

            try
            {
                textFile = new TextFile(id, 1);
                textFile.GetContent();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(textFile);
        }

        /// GET api/file/
        [HttpGet]
        [Route("[controller]/find/{select}")]
        public JsonResult Get(string select)
        {
            List<TextFile> files = null;

            try
            {
                using (var dbProvider = new DbProvider(Startup.ConnectionString))
                    files = dbProvider.Find(select);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(files);
        }

/// POST api/file/
[HttpPost]
[Route("[controller]")]
public JsonResult Post(IFormFile file)
{
    TextFile textFile = null;
    try
    {
        // Сохранение полей из IFormFile
        textFile = new TextFile(file);
        // Сохранение файла в базу
        int id = textFile.Save();
        textFile = new TextFile(id, 2);
    }
    catch (Exception ex)
    {
        return Json(new { error = ex.Message });
    }

    return Json(textFile);
}


        // PUT api/File
        [HttpPut]
        [Route("[controller]")]
        public JsonResult Put(int id, string name, string description)
        {
            int count = 0;
            TextFile textFile = null;
            try
            {
                textFile = new TextFile(id, 1);
                count = textFile.Update(name, description);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { success = count });
        }

        // DELETE api/File
        //[HttpDelete("{id}")]
        [HttpDelete]
        [Route("[controller]/{id:int}")]
        public JsonResult Delete(int id)
        {
            TextFile textFile = null;
            int count = 0;
            try
            {
                // Получение файла по id
                textFile = new TextFile(id, 1);
                // Удаление файла
                count = textFile.Delete();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(new { success = count });
        }
    }
}
