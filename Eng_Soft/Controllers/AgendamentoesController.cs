using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD;

namespace Eng_Soft.Controllers
{
    public class AgendamentoesController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Agendamentoes
        public ActionResult Index()
        {
            var agendamento = db.Agendamento.Include(a => a.Utilizador).Include(a => a.Utilizador1);
            return View(agendamento.ToList());
        }

        // GET: Agendamentoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agendamento agendamento = db.Agendamento.Find(id);
            if (agendamento == null)
            {
                return HttpNotFound();
            }
            return View(agendamento);
        }

        // GET: Agendamentoes/Create
        public ActionResult Create(int id)
        {
            Agendamento a = new Agendamento();
            a.id_user = id;
            return View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,data,objetivo,id")] Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                db.Agendamento.Add(agendamento);
                db.SaveChanges();
                return RedirectToAction("Index","Home",null);
            }

            return View(agendamento);
        }
        

        // GET: Agendamentoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agendamento agendamento = db.Agendamento.Find(id);
            if (agendamento == null)
            {
                return HttpNotFound();
            }
            return View(agendamento);
        }

        // POST: Agendamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agendamento agendamento = db.Agendamento.Find(id);
            db.Agendamento.Remove(agendamento);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
