using Eng_Soft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eng_Soft.Controllers
{
    public class WeatherController : Controller
    {
        public JsonResult GetWeather(string localidade)
        {
            Weather weath = new Weather();
            return Json(weath.getWeather(localidade), JsonRequestBehavior.AllowGet);
        }
    }
}