// -----------------------------------------------------------------------
// <copyright file="FileManager.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Common
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Cannot be inherited and cannot have instance
    /// This class can be used for file management
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Check whether the file is exists in given path
        /// </summary>
        /// <param name="folder">Fully qualified folder path</param>
        /// <param name="fileName">File name</param>
        /// <returns>Boolean value that returns whether the file is exist or not</returns>
        #region Public Methods
        public static bool IsFileExists(string folder, string fileName)
        {
            bool isExists = false;
            DirectoryInfo directory = new DirectoryInfo(folder);
            if (directory.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Where(f => string.Compare(f.Name, fileName, true) == 0).ToList().Count > 0)
            {
                isExists = true;
            }

            directory = null;
            return isExists;
        }
        
        /// <summary>
        /// Check whether the folder is exists or not
        /// </summary>
        /// <param name="folder">Fully qualified path</param>
        /// <returns>Boolean value indicates whether the folder is exists or not</returns>
       
        public static bool IsFolderExists(string folder)
        {
            bool isExists = false;
            DirectoryInfo directory = new DirectoryInfo(folder);
            isExists = directory.Exists;
            directory = null;

            return isExists;
        }
       
        /// <summary>
        /// Copy a file from one folder and move the same to another folder
        /// </summary>
        /// <param name="sourceFolder">Fully qualified source folder path</param>
        /// <param name="destinationFolder">Fully qualified destination folder path</param>
        public static void CopyFiles(string sourceFolder, string destinationFolder)
        {
            if (System.IO.Directory.Exists(sourceFolder))
            {
                string[] files = System.IO.Directory.GetFiles(sourceFolder).ToArray();
                string fileName = string.Empty;

                foreach (string file in files)
                {
                    fileName = System.IO.Path.GetFileName(file);
                    File.Move(file, System.IO.Path.Combine(destinationFolder, fileName));
                }
            }
        }
       
        /// <summary>
        /// Save the file into new location
        /// </summary>
        /// <param name="sourceFile">Source file with fully qualified path with file name</param>
        /// <param name="destinationFile">Destination file with fully qualified path with file name</param>\
        /// <param name="deleteFile">Delete the file from source folder</param>
        public static void SaveAs(string sourceFile, string destinationFile, bool deleteFile)
        {
            if (File.Exists(sourceFile))
            {
                FileInfo fileInfo = new FileInfo(sourceFile);
                fileInfo.CopyTo(destinationFile, true);
                fileInfo = null;
            }

            if (deleteFile)
            {
                DeleteFile(sourceFile);
            }
        }
        
        /// <summary>
        /// Get all file names from given folder
        /// </summary>
        /// <param name="folder">Fully qualified folder path</param>
        /// <returns>List of file name as string</returns>
        public static string[] GetFileNameFromFolder(string folder)
        {
            if (System.IO.Directory.Exists(folder))
            {
                return System.IO.Directory.GetFiles(folder).ToArray();
            }

            return null;
        }
       

        /// <summary>
        /// Delete file from specific folder
        /// </summary>
        /// <param name="fileName">File name with fully qualified path</param>
       public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }
       

        /// <summary>
        /// Read text file from given path
        /// </summary>
        /// <param name="filePath">Fully qualified path with file name</param>
        /// <param name="delimiter">Delimiter to separate columns</param>
        /// <returns>Data table as result</returns>
        public static DataTable ReadTextFile(string filePath, string delimiter)
        {
            DataTable employee = new DataTable("Employee");
            employee.Columns.Add("EmployeeId", typeof(int));
            employee.Columns.Add("EmployeeName", typeof(string));

            string[] lines = System.IO.File.ReadAllLines(filePath);

            if (null != lines && lines.Length > 0)
            {
                DataRow row = null;
                char delim = Convert.ToChar(delimiter);
                foreach (string line in lines)
                {
                    if (line.Split(delim).Length == 2)
                    {
                        if ((null != line.Split(delim)[0] && !string.IsNullOrEmpty(line.Split(delim)[0]))
                            && (null != line.Split(delim)[1] && !string.IsNullOrEmpty(line.Split(delim)[1])))
                        {
                            row = employee.NewRow();
                            row[0] = line.Split(delim)[0].Trim();
                            row[1] = line.Split(delim)[1].Trim();
                            employee.Rows.Add(row);

                            row = null;
                        }
                    }
                }
            }
            else
            {
                throw new NoNullAllowedException("File is empty!");
            }

            return employee;
        }
        #endregion
    }
}
