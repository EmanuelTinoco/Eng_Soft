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
        public ActionResult Index()
        {
            int id = 6;
            Permicoes p = new Permicoes();
            p.Id = id;
            p.Admin = isAdmin(id);
            p.Residente = isResidente(id);
            p.Membro = isMembro(id);
            return View(p);
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