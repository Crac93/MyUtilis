using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis
{
    public class CMD
    { /// <summary>
      /// Run cmd 
      /// </summary>
      /// <param name="FileName">The name of console to run.</param>
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
            catch
            {
                return -1;
            }
        }

        public int RunWithArguments(string[] Arguments)
        {
            try
            {
                Process Prcss = new Process();
                Prcss.StartInfo.FileName = Arguments[1];
                Prcss.StartInfo.Arguments = string.Join(" ", Arguments, 2, Arguments.Length - 2);
                Prcss.Start();
                Prcss.WaitForExit();
                return Prcss.ExitCode;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
