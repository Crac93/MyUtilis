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
    /// <summary>
    /// Diferents methods to get information of employee.
    /// </summary>
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
        /// <summary>
        /// Obtener toda la informacion de un Empleado
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static object GetAllInfoUser(string username)
        {
            var _infoUser = new Dictionary<string, string>();

            using (var dsSearcher = new DirectorySearcher())
            {
                var idx = username.IndexOf('\\');
                if (idx > 0)
                    username = username.Substring(idx + 1);
                dsSearcher.Filter = string.Format("(&(objectClass=user)(samaccountname={0}))", username);
                SearchResult result = dsSearcher.FindOne();
                if (result != null)
                {
                    using (var user = new DirectoryEntry(result.Path))
                    {
                        _infoUser.Add("FirstName", (string)user.Properties["givenName"].Value);
                        _infoUser.Add("LastName", (string)user.Properties["sn"].Value);
                        _infoUser.Add("DisplayName", (string)user.Properties["name"].Value);
                        _infoUser.Add("JobTitle", (string)user.Properties["title"].Value);
                        _infoUser.Add("Telephone", (string)user.Properties["telephoneNumber"].Value);
                        _infoUser.Add("Ingresed", Convert.ToString(user.Properties["whenCreated"].Value));
                        _infoUser.Add("Department", (string)user.Properties["department"].Value);
                        _infoUser.Add("Address", (string)user.Properties["mail"].Value);
                        _infoUser.Add("BoosAddress", (string)user.Properties["extensionAttribute11"].Value);
                        _infoUser.Add("EmployId", (string)user.Properties["employeeID"].Value);
                    }
                }
            }
            return _infoUser;
        }
    }
}
