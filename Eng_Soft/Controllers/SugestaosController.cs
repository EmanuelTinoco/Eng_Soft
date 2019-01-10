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
    public class SugestaosController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Sugestaos
        public ActionResult Index()
        {
            var sugestao = db.Sugestao.Include(s => s.Utilizador);
            return View(sugestao.ToList());
        }

        // GET: Sugestaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugestao sugestao = db.Sugestao.Find(id);
            if (sugestao == null)
            {
                return HttpNotFound();
            }
            return View(sugestao);
        }

        // GET: Sugestaos/Create
        public ActionResult Create()
        {
            Sugestao s = new Sugestao();
            s.id_user = int.Parse(Request.Cookies["user"]["id"]);
            return View(s);
        }

        // POST: Sugestaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_user,data,descricao,id")] Sugestao sugestao)
        {
            sugestao.data = DateTime.Now;
            sugestao.id_user = int.Parse(Request.Cookies["user"]["id"]);
            if (ModelState.IsValid)
            {
                db.Sugestao.Add(sugestao);
                db.SaveChanges();
                return RedirectToAction("Index", "Home", null);
            }

            return View(sugestao);
        }

        // GET: Sugestaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugestao sugestao = db.Sugestao.Find(id);
            if (sugestao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", sugestao.id_user);
            return View(sugestao);
        }

        // POST: Sugestaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,data,descricao,id")] Sugestao sugestao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sugestao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", sugestao.id_user);
            return View(sugestao);
        }

        // GET: Sugestaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugestao sugestao = db.Sugestao.Find(id);
            if (sugestao == null)
            {
                return HttpNotFound();
            }
            return View(sugestao);
        }

        // POST: Sugestaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sugestao sugestao = db.Sugestao.Find(id);
            db.Sugestao.Remove(sugestao);
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
