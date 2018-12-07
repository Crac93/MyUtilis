using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Windows
{
    /// <summary>
    /// Crear y adiminstrar log files, objeto no estaico
    /// </summary>
    public class TraceLog
    {
        public string PathLog { get; set; }
        public string PathName { get; set; }
        public bool AutomaticFiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="automaticFiles">Crea</param>
        /// <param name="pathName"></param>
        public TraceLog(string path, bool automaticFiles, string pathName = "Log.txt")
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                PathLog = path;
                AutomaticFiles = automaticFiles;
                if (pathName != "Log.txt")
                    PathName = pathName;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// 
        public void WriteLog(string logMessage)
        {
            String Year = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
            String Month = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            String Day = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
            try
            {
                if (AutomaticFiles)
                {
                    string NewLogDay = Path.Combine(PathLog, Year, Month, "Log_" + Day + ".txt");
                    string PathYear = Path.Combine(PathLog, Year);
                    string PathMont = Path.Combine(PathLog, Year, Month);

                    if (!Directory.Exists(PathYear))
                        Directory.CreateDirectory(PathYear);

                    if (!Directory.Exists(PathMont))
                        Directory.CreateDirectory(PathMont);

                    using (StreamWriter w = File.AppendText(NewLogDay))
                    {
                        Log(logMessage, w);
                    }
                }
                else
                {
                    using (StreamWriter w = File.AppendText(PathLog + "\\" + PathName))
                    {
                        Log(logMessage, w);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="txtWriter"></param>
        private void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0}: {1} ", "[" + DateTime.Now + "]", logMessage);
                txtWriter.WriteLine("-------------------------------------------------------------------------------");
            }
            catch
            {

            }
        }

    }
}
