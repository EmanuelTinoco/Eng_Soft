using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD;

namespace EG.Controllers
{
    public class DeclaracaoController : Controller
    {

        private estp2Entities db = new estp2Entities();
        Declaracao d;
        // GET: Declaracao
        public ActionResult Index()
        {
            return View();
        }
    }
}