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
    public class Pedido_DeclaracaoController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Pedido_Declaracao
        public ActionResult Index()
        {
            var pedido_Declaracao = db.Pedido_Declaracao.Include(p => p.Utilizador);
            return View(pedido_Declaracao.ToList());
        }

        // GET: Pedido_Declaracao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Declaracao pedido_Declaracao = db.Pedido_Declaracao.Find(id);
            if (pedido_Declaracao == null)
            {
                return HttpNotFound();
            }
            return View(pedido_Declaracao);
        }

        // GET: Pedido_Declaracao/Create
        public ActionResult Create()
        {
            Pedido_Declaracao p = new Pedido_Declaracao();
            
            return View(p);
        }

        // POST: Pedido_Declaracao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,descricacao,data,id")] Pedido_Declaracao pedido_Declaracao)
        {
            if (ModelState.IsValid)
            {
                pedido_Declaracao.id_user = int.Parse(Request.Cookies["user"]["id"]);
                pedido_Declaracao.data = DateTime.Now;
                db.Pedido_Declaracao.Add(pedido_Declaracao);
                db.SaveChanges();
                return RedirectToAction("Index", "Home", null);
            }

            return View(pedido_Declaracao);
        }

        // GET: Pedido_Declaracao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Declaracao pedido_Declaracao = db.Pedido_Declaracao.Find(id);
            if (pedido_Declaracao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Declaracao.id_user);
            return View(pedido_Declaracao);
        }

        // POST: Pedido_Declaracao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,descricacao,data,id")] Pedido_Declaracao pedido_Declaracao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido_Declaracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", pedido_Declaracao.id_user);
            return View(pedido_Declaracao);
        }

        // GET: Pedido_Declaracao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido_Declaracao pedido_Declaracao = db.Pedido_Declaracao.Find(id);
            if (pedido_Declaracao == null)
            {
                return HttpNotFound();
            }
            return View(pedido_Declaracao);
        }

        // POST: Pedido_Declaracao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido_Declaracao pedido_Declaracao = db.Pedido_Declaracao.Find(id);
            db.Pedido_Declaracao.Remove(pedido_Declaracao);
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
