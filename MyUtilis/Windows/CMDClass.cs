using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Windows
{
    class CMDClass
    {

        /// <summary>
        /// Execumte a comanda in CMD
        /// </summary>
        /// <param name="Command"></param>
        public static void ExecuteCommandSync(string Command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + Command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string Result = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
              
            }
        }
    }
}
