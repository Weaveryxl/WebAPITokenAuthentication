using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using WebAPITokenAuthentication.DAL;

namespace WebAPITokenAuthentication.Infrastructure
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //method is responsible for validating the “Client”, in our case we have only one client so we’ll always return that its validated successfully
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        // The second method “GrantResourceOwnerCredentials” is responsible to validate the username and password sent to 
        // the authorization server’s token endpoint, so we’ll use the “AuthRepository” class we created earlier and
        // call the method “FindUser” to check if the username and password are valid

       // If the credentials are valid we’ll create “ClaimsIdentity” class and pass the authentication type to it,
       // in our case “bearer token”, then we’ll add two claims(“sub”,”role”) and those will be included in the   signed token.
       // You can add different claims here but the token size will increase for sure.
       // Now generating the token happens behind the scenes when we call “context.Validated(identity)”.


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

            using (var repo = new AuthRepository())
            {
                IdentityUser user = await repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}