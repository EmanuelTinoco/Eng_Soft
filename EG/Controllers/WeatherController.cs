using EG.Models;
using System.Web.Mvc;

namespace EG.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public JsonResult GetWeather(string localidade)
        {
            Weather weath = new Weather();
            return Json(weath.getWeather(localidade), JsonRequestBehavior.AllowGet);
        }

    }
}