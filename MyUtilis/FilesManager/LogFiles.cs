using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.FilesManager
{
    class LogFiles
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="path"></param>
        /// <param name="AutomaticFiles"></param>
        public static void LogWrite(string logMessage, string path, bool AutomaticFiles)
        {
            String Year = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
            String Month = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            String Day = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
            try
            {
                if (AutomaticFiles)
                {
                    string NewLogDay = Path.Combine(path, Year, Month, "Log_" + Day + ".txt");
                    string PathYear = Path.Combine(path, Year);
                    string PathMont = Path.Combine(path, Year, Month);

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
                    using (StreamWriter w = File.AppendText(path + "\\" + "Log.txt"))
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
        /// <param name="path"></param>
        public static void LogWrite(string logMessage, string path)
        {
            try
            {
                using (StreamWriter w = File.AppendText(path + "\\" + "Log.txt"))
                {
                    Log(logMessage, w);
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
        private static void Log(string logMessage, TextWriter txtWriter)
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

        /// <summary>
        /// /
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLogExeption(Exception ex)
        {
            try
            {
                StreamWriter sw = null;
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine("[" + DateTime.Now.ToString() + "]: " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLogExeption(string message)
        {
            try
            {
                StreamWriter sw = null;
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine("[" + DateTime.Now.ToString() + "]: " + message);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }
    }
}
