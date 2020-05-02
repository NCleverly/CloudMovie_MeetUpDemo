using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CloudMovie.Web.Services
{
    public interface IKeyVaultService
    {
        Task<X509Certificate2> GetCert(string thumbprint, string certId);
        Task<string> GetStringSecret(string settingName);
    }
}
