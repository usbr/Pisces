using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
//using ICSharpCode.SharpZipLib.Checksums;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.GZip;


namespace Reclamation.Core
{
    /// <summary>
    /// Summary description for ZipFile.
    /// </summary>
    public class ZipFile
    {

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
        public static event ProgressEventHandler OnProgress;

        /// <summary>
        /// compress a single file into a zip file.
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="outputZipFile"></param>
        public static void CompressFile(string fileToZip, string outputZipFile)
        {
            File.Delete(outputZipFile);
            using (var zip = System.IO.Compression.ZipFile.Open(outputZipFile, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(fileToZip, fileToZip);
            }
        }

        /// <summary>
        /// compress a directory recursively into a zip file.
        /// </summary>
        /// <param name="dirToZip"></param>
        /// <param name="outputZipFile"></param>
        public static void CompressDir(string dirToZip, string outputZipFile)
        {
            CompressDir(dirToZip, outputZipFile, new string[] { });
        }

        /// <summary>
        /// Compress all files in directory specified.
        /// <param name="extensionsToExclude">extensions to exclude including leading period. i.e. ".pdb"</param>
        /// </summary>
        public static void CompressDir(string dirToZip, string outputZipFile, string[] extensionsToExclude)
        {

            string[] filenames = FileUtility.GetFilesRecursive(dirToZip, extensionsToExclude);

            File.Delete(outputZipFile);
            using (var zip = System.IO.Compression.ZipFile.Open(outputZipFile, ZipArchiveMode.Create))
            {
                foreach (var file in filenames)
                {
                    zip.CreateEntryFromFile(file, file);
                }
            }
        }



        /// <summary>
        /// Return a list of files in the Zip archive.
        /// </summary>
        /// <param name="zipFilename"></param>
        /// <returns></returns>
        public static string[] ZipInfo(string zipFilename)
        {
            using (var zip = System.IO.Compression.ZipFile.Open(zipFilename, ZipArchiveMode.Read))
            {
                ArrayList list = new ArrayList();
                foreach (var item in zip.Entries)
                {
                    list.Add(item.Name);
                }
                string[] rval = new string[list.Count];
                list.CopyTo(rval);
                return rval;
            }
        }


        /// <summary>
        /// unzips single zip entry.
        /// </summary>
        /// <param name="zipFilename"></param>
        /// <param name="unzipFile"></param>
        /// <returns></returns>
        public static void UnzipFile(string zipFilename, string unzipFile)
        {
            using (var zip = System.IO.Compression.ZipFile.Open(zipFilename, ZipArchiveMode.Read))
            {
                var unzipDir = Path.GetDirectoryName(unzipFile);
                File.Delete(unzipFile);
                zip.ExtractToDirectory(unzipDir);
            }        
        }

        /// <summary>
        /// unzip a file into specified directory recursively.
        /// </summary>
        /// <param name="zipFilename"></param>
        /// <param name="unzipDirectory"></param>
        public static void UnzipDir(string zipFilename, string unzipDirectory)
        {
            UnzipFile(zipFilename, unzipDirectory);            
        }
    }
}

