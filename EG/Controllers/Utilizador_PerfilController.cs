using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD;
using EG.Models;

namespace EG.Controllers
{
    public class Utilizador_PerfilController : Controller
    {
        private static estp2Entities db = new estp2Entities();

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

        public static bool adiciona(int user_id, int perfil_id)
        {
            Utilizador_Perfil u = new Utilizador_Perfil();
            u.user_id = user_id;
            u.perfil_id = perfil_id;
            u.data = DateTime.Now;
            u.ativo = 1;
            db.Utilizador_Perfil.Add(u);
            db.SaveChanges();
            return true;
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
        public ActionResult Edit(int id)
        {
            //bool admin, residente, membro;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilizador user = db.Utilizador.Find(id);
            if (user == null)
            {
                HttpNotFound();
                return null;
            }
            Permicoes p = new Permicoes(id, isAdmin(id), isResidente(id), isMembro(id));
            return View();
            Utilizador_Perfil user_perfil = db.Utilizador_Perfil.Find(id, 3);//este utilizador tem sempre que existir, porque é nao residente
            user_perfil.admin = ViewBag.admin = isAdmin(id);
            user_perfil.residente = ViewBag.residente = isResidente(id);
            user_perfil.membro = ViewBag.membro = isMembro(id);
            
            return View();
        }

        

        // POST: Utilizador_Perfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id, perfil_id, data, ativo, admin, residente, membro")] Utilizador_Perfil user_perfil)
        {
            //user_perfil.user_id = int.Parse(IdentidadeManager.IdentidadeUser.GetId());
            //user_perfil.user_id = 11;
            changePermission(user_perfil.user_id, 1, user_perfil.admin);
            changePermission(user_perfil.user_id, 2, user_perfil.residente);
            changePermission(user_perfil.user_id, 4, user_perfil.membro);
            
            ViewBag.perfil_id = new SelectList(db.Utilizador_Perfil, "id_user", "ID tipo Utilizador", user_perfil.perfil_id);
            ViewBag.user_id = new SelectList(db.Utilizador_Perfil, "id", "ID", user_perfil.user_id);
            ViewBag.admin = new SelectList(db.Utilizador_Perfil, "admin", "Admin", user_perfil.admin);
            ViewBag.residente = new SelectList(db.Utilizador_Perfil, "residente", "Residente", user_perfil.admin);
            ViewBag.membro = new SelectList(db.Utilizador_Perfil, "membro", "Membro da Junta", user_perfil.admin);
            return View("Edit");
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

        public Utilizador_Perfil PermissionExists(int user_id, int perfil_id)
        {
            Utilizador_Perfil user = db.Utilizador_Perfil.Find(user_id, perfil_id);
            
            return user;
        }

        private bool isAdmin(int id)
        {
            var perm = (from user in db.Utilizador
                        join user_p in db.Utilizador_Perfil on user.id equals user_p.user_id
                        where user.id == id && user_p.ativo == 1 && user_p.perfil_id == 1
                        select new
                        {
                            PERM = user_p.perfil_id
                        }).FirstOrDefault();
            if (perm == null)
            {
                return false;
            }
            return true;
        }

        private bool isResidente(int id)
        {
            var perm = (from user in db.Utilizador
                        join user_p in db.Utilizador_Perfil on user.id equals user_p.user_id
                        where user.id == id && user_p.ativo == 1 && user_p.perfil_id == 2
                        select new
                        {
                            PERM = user_p.perfil_id
                        }).FirstOrDefault();
            if (perm == null)
            {
                return false;
            }
            return true;
        }

        private bool isMembro(int id)
        {
            var perm = (from user in db.Utilizador
                        join user_p in db.Utilizador_Perfil on user.id equals user_p.user_id
                        where user.id == id && user_p.ativo == 1 && user_p.perfil_id == 4
                        select new
                        {
                            PERM = user_p.perfil_id
                        }).FirstOrDefault();
            if (perm == null)
            {
                return false;
            }
            return true;
        }


        public bool changePermission(int u_id, int p_id, bool status)
        {
            Utilizador_Perfil user = PermissionExists(u_id, p_id);
            int s = status ? 1 : 0;
            //Se existir faço update do status
            if (user != null)
            {
                user = (from x in db.Utilizador_Perfil
                                          where x.user_id == u_id && x.perfil_id == p_id
                                          select x).First();
                user.ativo = s;
                db.SaveChanges();
            }
            //se não existir é adicionada uma nova entrada a tabela
            else
            {
                using (var context = new estp2Entities())
                {
                    context.Utilizador_Perfil.Add(new Utilizador_Perfil { user_id = u_id, perfil_id = p_id,  ativo = s, data = DateTime.Now });
                    //var users = db.Set<Utilizador_Perfil>();
                    //users.Add(new Utilizador_Perfil { perfil_id = p_id, user_id = u_id, ativo = s, data = DateTime.Now });
                    context.SaveChanges();
                    //db.SaveChanges();
                }
            }
            return true;
        }
    }
}
