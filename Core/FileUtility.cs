using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Reclamation.Core
{
    public static class FileUtility
    {
        static List<string> fileList = new List<string>();
        /// <summary>
        /// Gets list of files
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string directoryName)
        {
            return GetFilesRecursive(directoryName, new string[] { });
        }
        

        /// <summary>
        /// returns a list of files.
        /// </summary>
        /// <param name="directoryName">starting path to recurse</param>
        /// <param name="extensionsToExclude">extensions to skip (includes leading .)</param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string directoryName, string[] extensionsToExclude)
        {
            fileList.Clear();
            List<string> finalList = new List<string>();
            if (Directory.Exists(directoryName))
            {
                ProcessDirectory(directoryName);
            }
            // remove files with extensions in exclude list.
            for (int i = 0; i < fileList.Count; i++)
            {
                string file = fileList[i].ToString();
                string ext = Path.GetExtension(file);
                bool keepFile = true;
                for (int j = 0; j < extensionsToExclude.Length; j++)
                {
                    if (extensionsToExclude[j].ToLower() == ext.ToLower())
                    {
                        keepFile = false;
                        break;
                    }
                }
                if (keepFile)
                {
                    finalList.Add(file);
                }
            }
            
            
            
            fileList.Clear();
            return finalList.ToArray();
        }

        /// <summary>
        ///Process all files in the directory passed in, recurse on any directories  
        ///that are found, and process the files they contain.
        /// </summary>
        /// <param name="targetDirectory"></param>
        private static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                fileList.Add(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void CopyFiles(string srcPath, string destPath, string searchPattern = "*.*", bool overwrite = true,
            bool preserveAttributes=false)
        {
            string[] files = Directory.GetFiles(srcPath, searchPattern);
            foreach (string srcFile in files)
            {
                string destFile = Path.Combine(destPath, Path.GetFileName(srcFile));
                if( !overwrite && File.Exists(destFile))
                {
                   // skip copy since file exists.
                }
                else
                {
                File.Copy(srcFile, destFile,overwrite);
                File.SetCreationTime(destFile, File.GetCreationTime(srcFile));
                File.SetLastAccessTime(destFile, File.GetLastAccessTime(srcFile));
                File.SetLastWriteTime(destFile, File.GetLastWriteTime(srcFile));
                }
            }
        }


        public static string GetTempPath()
        {
            string s = Path.GetTempPath();
            string n = System.Windows.Forms.Application.ProductName;

            s = Path.Combine(s,"Reclamation");
            s = Path.Combine(s, n);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }

            return s;
        }


        public static void CleanTempPath()
        {
           
            string path = GetTempPath();
            Logger.WriteLine("temp path = '" + path + "'");
            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string f in files)
                {
                  Logger.WriteLine("Deleting " + f + " from cache");
                  File.Delete(f);
                }
            }
            catch (Exception)
            {
            }

        }

        public static string GetExecutableDirectory()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        /// C:\Users\UserName\AppData\Local\Reclamation\ProductName  (Windows 7)
        /// </summary>
        /// <returns></returns>
        public static string GetLocalApplicationPath()
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string n = System.Windows.Forms.Application.ProductName;

            s = Path.Combine(s, "Reclamation");
            s = Path.Combine(s, n);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }

            return s;
          
        }

        /// <summary>
        /// create a simple 8 character unique temporary filename with specified extension
        /// in a temporary directory
        /// <param name="extension">extension includes leading .</param>
        /// </summary>
        public static string GetTempFileName(string extension)
        {
            string path = GetTempPath();
            return GetTempFileNameInDirectory(path, extension);
        }

        public static string GetTempFileNameInDirectory(string path, string extension)
           {
               return GetTempFileNameInDirectory(path, extension, "temp");
        }

        /// <summary>
        /// create a simple 8 character unique temporary filename in the specfied directory
        /// </summary>
        public static string GetTempFileNameInDirectory(string path, string extension, string filePrefix)
        {
            string fn = "tmp1.tmp";
            string fullName = "";
            for (int i = 0; i < 500000; i++)
            {
                fn = filePrefix + i + extension;//".tmp";
                fullName = Path.Combine( path ,  fn);
                if (!File.Exists(fullName))
                {
                    // create zero byte file. to reserve this name..
                    File.Create(fullName).Close();
                    return fullName;
                   // break;
                }
            }

                return fullName+Guid.NewGuid().ToString();
        }


        /// <summary>
        /// read file and convert to array of bytes
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] ReadFileAsBytes(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] asm = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return asm;
        }

        /// <summary>
        /// Read a file into memory and return a hash.
        /// used to identify a specific version of a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetSha1Hash(string filename)
        {
            FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            sha1.ComputeHash(f);
            f.Close();

            byte[] hash = sha1.Hash;
            StringBuilder buff = new StringBuilder();
            foreach (byte hashByte in hash)
            {
                buff.Append(String.Format("{0:X1}", hashByte));
            }
            string rval = buff.ToString();
            return rval;
        }


            /// <summary>
            /// Creates a relative path from one file
            /// or folder to another.
            /// </summary>
            /// <param name="fromDirectory">
            /// Contains the directory that defines the 
            /// start of the relative path.
            /// </param>
            /// <param name="toPath">
            /// Contains the path that defines the
            /// endpoint of the relative path.
            /// </param>
            /// <returns>
            /// The relative path from the start
            /// directory to the end path.
            /// </returns>
            /// <exception cref="ArgumentNullException"></exception>
            public static string RelativePathTo(
                string fromDirectory, string toPath)
            {
                  if (fromDirectory == null)
                    throw new ArgumentNullException("fromDirectory");
                if (toPath == null)
                    throw new ArgumentNullException("toPath");

                bool isRooted = Path.IsPathRooted(fromDirectory)
                                  && Path.IsPathRooted(toPath);

                if (isRooted)
                {
                    bool isDifferentRoot = string.Compare(

                        Path.GetPathRoot(fromDirectory),

                        Path.GetPathRoot(toPath), true) != 0;



                    if (isDifferentRoot)

                        return toPath;

                }



                StringCollection relativePath = new StringCollection();

                string[] fromDirectories = fromDirectory.Split(

                    Path.DirectorySeparatorChar);



                string[] toDirectories = toPath.Split(

                    Path.DirectorySeparatorChar);



                int length = Math.Min(

                    fromDirectories.Length,

                    toDirectories.Length);



                int lastCommonRoot = -1;



                // find common root

                for (int x = 0; x < length; x++)
                {

                    if (string.Compare(fromDirectories[x],

                        toDirectories[x], true) != 0)

                        break;



                    lastCommonRoot = x;

                }

                if (lastCommonRoot == -1)

                    return toPath;



                // add relative folders in from path

                for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)

                    if (fromDirectories[x].Length > 0)

                        relativePath.Add("..");



                // add to folders to path

                for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)

                    relativePath.Add(toDirectories[x]);



                // create relative path

                string[] relativeParts = new string[relativePath.Count];

                relativePath.CopyTo(relativeParts, 0);



                string newPath = string.Join(

                    Path.DirectorySeparatorChar.ToString(),

                    relativeParts);



                return newPath;

            }

            public static bool FilesAreEqual(string file1, string file2)
            {
                return GetSha1Hash(file1) == GetSha1Hash(file2);
            }

        
        
        /// <summary>
        /// Returns true if the file is less than 20 minutes old
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
            public static bool IsFileNewEnough(string filename)
            {
                if (!File.Exists(filename))
                    return false;

                FileInfo fi = new FileInfo(filename);
                // int minutes = GetCacheAllowedMinutes();
                bool rval = fi.CreationTime.AddMinutes(20) > DateTime.Now;
                return rval;
            }
            /// <summary>
            /// Gets the full path to a file using the following preference order:
            /// 1) executable path 
            /// 2) Gloabal path Globals.MetaDataPath (network drive)
            /// 3) web site (or temporary directory copy)
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            public static string GetFileReference(string filename)
            {
                string rval = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), filename);
                if (File.Exists(rval))
                {
                    Logger.WriteLine("Using local file " + rval);
                    return rval;
                }


                rval = Path.Combine(Globals.LocalConfigurationDataPath, filename);
                Logger.WriteLine("Requesting file '" + rval + "'");


                Logger.WriteLine("File Exists  = " + File.Exists(rval));
                if ( !File.Exists(rval))
                {

                    rval = Path.Combine(FileUtility.GetTempPath(), filename);
                    if (File.Exists(rval) && FileUtility.IsFileNewEnough(rval))
                    {
                        Logger.WriteLine("using temporary directory copy of file");
                    }
                    else
                    {
                        string webFile = "http://www.usbr.gov/pn/hydromet/configurationdata/" + filename;
                        Logger.WriteLine("Copying from " + webFile);
                        Web.GetFile(webFile, rval);
                    }
                }
                if (!File.Exists(rval))
                {
                    Logger.WriteLine("Warning: could not find " + filename);
                    throw new FileNotFoundException(filename);
                }

                return rval;
            }
    }
}
