using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BaseArchitect.Utility
{
    public class FileHelper
    {
        #region Directory action

        /// <summary>
        /// Creates the folder. No need check existing of folder before calling this function
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static bool CreateFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                return true;
            }

            try
            {
                Directory.CreateDirectory(folder);
                return true;
            }
            catch (Exception ex)
            {
                //LogManager.Web.LogDebug("Create folder fail: " + folder, ex);
            }
            return false;
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static bool DeleteFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return true;
            }

            try
            {
                Directory.Delete(folder, true);
                return true;
            }
            catch (Exception ex)
            {
                //LogManager.Web.LogDebug("Delete folder fail: " + folder, ex);
            }
            return false;
        }

        #endregion

        #region File action

        /// <summary>
        /// Insert Guid string to start of file name to prevent duplication
        /// </summary>
        /// <param name="orgFile">Original file name</param>
        /// <returns>GuidString-OrgFile</returns>
        public static string NomarlizeFileName(string orgFile)
        {
            if (string.IsNullOrWhiteSpace(orgFile))
            {
                return string.Empty;
            }
            else
            {
                //Ngoại trừ chữ, số, dấu chấm "." hoặc khoảng là khoảng trắng thì thay thế bằng "-"
                Regex rg = new Regex(@"[^a-zA-Z0-9.]+|\s+");

                string fileName = rg.Replace(orgFile, "-");

                //Loại bỏ nhiều dấu "-" cạnh nhau
                Regex rgMinus = new Regex("-+");
                fileName = rgMinus.Replace(fileName, "-");

                return string.Format("{0}-{1}", Guid.NewGuid().ToString().ToLower(), fileName);
            }
        }

        /// <summary>
        /// Remove GuidID in file name to get original uploaded file
        /// </summary>
        /// <param name="normalizedFile"></param>
        /// <returns></returns>
        public static string GetOriginalFileName(string normalizedFile)
        {
            if (!string.IsNullOrWhiteSpace(normalizedFile)
                && normalizedFile.Length > 37)
            {
                return normalizedFile.Substring(37, normalizedFile.Length - 37);
            }
            else
            {
                return normalizedFile;
            }
        }

        public static bool UploadFile(Stream fileStream, string destinationName)
        {
            string directory = Path.GetDirectoryName(destinationName);
            if (Directory.Exists(directory) == false)
            {
                bool createdDirResult = CreateFolder(directory);
                if (createdDirResult == false)
                {
                    return false;
                }
            }

            try
            {
                int readByte;
                int count = 1024;
                byte[] arrByte = new byte[count];
                using (FileStream writer = new FileStream(destinationName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    while ((readByte = fileStream.Read(arrByte, 0, count)) > 0)
                    {
                        writer.Write(arrByte, 0, readByte);
                    }
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                //LogManager.Web.LogError(ex.ToString());
                return false;
            }

            return true;
        }

        public static bool UploadFile(byte[] fileContent, string destinationName)
        {
            string directory = Path.GetDirectoryName(destinationName);
            if (Directory.Exists(directory) == false)
            {
                bool createdDirResult = CreateFolder(directory);
                if (createdDirResult == false)
                {
                    return false;
                }
            }

            try
            {
                using (FileStream writer = new FileStream(destinationName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    writer.Write(fileContent, 0, fileContent.Length);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                //LogManager.Web.LogError(ex.ToString());
                return false;
            }

            return true;
        }

        #endregion

        //public static void CopyFile(string sourcePath, int itemType, int itemID, string destinationFileName)
        //{
        //    string destinationPath = GetFilePath(itemType, itemID, destinationFileName);
        //    CopyFile(sourcePath, destinationPath);
        //}

        public static void CopyFile(string sourcePath, string destinationPath)
        {
            string directory = Path.GetDirectoryName(destinationPath);

            try
            {
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, destinationPath, true);
                }
            }
            catch (Exception ex)
            {
                //LogManager.ServiceManagement.LogError(ex.ToString());
            }
        }

        public static void TryDeleteDirectory(string fullPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fullPath))
                {
                    Directory.Delete(fullPath, true);
                }
            }
            catch (Exception ex)
            {
                //LogManager.ServiceManagement.LogError(ex.ToString());
            }
        }

        public static void TryDelete(string fullFilePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }
            }
            catch (Exception ex)
            {
                //LogManager.ServiceManagement.LogError(ex.ToString());
            }
        }

        public static byte[] GetBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Waiting until can read the file
        /// LEON's solution
        /// </summary>
        /// <param name="filePath"></param>
        public static void WaitingReadyFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            int tryCount = 0;
            while (true)
            {
                try
                {
                    StreamReader sr = new StreamReader(filePath);
                    sr.Close();
                    break;
                }
                catch { }

                tryCount++;
                if (tryCount > 100)
                    throw new Exception("Access file failed: " + filePath);

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Build with Folder input
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string BuildFilePath(string folder, string fileName)
        {
            string destination = Path.Combine(folder, fileName);
            return destination;
        }
    }
}
