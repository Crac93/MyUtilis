using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyUtilis.Windows
{
    public class Login
    {
        /// <summary>
        /// Using the Principal context of server domain.(Very easy)
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="ServerDns">Ejemplo:mxrexsv01</param>
        /// <returns></returns>
        public static bool ValidateUserDNS(string UserName, string Password, string ServerDns)
        {
            PrincipalContext Pc = new PrincipalContext(ContextType.Domain, ServerDns);
            return Pc.ValidateCredentials(UserName, Password);
        }

        /// <summary>
        /// Se usan los directorySearcher , puede usarse el LDAP del dominio.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="Password"></param>
        /// <param name="path">Ejemplo;LDAP://Empresa</param>
        /// <returns></returns>
        public static string AuthenticationEntry(string userID, string Password, string path)
        {
            DirectoryEntry entry = new DirectoryEntry(path, userID, Password);
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(objectclass=user)";

            try
            {
                SearchResult result = searcher.FindOne();
                return "Welcome";
            }
            catch (Exception ex) { return ex.Message.ToString().Trim(' '); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Bitmap GetUserPhoto(string userName)
        {
            Bitmap image = null;
            using (var dsSearcher = new DirectorySearcher())
            {
                var idx = userName.IndexOf('\\');
                if (idx > 0)
                    userName = userName.Substring(idx + 1);
                dsSearcher.Filter = string.Format("(&(objectClass=user)(samaccountname={0}))", userName);
                SearchResult result = dsSearcher.FindOne();
                if (result != null)
                {
                    using (var user = new DirectoryEntry(result.Path))
                    {
                        byte[] userImage = user.Properties["thumbnailPhoto"].Value as byte[];
                        if (userImage != null)
                        {
                            using (var ms = new MemoryStream(userImage))
                            {
                                image = new Bitmap(ms);
                            }
                        }
                    }
                }
            }
            return image;
        }
        /// <summary>
        /// Extraer el nombre completo del usuario en DNS 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetUserFullName(string userName)
        {
            string firstName = "";
            string lastName = "";
            using (var dsSearcher = new DirectorySearcher())
            {
                var idx = userName.IndexOf('\\');
                if (idx > 0)
                    userName = userName.Substring(idx + 1);
                dsSearcher.Filter = string.Format("(&(objectClass=user)(samaccountname={0}))", userName);
                SearchResult result = dsSearcher.FindOne();
                if (result != null)
                {
                    using (var user = new DirectoryEntry(result.Path))
                    {
                        firstName = (string)user.Properties["givenName"].Value;
                        lastName = (string)user.Properties["sn"].Value;
                    }
                }
            }
            return firstName + " " + lastName;
        }
    }
}
