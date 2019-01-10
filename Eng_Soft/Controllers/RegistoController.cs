using Eng_Soft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Eng_Soft.Controllers
{
    public class RegistoController : Controller
    {
        // GET: Registo
        public ActionResult Index()
        {
            IEnumerable<Registo> regists = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55238/api/");
                //HTTP GET
                string id = Request.Cookies["user"]["id"];
                var responseTask = client.GetAsync("registos/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Registo>>();
                    readTask.Wait();

                    regists = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    regists = Enumerable.Empty<Registo>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(regists);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55238/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("registos/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public bool Edit(int id)
        {
            bool b = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55238/api/");
                //HTTP GET
                var responseTask = client.GetAsync("registo?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    b = true;
                }
            }

            return b;
        }

        //public ActionResult Edit(int id)
        //{
        //    Registo registo = null;

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:55238/api/");
        //        //HTTP GET
        //        var responseTask = client.GetAsync("registo?id=" + id.ToString());
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<Registo>();
        //            readTask.Wait();

        //            registo = readTask.Result;
        //        }
        //    }

        //    return View(registo);
        //}
    }
}