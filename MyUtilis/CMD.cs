using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis
{
    /// <summary>
    /// 
    /// </summary>
    public class CMD
    { /// <summary>
      /// Run cmd 
      /// </summary>
      /// <param name="FileName">The name of console to run.</param>
      /// <param name="Arguments">Arguemtns to send</param>
      /// <returns></returns>
      /// 
        public static int Run(string FileName, string Arguments)
        {
            try
            {
                Process Prc = new Process();
                Prc.StartInfo.FileName = FileName;
                Prc.StartInfo.Arguments = Arguments;
                Prc.Start();
                Prc.WaitForExit();
                return Prc.ExitCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString().Trim(' '));
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public bool VerifyProcessRun(string processName)
        {
            bool response = false;

            Process[] procesos = Process.GetProcesses();
            foreach (Process proceso in procesos)
            {
                if (proceso.ProcessName == processName)
                    response = true;
            }
            return response;
        }
    }
}
