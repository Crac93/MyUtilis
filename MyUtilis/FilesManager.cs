using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyUtilis
{
    public class FilesManager
    {

        public static void DeleteFolder(string FolderPath)
        {
            Console.WriteLine(Utilis.Debug() + "DeleteFolder()");
            Console.WriteLine(Utilis.Debug() + "FolderPath: " + FolderPath);

            if (System.IO.Directory.Exists(FolderPath))
            {
                try
                {
                    System.IO.Directory.Delete(FolderPath);

                    Console.WriteLine(Utilis.Debug() + "Result: Folder succesfully deleted");
                }
                catch (Exception ex) { Console.WriteLine(Utilis.Debug() + "Error: " + ex.ToString()); }
            }
            else
            {
                Console.WriteLine(Utilis.Debug() + "Error: FolderPath does not exists");
            }

        }
        /// <summary>
        /// Get all the files in specific folder
        /// </summary>
        /// <param name="FolderPath">Target folder</param>
        /// <returns></returns>
        public static string[] GetAllFilesInFolder(string FolderPath)
        {
            Console.WriteLine(Utilis.Debug() + "GetAllFilesInFolder()");
            Console.WriteLine(Utilis.Debug() + "FolderPath: " + FolderPath);

            string[] Result = null;

            if (System.IO.Directory.Exists(FolderPath))
                Result = Directory.GetFiles(FolderPath);
            else
                Console.WriteLine(Utilis.Debug() + "Error: Folder path does not exists");

            Console.WriteLine(Utilis.Debug() + "Result: ");

            foreach (string File in Result)
                Console.WriteLine(Utilis.Debug() + File);

            return Result;
        }

        public static long FolderSize(string FolderPath)
        {
            long Result = 0;
            try
            {
                Result = Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
            }
            catch { }

            return Result;
        }
        /// <summary>
        /// Return the string path of folder
        /// </summary>
        /// <returns></returns>
        public static string GetFolderPath()
        {
            string FolderPath = null;

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)

            { FolderPath = dialog.SelectedPath; }
            return (FolderPath);

        }
        /// <summary>
        /// Open file dialog and return the string path.
        /// </summary>
        /// <returns></returns>
        public static string GetFilePath()
        {
            string FilePath = null;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = dialog.FileName;
                Console.WriteLine(Utilis.Debug() + "GetFilePath ( ) ---> OK");
            }
            return (FilePath);
        }

        public static string GetFileName()
        {
            string FilePath = null;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            { FilePath = dialog.SafeFileName; }
            return (FilePath);
        }

        public static int ReWriteTextFileOmitingRepatedLines(string OriginalFile, string FixedFile)
        {
            int Counter = 0;

            if (File.Exists(OriginalFile))
                File.WriteAllLines(FixedFile, File.ReadAllLines(OriginalFile).Distinct().ToArray());

            else { Counter = -1; }

            return (Counter);
        }

        public static void CreateFolder(string folder_path)
        {
            if (!System.IO.Directory.Exists(folder_path))
                System.IO.Directory.CreateDirectory(folder_path);
        }

        public static int CountFolderFiles(string FolderPath)
        {
            int Result = -1;
            try
            {
                System.IO.DirectoryInfo Directory = new System.IO.DirectoryInfo(FolderPath);
                Result = Directory.GetFiles().Length;
                Console.WriteLine(Utilis.Debug() + "CountFolderFiles ( ) ---> OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Utilis.Debug() + "CountFolderFiles ( ) ---> NOK" +
                    System.Environment.NewLine + ex.ToString());
            }

            return Result;
        }

        public static bool DeleteAllFilesInFolder(string FolderPath)
        {
            bool Result = false;

            try
            {
                if (System.IO.Directory.Exists(FolderPath))
                    Array.ForEach(Directory.GetFiles(FolderPath), delegate (string path) { File.Delete(path); });

                Console.WriteLine(Utilis.Debug() + "c_IO.DeleteAllFilesInFolder( )");
                Result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Utilis.Debug() + "c_IO.DeleteAllFilesInFolder ( ) ---> NOK" +
                    System.Environment.NewLine + ex.ToString());
            }

            return Result;
        }

        public static void DeleteFile(string FilePath)
        {
            Console.WriteLine(Utilis.Debug() + "DeleteFile()");
            Console.WriteLine(Utilis.Debug() + "FilePath: " + FilePath);

            if (System.IO.File.Exists(FilePath))
            {
                try
                {
                    System.IO.File.Delete(FilePath);

                    Console.WriteLine(Utilis.Debug() + "Result: File succesfully deleted");
                }
                catch (Exception ex) { Console.WriteLine(Utilis.Debug() + "Error: " + ex.ToString()); }
            }
            else
            {
                Console.WriteLine(Utilis.Debug() + "Error: File does not exists");
            }

        }

        public static void CreateTextFile(string file_path, string text)
        {
            StreamWriter file = File.CreateText(file_path);
            file.Write(text);
            file.Close();

        }

        public static bool WriteLine(string FilePath, string TextLine)
        {
            Console.WriteLine(Utilis.Debug() + "WriteLine()");
            Console.WriteLine(Utilis.Debug() + "FilePath: " + FilePath);
            Console.WriteLine(Utilis.Debug() + "TextLine: " + TextLine);

            bool Result = false;

            try
            {
                StreamWriter Writter = File.AppendText(FilePath);
                Writter.WriteLine(TextLine);
                Writter.Close();
                Result = true;
                Console.WriteLine(Utilis.Debug() + "Result: " + Result);

            }
            catch (Exception ex) { Console.WriteLine(Utilis.Debug() + "Error: " + ex.ToString()); }


            return Result;
        }

        public static string ReadTextFile(string FilePath)
        {
            Console.WriteLine(Utilis.Debug() + "ReadTextFile()");
            Console.WriteLine(Utilis.Debug() + "FilePath: " + FilePath);
            string Result = null;

            if (File.Exists(FilePath))
            {
                Result = File.ReadAllText(FilePath);
                Console.WriteLine(Utilis.Debug() + "Result: " + Result);
            }
            else
            {
                Console.WriteLine(Utilis.Debug() + "Error: " + FilePath + "does not exists . . . ");
                
            }

            return (Result);
        }

        public static bool ReplaceStringIntoTextFile(string FilePath, string Old, string New)

        {
            bool Result = false;

            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, File.ReadAllText(FilePath).Replace(Old, New));
                Result = true;
                Console.WriteLine(Utilis.Debug() + "ReplaceStringIntoTextFile ( ) ---> OK");

            }

            return Result;
        }

        public static bool ReplaceStringArrayIntoTextFile(string FilePath, string[] Olds, string[] News)
        {
            Console.WriteLine(Utilis.Debug() + "c_IO.ReplaceStringArrayIntoTextFile()");
            bool Result = false;

            if (File.Exists(FilePath))
            {
                if (Olds.Length == News.Length)
                {
                    try
                    {

                        for (int i = 0; i < Olds.Length; i++)
                        {
                            File.WriteAllText(FilePath, File.ReadAllText(FilePath).Replace(Olds[i], News[i]));
                            Console.WriteLine(Utilis.Debug() + "String[" + i + "] " + Olds[i] + " <---> " + News[i]);
                        }
                        Result = true;
                    }
                    catch (Exception ex) { Console.WriteLine(Utilis.Debug() + "Error: " + ex.ToString()); }


                }
                else
                {
                    Console.WriteLine(Utilis.Debug() + "Error: Mismatch in Array Sizes");
                }
            }
            else
            {
                Console.WriteLine(Utilis.Debug() + "Error: File does not exists");

            }

            return Result;
        }

        public static string extract_substring_from_string(string my_string, int start_index, int end_index)
        {
            string sub_string = my_string.Substring(start_index, end_index);
            return (sub_string);
        }

        public static bool CopyFile(string SourceFilePath, string DestinationFilePath)
        {

            Console.WriteLine(Utilis.Debug() + "CopyFile()");
            Console.WriteLine(Utilis.Debug() + "SourceFilePath: " + SourceFilePath);
            Console.WriteLine(Utilis.Debug() + "DestinationFilePath: " + DestinationFilePath);

            Boolean Result = false;

            if (System.IO.File.Exists(SourceFilePath))
            {
                if (System.IO.File.Exists(DestinationFilePath))
                {
                    Console.WriteLine(Utilis.Debug() + "Error: Destination file path already exists");
                }
                else
                {

                    System.IO.File.Copy(SourceFilePath, DestinationFilePath, true);
                    Result = true;
                    Console.WriteLine(Utilis.Debug() + "Result: File succesfully copied");
                }
            }
            else
            {
                Console.WriteLine(Utilis.Debug() + "Error: source file path does not exists");
            }

            return Result;
        }

        public static string ReplaceSubstring(string OriginalString, string OldChar, string NewChar)
        {
            string NewString = null;
            NewString = OriginalString.Replace(OldChar, NewChar);

            return (NewString);

        }

    }

}
