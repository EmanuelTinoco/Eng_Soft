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
using Eng_Soft.Models;
using BD;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Net;
using System.Net.Mail;

using Facebook;
using System.Net.Http;

using Newtonsoft.Json;


namespace Eng_Soft.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        estp2Entities db = new estp2Entities();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public static string EconfUser { get; set; }
        public static string OEmail { get; set; }
        public static string ODataNascimento { get; set; }
        public static string OFnome { get; set; }
        public static string OLnome { get; set; }

        /// <summary>  
        /// GET: /Account/Login    
        /// </summary>  
        /// <param name="returnUrl">Return URL parameter</param>  
        /// <returns>Return login view</returns>  
        [AllowAnonymous]
        public ActionResult RecuperarPassWord(string returnUrl)
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarPassWord(string email, string returnUrl)
        {
            try
            {
                // Verification.    
                if (ModelState.IsValid)
                {

                    var v = db.Utilizador.Where(a => a.email.Equals(email)).FirstOrDefault();
                    if (v != null)
                    {
                        SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
                        cliente.EnableSsl = true;
                        cliente.UseDefaultCredentials = false;
                        cliente.Timeout = 50000;
                        cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                        cliente.Credentials = new NetworkCredential("CM.Barcelos.ES@gmail.com", "engenhariasoftwareRED");

                        MailMessage msg = new MailMessage();
                        msg.To.Add(v.email);
                        msg.From = new MailAddress("CM.Barcelos.ES@gmail.com");
                        msg.Subject = "Recuperaçao de email";
                        msg.Body = ("A sua password de acesso a platafomra da CM Barcelos é: " + v.password);
                        cliente.Send(msg);

                        return this.View("RecuperarPassWordConfirmacao");
                    }
                    else
                    {
                        // Setting.    
                        ModelState.AddModelError(string.Empty, "Email invalido.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            return this.View();
        }


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
                    var v = db.Utilizador.Where(a => a.username.Equals(u.username) && a.password.Equals(u.password)).FirstOrDefault();
                    if (v != null)
                    {
                        // Initialization.    

                        // Login In.    
                        this.SignInUser(u.username, false);
                        // Info.
                        HttpCookie cookie = new HttpCookie("user");
                        cookie["id"] = v.id.ToString();
                        cookie["admin"] = isAdmin(v.id).ToString();
                        cookie["residente"] = isResidente(v.id).ToString();
                        cookie["membro"] = isMembro(v.id).ToString();
                        cookie["id_registo"] = GetIdRegist(v.id).ToString();
                        Response.Cookies.Add(cookie);


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

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
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

        private int GetIdRegist(int id)
        {
            int regist = -1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:55238/api/");
                var data = new
                {
                    Userid = id,
                    Data_Hora_Login = DateTime.Now,
                    Data_Hora_Logoff = DateTime.Now
                };

                var response = client.PostAsJsonAsync("registos", data);
                response.Wait();
                char[] delimiterChars = { '"', ':', '{', '}', ',' };
                var contentString = response.Result.Content.ReadAsStringAsync().Result;

                //adicionar cookie com o valor do id_registo
                regist = int.Parse(contentString.Split(delimiterChars)[4]);
            }
            return regist;
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl = null)
        {

            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string userproKey = info.Login.ProviderKey;

            //Procuro na BD se o user ja eiste
            var user = db.Utilizador.Where(a => a.email.Equals(info.Email.ToString()) && a.password.Equals(info.Login.ProviderKey.ToString())).FirstOrDefault();
            //se Exisitir entra
            if (user != null)
            {
                this.SignInUser(user.username, false);
                HttpCookie cookie = new HttpCookie("user");
                cookie["id"] = user.id.ToString();
                cookie["admin"] = isAdmin(user.id).ToString();
                cookie["residente"] = isResidente(user.id).ToString();
                cookie["membro"] = isMembro(user.id).ToString();
               // cookie["id_registo"] = GetIdRegist(user.id).ToString();
                Response.Cookies.Add(cookie);
                return this.RedirectToAction("Index", "Home");
            }
            //
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = info.Login.LoginProvider;

                if (info.Login.LoginProvider == "Google")
                {
                    OEmail = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                    //OFnome = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "https://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenName").Value;
                    //OLnome = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "https://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
                }

                else if (info.Login.LoginProvider == "Facebook")
                {
                    var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                    var access_token = identity.FindFirstValue("FacebookAccessToken");
                   // var fb = new FacebookClient(access_token);



                    //dynamic me = fb.Get("me");
                    //string firstName = me.first_name;
                    //string lastName = me.last_name;
                    //string email = me.email;

                    //dynamic uEmail = fb.Get("/m?fields=email");
                    //dynamic uNome = fb.Get("/m?fields=first_name");
                    //dynamic uApelido = fb.Get("/m?fields=last_name");

                    OEmail = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                    OFnome = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                    //OFnome = firstName;
                    //OLnome = lastName;

                }
                else
                {
                    OEmail = null;
                    OFnome = null;
                    OLnome = null;
                }
                return View("ExternalLoginConfirmation");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            // Obter as informações sobre o usuário do provedor de logon externo
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return View("ExternalLoginFailure");
            }

            var user = new Utilizador
            {
                password = info.Login.ProviderKey,
                email = info.Email,
                username = info.DefaultUserName.ToString(),
                cc = model.cc,
                nome = model.nome,
                n_eleitor = model.n_eleitor,
                contacto = model.contacto,
                cod_postal = "4720"

            };

            int perfil_id = 3;

            if (user.cod_postal.Equals("Amares"))
            {
                perfil_id = 2;
            }

            db.Utilizador.Add(user);
            //Utilizador_PerfilController.adiciona(user.id, perfil_id);

            db.SaveChanges();


            this.SignInUser(user.username, false);
            // Info.
            HttpCookie cookie = new HttpCookie("user");
            cookie["id"] = user.id.ToString();
            cookie["admin"] = isAdmin(user.id).ToString();
            cookie["residente"] = isResidente(user.id).ToString();
            cookie["membro"] = isMembro(user.id).ToString();
            cookie["id_registo"] = GetIdRegist(user.id).ToString();
            Response.Cookies.Add(cookie);
            return this.RedirectToLocal(returnUrl);

        }

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

        /// <summary>  
        /// POST: /Account/LogOff    
        /// </summary>  
        /// <returns>Return log off action</returns>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            /*
             * Talvez crash quando o loggot seja feito quando foi inicializado externamente, criar cookie com id
             * registo no loggin externo
             */
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign Out.    
                authenticationManager.SignOut();
                //string gasgjdagj = Request.Cookies["user"].Value;
                Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);

            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}