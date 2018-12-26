using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EG.Models;
using BD;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentidadeManager;

namespace EG.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Private Properties    
        /// <summary>  
        /// Database Store property.    
        /// </summary>  

        estp2Entities db = new estp2Entities();
        #endregion
        #region Default Constructor    
        /// <summary>  
        /// Initializes a new instance of the <see cref="AccountController" /> class.    
        /// </summary>  
        public AccountController()
        {
        }
        #endregion
        #region Login methods    
        /// <summary>  
        /// GET: /Account/Login    
        /// </summary>  
        /// <param name="returnUrl">Return URL parameter</param>  
        /// <returns>Return login view</returns>  
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.    
                if (this.Request.IsAuthenticated)
                {
                    // Info.    
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            // Info.    
            return this.View();
        }
        /// <summary>  
        /// POST: /Account/Login    
        /// </summary>  
        /// <param name="model">Model parameter</param>  
        /// <param name="returnUrl">Return URL parameter</param>  
        /// <returns>Return login view</returns>  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utilizador u, string returnUrl)
        {
            try
            {
                // Verification.    
                if (ModelState.IsValid)
                {
                    // Initialization.    
                    // var loginInfo = this.databaseManager.LoginByUsernamePassword(model.Username, model.Password).ToList();
                    // Verification.    
                    var v = db.Utilizador.Where(a => a.username.Equals(u.username) && a.password.Equals(u.password)).FirstOrDefault();
                    if (v != null)
                    {
                        // Initialization.    
                       
                        // Login In.    
                        this.SignInUser(u.username, false);
                        // Info.
                        IdentidadeUser.AddIdentidadeUser(v.username, v.id.ToString(), Get_Permissions(v.id));
                        Get_Permissions(v.id);
                        return this.RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        // Setting.    
                        ModelState.AddModelError(string.Empty, "Username ou password errado.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            // If we got this far, something failed, redisplay form    
            return this.View(u);
        }

        
        private bool[] Get_Permissions(int id)
        {
            bool[] perm = { false, false, false };
            var permissions = (from user in db.Utilizador
                               join user_p in db.Utilizador_Perfil on user.id equals user_p.user_id
                               where user.id == id && user_p.ativo == 1
                               select new {
                                   PERM = user_p.perfil_id
                               }).Take(4);
            foreach (var p in permissions)
            {
                if(p.PERM == 1)
                {
                    perm[0] = true;
                }
                if(p.PERM == 2)
                {
                    perm[1] = true;
                }
                if(p.PERM == 4)
                {
                    perm[2] = true;
                }
            }

            return perm;
           
        }
        


        #endregion
        #region Log Out method.    
        /// <summary>  
        /// POST: /Account/LogOff    
        /// </summary>  
        /// <returns>Return log off action</returns>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign Out.    
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }

        #endregion
        #region Helpers    
        #region Sign In method.    
        /// <summary>  
        /// Sign In User method.    
        /// </summary>  
        /// <param name="username">Username parameter.</param>  
        /// <param name="isPersistent">Is persistent parameter.</param>  
        private void SignInUser(string username, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                // Setting    
                claims.Add(new Claim(ClaimTypes.Name, username));

                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #endregion
        #region Redirect to local method.    
        /// <summary>  
        /// Redirect to local method.    
        /// </summary>  
        /// <param name="returnUrl">Return URL parameter.</param>  
        /// <returns>Return redirection action</returns>  
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : ActionResult
        {
            private string provider;
            private string v1;
            private string v2;

            public ChallengeResult(string provider, string v1, string v2)
            {
                this.provider = provider;
                this.v1 = v1;
                this.v2 = v2;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #endregion
    }
}  
   