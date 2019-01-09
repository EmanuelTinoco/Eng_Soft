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
    public class Utilizador_PerfilController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Utilizador_Perfil
        public ActionResult Index()
        {
            var utilizador_Perfil = db.Utilizador_Perfil.Include(u => u.Perfil).Include(u => u.Utilizador);
            return View(utilizador_Perfil.ToList());
        }

        // GET: Utilizador_Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador_Perfil utilizador_Perfil = db.Utilizador_Perfil.Find(id);
            if (utilizador_Perfil == null)
            {
                return HttpNotFound();
            }
            return View(utilizador_Perfil);
        }

        // GET: Utilizador_Perfil/Create
        public ActionResult Create()
        {
            ViewBag.perfil_id = new SelectList(db.Perfil, "id_user", "tipo_utilizador");
            ViewBag.user_id = new SelectList(db.Utilizador, "id", "cod_postal");
            return View();
        }

        // POST: Utilizador_Perfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,perfil_id,data,ativo")] Utilizador_Perfil utilizador_Perfil)
        {
            if (ModelState.IsValid)
            {
                db.Utilizador_Perfil.Add(utilizador_Perfil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.perfil_id = new SelectList(db.Perfil, "id_user", "tipo_utilizador", utilizador_Perfil.perfil_id);
            ViewBag.user_id = new SelectList(db.Utilizador, "id", "cod_postal", utilizador_Perfil.user_id);
            return View(utilizador_Perfil);
        }

        // GET: Utilizador_Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador_Perfil utilizador_Perfil = db.Utilizador_Perfil.Find(id);
            if (utilizador_Perfil == null)
            {
                return HttpNotFound();
            }
            ViewBag.perfil_id = new SelectList(db.Perfil, "id_user", "tipo_utilizador", utilizador_Perfil.perfil_id);
            ViewBag.user_id = new SelectList(db.Utilizador, "id", "cod_postal", utilizador_Perfil.user_id);
            return View(utilizador_Perfil);
        }

        // POST: Utilizador_Perfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,perfil_id,data,ativo")] Utilizador_Perfil utilizador_Perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilizador_Perfil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.perfil_id = new SelectList(db.Perfil, "id_user", "tipo_utilizador", utilizador_Perfil.perfil_id);
            ViewBag.user_id = new SelectList(db.Utilizador, "id", "cod_postal", utilizador_Perfil.user_id);
            return View(utilizador_Perfil);
        }

        // GET: Utilizador_Perfil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador_Perfil utilizador_Perfil = db.Utilizador_Perfil.Find(id);
            if (utilizador_Perfil == null)
            {
                return HttpNotFound();
            }
            return View(utilizador_Perfil);
        }

        // POST: Utilizador_Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilizador_Perfil utilizador_Perfil = db.Utilizador_Perfil.Find(id);
            db.Utilizador_Perfil.Remove(utilizador_Perfil);
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
