using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD;

namespace Test.Controllers
{
    public class DeclaracoesController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Declaracoes
        public ActionResult Index()
        {
            var declaracao = db.Declaracao.Include(d => d.Utilizador);
            return View(declaracao.ToList());
        }

        // GET: Declaracoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            return View(declaracao);
        }

        // GET: Declaracoes/Create
        public ActionResult Create()
        {
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal");
            return View();
        }

        // POST: Declaracoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,ficheiro,data,descricao,id")] Declaracao declaracao)
        {
            if (ModelState.IsValid)
            {
                db.Declaracao.Add(declaracao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
            return View(declaracao);
        }

        // GET: Declaracoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
            return View(declaracao);
        }

        // POST: Declaracoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,ficheiro,data,descricao,id")] Declaracao declaracao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(declaracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
            return View(declaracao);
        }

        // GET: Declaracoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            return View(declaracao);
        }

        // POST: Declaracoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Declaracao declaracao = db.Declaracao.Find(id);
            db.Declaracao.Remove(declaracao);
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
