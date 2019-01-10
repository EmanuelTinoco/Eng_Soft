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
    public class NotifcacaosController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Notifcacaos
        public ActionResult Index()
        {
            var notifcacao = db.Notifcacao.Include(n => n.Freguesia);
            return View(notifcacao.ToList());
        }

        // GET: Notifcacaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifcacao notifcacao = db.Notifcacao.Find(id);
            if (notifcacao == null)
            {
                return HttpNotFound();
            }
            return View(notifcacao);
        }

        // GET: Notifcacaos/Create
        public ActionResult Create()
        {
            return View(new Notifcacao());
        }

        // POST: Notifcacaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cod_postal,data,descricao")] Notifcacao notifcacao)
        {
            notifcacao.data = DateTime.Now;
            notifcacao.cod_postal = "4720";
            if (ModelState.IsValid)
            {
                db.Notifcacao.Add(notifcacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", notifcacao.cod_postal);
            return View(notifcacao);
        }

        // GET: Notifcacaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifcacao notifcacao = db.Notifcacao.Find(id);
            if (notifcacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", notifcacao.cod_postal);
            return View(notifcacao);
        }

        // POST: Notifcacaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cod_postal,data,descricao")] Notifcacao notifcacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notifcacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", notifcacao.cod_postal);
            return View(notifcacao);
        }

        // GET: Notifcacaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifcacao notifcacao = db.Notifcacao.Find(id);
            if (notifcacao == null)
            {
                return HttpNotFound();
            }
            return View(notifcacao);
        }

        // POST: Notifcacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notifcacao notifcacao = db.Notifcacao.Find(id);
            db.Notifcacao.Remove(notifcacao);
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
