using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Hartalega.BarcodeScannerIntegrator
{
    public class OAuthHelper
    {
        public OAuthHelper()
        {
        }
        /// <summary>
        /// The header to use for OAuth.
        /// </summary>
        public const string OAuthHeader = "Authorization";

        /// <summary>
        /// Retrieves an authentication header from the service.
        /// </summary>
        /// <returns>The authentication header for the Web API call.</returns>
        public static string GetAuthenticationHeader(D365ClientConfiguration d365ClientConfiguration = null)
        {
            if (d365ClientConfiguration == null)
                d365ClientConfiguration = D365ClientConfiguration.Default;
            bool useWebAppAuthentication = d365ClientConfiguration.UseWebAppAuthentication;
            string aadTenant = d365ClientConfiguration.ActiveDirectoryTenant;
            string aadClientAppId = d365ClientConfiguration.ActiveDirectoryClientAppId;
            string aadResource = d365ClientConfiguration.ActiveDirectoryResource;
            // OAuth through username and password.
            string username = d365ClientConfiguration.UserName;
            string password = d365ClientConfiguration.Password;
            string aadClientAppSecret = d365ClientConfiguration.ActiveDirectoryClientAppSecret;

            AuthenticationContext authenticationContext = new AuthenticationContext(aadTenant, d365ClientConfiguration.ValidateAuthority);
            AuthenticationResult authenticationResult;

            if (useWebAppAuthentication)
            {
                var creadential = new ClientCredential(aadClientAppId, aadClientAppSecret);
                authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, creadential).Result;
            }
            else
            {
                // Get token object
                // var userCredential = new UserPasswordCredential(username, password);// for .net 4.5 above
                var userCredential = new UserCredential(username);// for .net 4

                authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, aadClientAppId, userCredential).Result;
            }

            // Create and get JWT token
            return authenticationResult.CreateAuthorizationHeader();
        }
    }
}
