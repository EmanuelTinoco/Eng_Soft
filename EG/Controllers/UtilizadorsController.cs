using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD;

namespace EG.Controllers
{
    public class UtilizadorsController : Controller
    {
        private estp2Entities db = new estp2Entities();

        // GET: Utilizadors
        public ActionResult Index()
        {
            var utilizador = db.Utilizador.Include(u => u.Agendamento).Include(u => u.Agendamento1).Include(u => u.Declaracao).Include(u => u.Freguesia).Include(u => u.Pedido_Declaracao).Include(u => u.Pedido_Esclarecimento).Include(u => u.Pedido_Esclarecimento1).Include(u => u.Reportar_Problema).Include(u => u.Reportar_Problema1).Include(u => u.Sugestao);
            return View(utilizador.ToList());
        }

        // GET: Utilizadors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador utilizador = db.Utilizador.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            return View(utilizador);
        }

        // GET: Utilizadors/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo");
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo");
            ViewBag.id = new SelectList(db.Declaracao, "id", "descricao");
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome");
            ViewBag.id = new SelectList(db.Pedido_Declaracao, "id", "descricacao");
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao");
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao");
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao");
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao");
            ViewBag.id = new SelectList(db.Sugestao, "id_user", "descricao");
            return View();
        }

        // POST: Utilizadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cod_postal,nome,cc,n_eleitor,email,username,password,contacto")] Utilizador utilizador)
        {
            int perfil_id = 3;
            if (ModelState.IsValid)
            {
                db.Utilizador.Add(utilizador);

                if (utilizador.cod_postal.Equals("Amares"))
                {
                    perfil_id = 2;
                }
                
                db.SaveChanges();
                Utilizador_PerfilController.adiciona(utilizador.id, perfil_id);
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Declaracao, "id", "descricao", utilizador.id);
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", utilizador.cod_postal);
            ViewBag.id = new SelectList(db.Pedido_Declaracao, "id", "descricacao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Sugestao, "id_user", "descricao", utilizador.id);
            return View(utilizador);
        }

        // GET: Utilizadors/Edit/5
        public ActionResult Edit(int? id)
        {
            Console.WriteLine(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador utilizador = db.Utilizador.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Declaracao, "id", "descricao", utilizador.id);
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", utilizador.cod_postal);
            ViewBag.id = new SelectList(db.Pedido_Declaracao, "id", "descricacao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Sugestao, "id_user", "descricao", utilizador.id);
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cod_postal,nome,cc,n_eleitor,email,username,password,contacto")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Agendamento, "id", "objetivo", utilizador.id);
            ViewBag.id = new SelectList(db.Declaracao, "id", "descricao", utilizador.id);
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", utilizador.cod_postal);
            ViewBag.id = new SelectList(db.Pedido_Declaracao, "id", "descricacao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Pedido_Esclarecimento, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Reportar_Problema, "id", "descricao", utilizador.id);
            ViewBag.id = new SelectList(db.Sugestao, "id_user", "descricao", utilizador.id);
            return View(utilizador);
        }

        // GET: Utilizadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador utilizador = db.Utilizador.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilizador utilizador = db.Utilizador.Find(id);
            db.Utilizador.Remove(utilizador);
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
