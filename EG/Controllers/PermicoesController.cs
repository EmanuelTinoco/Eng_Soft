using BD;
using EG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EG.Controllers
{
    public class PermicoesController : Controller
    {
        private static estp2Entities db = new estp2Entities();

        
        // GET: Permicoes
        public ActionResult Index(int id)
        {
            Permicoes p = new Permicoes();
            p.Id = id;
            p.Admin = isAdmin(id);
            p.Residente = isResidente(id);
            p.Membro = isMembro(id);
            return View(p);
        }

        public ActionResult Edit(int id)
        {
            Permicoes p = new Permicoes();
            p.Id = id;
            p.Admin = isAdmin(id);
            p.Residente = isResidente(id);
            p.Membro = isMembro(id);
            return View(p);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, admin, residente, membro")] Permicoes p)
        {
            changePermission(p.Id, 1, p.Admin);
            changePermission(p.Id, 2, p.Residente);
            changePermission(p.Id, 4, p.Membro);
            return View(p);
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
                    context.Utilizador_Perfil.Add(new Utilizador_Perfil { user_id = u_id, perfil_id = p_id, ativo = s, data = DateTime.Now });
                    //var users = db.Set<Utilizador_Perfil>();
                    //users.Add(new Utilizador_Perfil { perfil_id = p_id, user_id = u_id, ativo = s, data = DateTime.Now });
                    context.SaveChanges();
                    //db.SaveChanges();
                }
            }
            return true;
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
    }
}