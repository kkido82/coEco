using CoEco.Front.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(CoEco.Front.Startup))]

namespace CoEco.Front
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var expireInMinutes = ConfigurationHelper.Get<int>("auth:ExpireInMinutes");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                ExpireTimeSpan = TimeSpan.FromMinutes(expireInMinutes),
                SlidingExpiration = true
            });


        }
    }
}