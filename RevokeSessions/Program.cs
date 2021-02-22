using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

using Azure.Core;
using Azure.Identity;

namespace RevokeSessions
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";
            string tenantId = "54826b22-38d6-4fb2-bad9-b7b93a3e9c5a";
            string[] scopes = new[] { "https://management.azure.com/.default" };

            var interOptions = new InteractiveBrowserCredentialOptions()
            {
                TenantId = tenantId,
                ClientId = clientId,
                DisableAutomaticAuthentication = true,
            };

            var credential = new InteractiveBrowserCredential(interOptions);
            var context = new TokenRequestContext(scopes);
            credential.Authenticate();
            var token = credential.GetToken(context).Token;

            var graphContext = new TokenRequestContext(new[] { "User.ReadWrite" });
            var graphToken = credential.GetToken(graphContext).Token;
            var httpClient = new HttpClient();
            //var message = new HttpRequestMessage(HttpMethod.Post, "https://graph.microsoft.com/v1.0/me/revokeSignInSessions");
            var message = new HttpRequestMessage(HttpMethod.Post, "https://graph.microsoft.com/v1.0/users/erichwang@azuresdkteam.onmicrosoft.com/revokeSignInSessions");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);
            var result = httpClient.SendAsync(message).ConfigureAwait(false).GetAwaiter().GetResult();
            Debug.Assert(result.StatusCode == System.Net.HttpStatusCode.OK);

        }
    }
}
