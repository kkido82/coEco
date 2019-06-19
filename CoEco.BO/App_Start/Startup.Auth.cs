using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using CoEco.BO.Auth;
using Owin;

namespace CoEco.BO.App_Start
{
	public partial class Startup
	{
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(Auth.ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, Auth.ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                    OnApplyRedirect = ctx =>
                    {
                        if (ctx.Response.StatusCode != 401)
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                },

            });
            app.Use(async (context, next) =>
            {
                if (context.Authentication.User == null)
                    await next();
                var userManager = context.GetUserManager<ApplicationUserManager>();
                var currentUser = await userManager.FindByNameAsync(context.Authentication.User.Identity.Name);
                if (currentUser != null && currentUser.ShouldBeSignedOut)
                {
                    context.Authentication.SignOut();
                    currentUser.ShouldBeSignedOut = false;
                    await context.Get<Auth.ApplicationDbContext>().SaveChangesAsync();


                }

                await next();
            });
            app.Use(async (context, next) =>
            {

                var userManager = context.GetUserManager<ApplicationUserManager>();
                var currentUser = await userManager.FindByNameAsync(context.Authentication.User.Identity.Name);
                if (currentUser != null && userManager.IsExpired(currentUser))
                {
                    context.Authentication.SignOut();




                }

                await next();
            });

        }
    }
}