using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;



namespace CloudMovie.Web.Services
{
    public class KeyVaultService : IKeyVaultService, IDisposable
    {
        bool disposed = false;
        private static KeyVaultClient _kv;
        private string _accessToken;

        public KeyVaultService(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // using managed identities
            GetAccessTokenAsync().Wait();
        }
        public async Task<X509Certificate2> GetCert(string keyvalutPath, string certName)
        {
            var cert = await _kv.GetCertificateAsync(keyvalutPath, certName).ConfigureAwait(false);
            var secretRetrieved = await _kv.GetSecretAsync(cert.SecretIdentifier.ToString()).ConfigureAwait(false);
            var pfxBytes = Convert.FromBase64String(secretRetrieved.Value);
            var certificate = new X509Certificate2(pfxBytes);

            return certificate;
        }


        private async Task GetAccessTokenAsync()
        {

            // Instantiate a new KeyVaultClient object, with an access token to Key Vault
            var azureServiceTokenProvider1 = new AzureServiceTokenProvider();
             _kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider1.KeyVaultTokenCallback));

            // Optional: Request an access token to other Azure services
            var azureServiceTokenProvider2 = new AzureServiceTokenProvider();
            _accessToken = await azureServiceTokenProvider2.GetAccessTokenAsync("https://management.azure.com/").ConfigureAwait(false);

        }
        public async Task<string> GetStringSecret(string settingName)
        {
            try
            {
                var item = await _kv.GetSecretAsync(settingName);
                return item.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _kv.Dispose();
                // Free any other managed obje
            }

            disposed = true;
        }
    }
}
