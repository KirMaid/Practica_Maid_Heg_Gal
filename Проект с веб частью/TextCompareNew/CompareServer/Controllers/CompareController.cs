using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompareServer.Models;

namespace CompareServer.Controllers
{
    [Route("api/[controller]")]
    public class CompareController : Controller
    {
        /*// GET api/compare/two-texts
        [HttpGet]
        [Route("two-texts/{text1}/{text2}")]
        public string CompareTwoTextsGet(string text1, string text2)
        {
            return LibRepository.CompareTwoTexts(text1, text2);
        }*/

        // POST api/compare/two-texts
        [HttpPost]
        [Route("two-texts")]
        public string CompareTwoTexts(string text1, string text2)
        {
            return LibRepository.CompareTwoTexts(text1, text2);
        }


        [HttpPost]
        [Route("two-texts-random")]
        public string CompareTwoTextsRandom(string text1, string text2)
        {
            return LibRepository.CompareTwoTextsRandom(text1, text2);
        }

        [HttpPost]
        [Route("two-texts-max")]
        public string CompareTwoTextsMax(string text1, string text2)
        {
            return LibRepository.CompareTwoTextsMax(text1, text2);
        }


        [HttpPost]
        [Route("two-texts-min")]
        public string CompareTwoTextsMin(string text1, string text2)
        {
            return LibRepository.CompareTwoTextsMin(text1, text2);
        }






            /*// GET api/compare/text-with-base
            [HttpGet]
            [Route("text-with-base")]
            public JsonResult CompareTextWithBaseGet(string text)
            {
                return Json(LibRepository.CompareTextWithBase(text));
            }*/

            // POST api/compare/text-with-base
            [HttpPost]
        [Route("text-with-base")]
        public JsonResult CompareTextWithBase(string text)
        {
            List<CompareResult> result = null;
            try
            {
                result = LibRepository.CompareTextWithBase(text);

		//result = LibRepository.CompareTextWithBase_DB(text);
                //result = LibRepository.CompareTextWithBase_Files(text);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(result);
        }
    }
}
