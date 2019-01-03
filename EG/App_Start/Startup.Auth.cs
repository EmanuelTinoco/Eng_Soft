using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using EG.Models;
using System.Configuration;

namespace EG
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864   
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user   
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider   
            // Configure the sign in cookie   
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5.0)
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Uncomment the following lines to enable logging in with third party login providers   
            //app.UseMicrosoftAccountAuthentication(   
            // clientId: "",   
            // clientSecret: "");   
            //app.UseTwitterAuthentication(   
            // consumerKey: "",   
            // consumerSecret: "");   
            //app.UseFacebookAuthentication(   
            // appId: "",   
            // appSecret: "");   

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "153357369749-j9s69fl4aft5jklgj3803c4fvv9scjie.apps.googleusercontent.com",
            //    ClientSecret = "3QHvJoKcoSCYtfS6rEmclSAr",
            //});
            var googleAuthenticationOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GglI"],
                ClientSecret = ConfigurationManager.AppSettings["GglS"],
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                      {
                          context.Identity.AddClaim(new System.Security.Claims.Claim("GoogleAccessToken", context.AccessToken));

                          foreach (var claim in context.User)
                          {
                              var claimType = string.Format("urn:Google:{0}", claim.Key);
                              string claymValue = claim.Value.ToString();
                              if (!context.Identity.HasClaim(claimType, claymValue))
                              {
                                  context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claymValue, "XmlSchemaString", "Google"));
                              }
                          }
                      }
                }
            };
            googleAuthenticationOptions.Scope.Add("https://www.googleapis.com/auth/plus.login email");
            app.UseGoogleAuthentication(googleAuthenticationOptions);

        }
    }
}