using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary
{/// <summary>
/// 
/// </summary>
    public class ConsoleClass
    {
        /// <summary>
        /// Run a console,if fails,returns -2
        /// </summary>
        /// <param name="fileName">Ejempo: ConsoleSBS.exe</param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static int Run(string fileName, string arguments)
        {
            try
            {
                Process Prc = new Process();
                Prc.StartInfo.FileName = fileName;
                Prc.StartInfo.Arguments = arguments;
                Prc.Start();
                Prc.WaitForExit();
                return Prc.ExitCode;
            }
            catch (Exception ex)
            {
                return -2;
                throw new ArgumentException(ex.ToString());
            }
        }


        /// <summary>
        /// Run a console,if fails,returns -2
        /// </summary>
        /// <param name="fileName">Ejempo: ConsoleSBS.exe</param>
        /// <param name="arguments"></param>
        /// <param name="maximize"> Mostrar toda la pantalla </param>
        /// <returns></returns>
        public static int Run(string fileName, string arguments, bool maximize)
        {

            try
            {
                Process Prc = new Process();
                Prc.StartInfo.FileName = fileName;
                Prc.StartInfo.Arguments = arguments;
                Prc.Start();
                TimeSpan.FromMilliseconds(2000);
                if (maximize)
                {
                    ShowWindowAsync(Prc.MainWindowHandle, 1);
                }
                Prc.WaitForExit();
                return Prc.ExitCode;
            }
            catch (Exception ex)
            {
                return -2;
                throw new ArgumentException(ex.ToString());
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

   
        /// <summary>
        /// Verificar si el proceso se encuentra en ejecucion.
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool VerifyProcessRun(string processName)
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



    } //CALSS



}//NAMESPACE
