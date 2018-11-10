using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace FileManagerLib
{

    /// <summary>
    /// 
    /// </summary>
    public class FileManagerClass
    {

        /// <summary>
        /// Delete all folder, with the option of only content, or not.
        /// </summary>
        /// <param name="targetFolder">Path of directory</param>
        /// <param name="options">Type of deleting folder</param>
        /// 
        public static void DeleteFolder(string targetFolder, FolderOptions options = FolderOptions.IncludeMainFolder)
        {
            try
            {

                var files = Directory.EnumerateFiles(targetFolder);
                var dirs = Directory.EnumerateDirectories(targetFolder);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteFolder(dir, FolderOptions.IncludeMainFolder);
                }
                if (options == FolderOptions.IncludeMainFolder)
                    Directory.Delete(targetFolder, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get the size of a directory.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static long GetFolderSize(string folderPath)
        {
            long Result = 0;
            try
            {
                Result = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Result;
        }

        /// <summary>
        /// Gat a list of files in the path specificated.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileExt">Select a file extension.</param>
        /// <param name="fileOption">Select options of returns file names.</param>
        /// <returns></returns>
        public static List<string> GetFilesInFolder(string path, FileExt fileExt = FileExt.AllExtensions, FileExtOption fileOption = FileExtOption.FullPath)
        {
            List<string> files = new List<string>();
            try
            {
                string extension = "";
                switch (fileExt)
                {
                    case FileExt.TXT:
                        extension = ".txt";
                        break;
                    case FileExt.INI:
                        extension = ".ini";
                        break;
                    case FileExt.PDF:
                        extension = ".pdf";
                        break;
                    case FileExt.LOG:
                        extension = ".log";
                        break;
                    case FileExt.PNG:
                        extension = ".png";
                        break;
                    case FileExt.AllExtensions:
                        extension = ".";
                        break;
                    default:
                        break;
                }

                string[] filePaths = Directory.GetFiles(path, "*" + extension + "*", SearchOption.AllDirectories);

                switch (fileOption)
                {
                    case FileExtOption.FullPath:
                        foreach (var file in filePaths)
                        {
                            FileInfo f = new FileInfo(file);
                            files.Add(f.FullName);
                        }
                        break;
                    case FileExtOption.OnlyName:

                        foreach (var file in filePaths)
                        {
                            FileInfo f = new FileInfo(file);
                            files.Add(f.Name);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return files;

        }

        /// <summary>
        /// Open a File explorer to search, and returns a folder path.
        /// </summary>
        /// <returns></returns>
        public static string BrowseFolderPath()
        {
            string FolderPath = null;
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)

            { FolderPath = dialog.SelectedPath; }
            return (FolderPath);

        }

        /// <summary>
        /// Open a File explorer to search, and returns a file full path.
        /// </summary>
        /// <returns></returns>
        public static string BrowseFilePath()
        {
            string FilePath = null;
            var dialog = new System.Windows.Forms.OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                FilePath = dialog.FileName;

            return (FilePath);
        }

        /// <summary>
        /// Retunr a "Created" if was success.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool CreateFolder(string folderPath)
        {
            bool Result = false;
            try
            {
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    Result = true;
                }
                else
                    throw new Exception("Folder already exists");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Result;
        }

        /// <summary>
        /// Verifiy if exists the file, then create.
        /// </summary>
        /// <param name="folderPath">Example: C:\Folder\hello.txt </param>
        /// <returns></returns>
        public static bool CreateFile(string folderPath)
        {
            bool Res = false;
            if (!File.Exists(folderPath))
            {
                File.Create(folderPath).Dispose();
                Res = true;
            }
            return Res;
        }

        /// <summary>
        /// Delete a specific file, return "Done" if is success.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string DeleteFile(string filePath)
        {

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return "Done";
                }
                catch (Exception ex) { return ex.ToString(); }
            }
            else
            {
                return "Error: File does not exists";
            }

        }



        /// <summary>
        /// Return "True" if was success.
        /// </summary>
        /// <param name="SourceFilePath"></param>
        /// <param name="DestinationFilePath"></param>
        /// <returns></returns>
        public static bool CopyFile(string SourceFilePath, string DestinationFilePath)
        {
            bool Result = false;

            if (System.IO.File.Exists(SourceFilePath))
            {
                if (!System.IO.File.Exists(DestinationFilePath))
                {
                    System.IO.File.Copy(SourceFilePath, DestinationFilePath, true);
                    Result = true;
                }
                //  throw new Exception("Error: Destination file path already exists");
            }

            return Result;
        }

        /// <summary>
        /// Verify if exitst the file, if yes, move file to destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static bool MoveFile(string source, string destination)
        {
            bool result = false;
            try
            {
                if (File.Exists(source))
                {
                    File.Move(source, destination);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }
        /// <summary>
        ///  Verify if exitst the folder, if yes, move the folder to destination.
        /// </summary>
        /// <param name="source">Example: C:\Source</param>
        /// <param name="destination">Example: C:\Destination</param>
        /// <returns></returns>
        public static bool MoveFolder(string source, string destination)
        {
            bool result = false;

            try
            {
                if (Directory.Exists(source))
                {
                    if (!Directory.Exists(destination))
                    {
                        Directory.Move(source, destination);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232800)
                {
                    if (CopyFolder(source, destination))
                    {
                        DeleteFolder(source);
                        result = true;
                    }
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Copy all folder content
        /// </summary>
        /// <param name="source">Example: C:\Source</param>
        /// <param name="destination">Example: C:\Destination</param>
        public static bool CopyFolder(string source, string destination)
        {
            //var zipFile = source + ".zip";

            //System.IO.Compression.ZipFile.CreateFromDirectory(source, zipFile);
            //CreateFolder(destination);
            //System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, destination);
            //File.Delete(zipFile);
            try
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "xcopy.exe");
                proc.StartInfo.Arguments = @"" + source + " " + destination + " /E /I /Y";
                proc.Start();
                proc.WaitForExit();
                return true;
            }

            catch
            {
                return false;
            }
        }
    }

}
