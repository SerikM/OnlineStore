using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WebUI2
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                  AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                  LoginPath = new PathString("/Account/Login")
                });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);



            app.UseGoogleAuthentication();

            app.UseFacebookAuthentication("552406271543513", "a7c97ba1654850584eb063cca4ad26cd");
        }
    }
}