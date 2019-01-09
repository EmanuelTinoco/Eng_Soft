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
    public class Reportar_ProblemaController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Reportar_Problema
        public ActionResult Index()
        {
            int id = int.Parse(Request.Cookies["user"]["id"]);
            var problema = db.Reportar_Problema.Where(p => p.id_user == id);
            return View(problema.ToList());
        }

        // GET: Declaracaos/Create
        public ActionResult Create()//adicionar id e assinar ao valor de r
        {
            Reportar_Problema r = new Reportar_Problema();
            r.id_user = int.Parse(Response.Cookies["cookie"]["id"]);
            return View(r);
        }

        [HttpPost]
        public ActionResult Create(Reportar_Problema model, HttpPostedFileBase file1)
        {
            if (file1 != null)
            {
                model.foto = new byte[file1.ContentLength];
                file1.InputStream.Read(model.foto, 0, file1.ContentLength);

            }
            db.Reportar_Problema.Add(model);
            db.SaveChanges();
            return View(model);
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            var file = (from d in db.Reportar_Problema
                        where d.id == id
                        select new { d.descricao, d.foto }).ToList().FirstOrDefault();
            byte[] fileBytes = file.foto;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.descricao);
        }

        // GET: Reportar_Problema/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reportar_Problema reportar_Problema = db.Reportar_Problema.Find(id);
            if (reportar_Problema == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", reportar_Problema.id_user);
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", reportar_Problema.id_user);
            return View(reportar_Problema);
        }

        // POST: Reportar_Problema/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,status,descricao,localizacao,id,foto")] Reportar_Problema reportar_Problema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportar_Problema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", reportar_Problema.id_user);
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", reportar_Problema.id_user);
            return View(reportar_Problema);
        }

        // GET: Reportar_Problema/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reportar_Problema reportar_Problema = db.Reportar_Problema.Find(id);
            if (reportar_Problema == null)
            {
                return HttpNotFound();
            }
            return View(reportar_Problema);
        }

        // POST: Reportar_Problema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reportar_Problema reportar_Problema = db.Reportar_Problema.Find(id);
            db.Reportar_Problema.Remove(reportar_Problema);
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
