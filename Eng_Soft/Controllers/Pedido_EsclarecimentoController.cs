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
    public class Pedido_EsclarecimentoController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Pedido_Esclarecimento
        public ActionResult Index()
        {
            var pedido_Esclarecimento = db.Pedido_Esclarecimento.Include(p => p.Utilizador).Include(p => p.Utilizador1);
            return View(pedido_Esclarecimento.ToList());
        }

        // GET: Pedido_Esclarecimento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Esclarecimento pedido_Esclarecimento = db.Pedido_Esclarecimento.Find(id);
            if (pedido_Esclarecimento == null)
            {
                return HttpNotFound();
            }
            return View(pedido_Esclarecimento);
        }

        // GET: Pedido_Esclarecimento/Create
        public ActionResult Create()
        {
            //ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal");
            //ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal");
            return View();
        }

        // POST: Pedido_Esclarecimento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "data,descricao")] Pedido_Esclarecimento pedido_Esclarecimento)
        {
            if (ModelState.IsValid)
            {
                pedido_Esclarecimento.id_user = 6;
                db.Pedido_Esclarecimento.Add(pedido_Esclarecimento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            return View(pedido_Esclarecimento);
        }

        // GET: Pedido_Esclarecimento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Esclarecimento pedido_Esclarecimento = db.Pedido_Esclarecimento.Find(id);
            if (pedido_Esclarecimento == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            return View(pedido_Esclarecimento);
        }

        // POST: Pedido_Esclarecimento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,data,descricao,id")] Pedido_Esclarecimento pedido_Esclarecimento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido_Esclarecimento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Esclarecimento.id_user);
            return View(pedido_Esclarecimento);
        }

        // GET: Pedido_Esclarecimento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Esclarecimento pedido_Esclarecimento = db.Pedido_Esclarecimento.Find(id);
            if (pedido_Esclarecimento == null)
            {
                return HttpNotFound();
            }
            return View(pedido_Esclarecimento);
        }

        // POST: Pedido_Esclarecimento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido_Esclarecimento pedido_Esclarecimento = db.Pedido_Esclarecimento.Find(id);
            db.Pedido_Esclarecimento.Remove(pedido_Esclarecimento);
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
