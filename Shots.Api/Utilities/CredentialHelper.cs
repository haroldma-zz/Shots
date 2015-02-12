using System.Linq;
using Windows.Security.Credentials;

namespace Shots.Api.Utilities
{
    /// <summary>
    /// Helper class for accessing and deleting credentials securely.
    /// </summary>
    public class CredentialHelper
    {
        public PasswordCredential GetCredentials(string resource)
        {
            var vault = new PasswordVault();
            
            var credential = vault.RetrieveAll().FirstOrDefault(p => p.Resource == resource);
            return credential;
        }

        public void SaveCredentials(string resource, string username, string password)
        {
            // Create and store the user credentials.
            var credential = new PasswordCredential(resource,
                username, password);
            new PasswordVault().Add(credential);
        }

        public void DeleteCredentials(string resource)
        {
            var credential = GetCredentials(resource);
            if (credential != null)
                new PasswordVault().Remove(credential);
        }
    }
}
