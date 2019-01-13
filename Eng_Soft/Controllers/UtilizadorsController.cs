using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BD;

namespace Eng_Soft.Controllers
{
    public class UtilizadorsController : Controller
    {
        private estp2Entities db = new estp2Entities();


        public ActionResult UsersView()
        {
            var users = db.Utilizador.ToList();
            return View(users);
        }


        // GET: Utilizadors
        public ActionResult Index()
        {
            var utilizador = db.Utilizador.Include(u => u.Freguesia);
            return View(utilizador.ToList());
        }

        public ActionResult EditMyProfile()
        {
            int id = int.Parse(Request.Cookies["user"]["id"]);
            Utilizador utilizador = db.Utilizador.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            return View(utilizador);
        }

        [HttpPost]
        public ActionResult EditMyProfile([Bind(Include = "id,cod_postal,nome,cc,n_eleitor,email,username,password,contacto")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilizador).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utilizador);
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

                if (utilizador.cod_postal.Equals("4720"))
                {
                    perfil_id = 2;
                }

                db.SaveChanges();
                addRegistoAtividade(utilizador.id, utilizador.username, utilizador.email);

                Utilizador_PerfilController.adiciona(utilizador.id, perfil_id);
                return RedirectToAction("Login", "Account", new { area = "" });
                //return RedirectToAction("Index");
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

        //Adiciona user registoAtividade
        private int addRegistoAtividade(int id, string username, string email)
        {

            int regist = -1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://isiapi20190113111536.azurewebsites.net/api/");

                var data = new
                {
                    id = id,
                    email = email,
                    username = username
                };
                var response = client.PostAsJsonAsync("user", data);
                response.Wait();
                var result = response.Result;
                
            }
            return regist;
        }
        // GET: Utilizadors/Edit/5
        public ActionResult Edit(int? id)
        {
            id = int.Parse(Request.Cookies["user"]["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador utilizador = db.Utilizador.Find(id);
            if (utilizador == null)
            {
                return HttpNotFound();
            }
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", utilizador.cod_postal);
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
                db.Entry(utilizador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home", null);
            }
            ViewBag.cod_postal = new SelectList(db.Freguesia, "cod_postal", "nome", utilizador.cod_postal);
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
