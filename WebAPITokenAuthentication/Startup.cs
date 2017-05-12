using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebAPITokenAuthentication.Infrastructure;

[assembly: OwinStartup(typeof(WebAPITokenAuthentication.Startup))]

namespace WebAPITokenAuthentication
{
    /*
     *  This class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up.
     *  The “Configuration” method accepts parameter of type “IAppBuilder” this parameter will be supplied by the host at run-time. 
     *  This “app” parameter is an interface which will be used to compose the application for our Owin server
     */

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //  The “HttpConfiguration” object is used to configure API routes, so we’ll pass this object to method
            // “Register” in “WebApiConfig” class
            //  Web API configuration setttings are defined in the HttpConfiguration class
            ConfigureOAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            // The path for generating tokens will be as :"http://localhost:port/token".
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}