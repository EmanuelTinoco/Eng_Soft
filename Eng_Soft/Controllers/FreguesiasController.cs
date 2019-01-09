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
    public class FreguesiasController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Freguesias
        public ActionResult Index()
        {
            return View(db.Freguesia.ToList());
        }

        // GET: Freguesias/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freguesia freguesia = db.Freguesia.Find(id);
            if (freguesia == null)
            {
                return HttpNotFound();
            }
            return View(freguesia);
        }

        // GET: Freguesias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Freguesias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cod_postal,nome,morada")] Freguesia freguesia)
        {
            if (ModelState.IsValid)
            {
                db.Freguesia.Add(freguesia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(freguesia);
        }

        // GET: Freguesias/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freguesia freguesia = db.Freguesia.Find(id);
            if (freguesia == null)
            {
                return HttpNotFound();
            }
            return View(freguesia);
        }

        // POST: Freguesias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cod_postal,nome,morada")] Freguesia freguesia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freguesia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(freguesia);
        }

        // GET: Freguesias/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freguesia freguesia = db.Freguesia.Find(id);
            if (freguesia == null)
            {
                return HttpNotFound();
            }
            return View(freguesia);
        }

        // POST: Freguesias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Freguesia freguesia = db.Freguesia.Find(id);
            db.Freguesia.Remove(freguesia);
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
