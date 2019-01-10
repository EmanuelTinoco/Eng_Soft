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
    public class DeclaracaosController : Controller
    {
        private estp2Entities db = new estp2Entities();

        
        public ActionResult ShowDocs()
        {
            return View(db.Declaracao.ToList());
        }

        public ActionResult Index()
        {
            int id = int.Parse(Request.Cookies["user"]["id"]);
            var dec = db.Declaracao.Where(d => d.id_user == id);
            return View(dec.ToList());
        }

        // GET: Declaracaos/Details/5
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

        

        // GET: Declaracaos/Create
        public ActionResult Create(int id)
        {
            Declaracao d = new Declaracao();
            d.id_user = id;
            return View(d);
        }

        [HttpPost]
        public ActionResult Create(Declaracao model, HttpPostedFileBase file1)
        {
            if (file1 != null)
            {
                model.id_user = model.id;
                model.id = 0;
                model.data = DateTime.Now;
                model.ficheiro = new byte[file1.ContentLength];
                file1.InputStream.Read(model.ficheiro, 0, file1.ContentLength);

            }
            db.Declaracao.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Home", null);
        }

         
        [HttpGet]
        public FileResult Download(int id)
        {
            var file = (from d in db.Declaracao
                        where d.id == id
                        select new { d.descricao, d.ficheiro }).ToList().FirstOrDefault();
            byte[] fileBytes = file.ficheiro;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.descricao);
        }

    
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

        // POST: Declaracaos/Edit/5
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

        // GET: Declaracaos/Delete/5
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

        // POST: Declaracaos/Delete/5
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
