using System;
using System.Threading;
using System.Threading.Tasks;

using Azure.Core;
using Azure.Identity;

namespace MsalTestApp
{
    class Program
    {

        static void Main(string[] args)
        {
            string powerShellClientId = "1950a258-227b-4e31-a9cf-717495945fc2";

            CancellationToken cancellationToken = new CancellationToken();

            DeviceCodeCredential codeCredential = new DeviceCodeCredential(DeviceCodeFunc, "organizations", powerShellClientId);
            var record = codeCredential.Authenticate(cancellationToken);

            var scopes = new[] { "https://management.core.windows.net//.default" };
            TokenRequestContext requestContext = new TokenRequestContext(scopes);
            //Get token to list tenants
            var token = codeCredential.GetToken(requestContext, cancellationToken);
        }

        private static Task DeviceCodeFunc(DeviceCodeInfo info, CancellationToken cancellation)
        {
            Console.WriteLine(info.Message);
            return Task.CompletedTask;
        }

    }
}
