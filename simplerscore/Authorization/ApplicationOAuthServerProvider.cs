namespace SimplerScore.Authorization
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security.OAuth;

    internal static class AuthorizationExtensions
    {
        private static void SetErrorAndReject (this OAuthGrantResourceOwnerCredentialsContext context, string error, string description)
        {
            context.SetError(error, description);
            context.Rejected();
        }

        internal static void SetInvalidGrantErrorAndReject (this OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.SetErrorAndReject("invalid_grant", "The username or password is incorrect");
        }
    }

    /// <summary>
    /// Nominal implementation based on article
    /// http://www.codeproject.com/Articles/876867/ASP-NET-Web-Api-Understanding-OWIN-Katana-Authenti
    /// </summary>
    public class ApplicationOAuthServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Validates the client authentication.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // to get token sent a POST to url:
            // http://localhost:8088/Token
            //
            // with x-www-form-urlencoded parameters of 
            // grant_type : password
            // username : whateverusername
            // password : password (to meet test below)
            //
            // Subsequent calls to service decorated with Authorize 
            // attribute should have Authorization header
            // with Bearer prefix and token value.
            // Authorization: Bearer {token}

            // This call is required...
            // but we're not using client authentication, so validate and move on...
            await Task.FromResult(context.Validated());
        }

        /// <summary>
        /// Grants the resource owner credentials.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials (OAuthGrantResourceOwnerCredentialsContext context)
        {
            //TODO: do password validation here...
            throw new NotImplementedException();
#if false
				if (context.Password != "password")
            {
                context.SetInvalidGrantErrorAndReject();
                return;
            }

            // Create or retrieve a ClaimsIdentity to represent the 
            // Authenticated user:
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            // Identity info will ultimately be encoded into an Access Token
            // as a result of this call:
            await Task.FromResult(context.Validated(identity));  
#endif // false
        }
    }
}
