using System;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
//using System.IO.Compression;


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
      Crc32 crc = new Crc32();
      ZipOutputStream s = new ZipOutputStream(File.Create(outputZipFile));
		
      s.SetLevel(6); // 0 - store only to 9 - means best compression
        FileStream fs = File.OpenRead(fileToZip);
			
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        string filename = Path.GetFileName(fileToZip);
        ZipEntry entry = new ZipEntry(filename);
        entry.DateTime = DateTime.Now;
        entry.Size = fs.Length;
        fs.Close();
        crc.Reset();
        crc.Update(buffer);
			
        entry.Crc  = crc.Value;
			
        s.PutNextEntry(entry);
			
        s.Write(buffer, 0, buffer.Length);
      s.Finish();
      s.Close();
    }

    /// <summary>
    /// compress a directory recursively into a zip file.
    /// </summary>
    /// <param name="dirToZip"></param>
    /// <param name="outputZipFile"></param>
    public static void CompressDir(string dirToZip, string outputZipFile)
    {
      CompressDir(dirToZip,outputZipFile, new string[] {});
    }

    /// <summary>
    /// Compress all files in directory specified.
    /// <param name="extensionsToExclude">extensions to exclude including leading period. i.e. ".pdb"</param>
    /// </summary>
    public static void CompressDir(string dirToZip, string outputZipFile,string[] extensionsToExclude)
    {

      string[] filenames = FileUtility.GetFilesRecursive(dirToZip,extensionsToExclude);
      //string[] filenames = Directory.GetFiles(dirToZip);

      int idx = dirToZip.LastIndexOf("\\",dirToZip.Length-2);
      string relativePath="";
      if( idx >=0)
		  relativePath = dirToZip.Substring(idx);

      Crc32 crc = new Crc32();
      ZipOutputStream s = new ZipOutputStream(File.Create(outputZipFile));
		
      s.SetLevel(9); // 0 - store only to 9 - means best compression
		
        int counter =0;
      foreach (string file in filenames) 
      {
        FileStream fs = File.OpenRead(file);

			 //   Console.WriteLine(file);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        string relativeFile = file.Substring(idx+1);
        ZipEntry entry = new ZipEntry(relativeFile);
			
        entry.DateTime = DateTime.Now;
			
        // set Size and the crc, because the information
        // about the size and crc should be stored in the header
        // if it is not set it is automatically written in the footer.
        // (in this case size == crc == -1 in the header)
        // Some ZIP programs have problems with zip files that don't store
        // the size and crc in the header.
        entry.Size = fs.Length;
        fs.Close();
			
        crc.Reset();
        crc.Update(buffer);
			
        entry.Crc  = crc.Value;

        s.PutNextEntry(entry);
			  
        s.Write(buffer, 0, buffer.Length);

        if (OnProgress != null)
        {
            string msg = relativeFile + " " +buffer.Length + " bytes  ";
            int percent = (int)((double) counter /(double) filenames.Length*100);
            OnProgress(null, new ProgressEventArgs(msg, percent));
        }

         counter++;
      }
		
      s.Finish();
      s.Close();
    }



    /// <summary>
    /// Return a list of files in the Zip archive.
    /// </summary>
    /// <param name="zipFilename"></param>
    /// <returns></returns>
    public static string[] ZipInfo(string zipFilename)
    {
      ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename));
		  ArrayList list = new ArrayList();
      ZipEntry theEntry;
      while ((theEntry = s.GetNextEntry()) != null) 
      {
			  list.Add(theEntry.Name);
      }
      s.Close();

      string[] rval  = new string[list.Count];
      list.CopyTo(rval);
      return rval;
    }


    /// <summary>
    /// unzips single zip entry.
    /// </summary>
    /// <param name="zipFilename"></param>
    /// <param name="unzipFile"></param>
    /// <returns></returns>
    public static void UnzipFile(string zipFilename, string unzipFile)
    {
      ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename));
		
      ZipEntry theEntry;
      if ((theEntry = s.GetNextEntry()) != null) 
      {
			
        string directoryName = Path.GetDirectoryName(unzipFile);
        Directory.CreateDirectory(directoryName);
			
        if (unzipFile != String.Empty) 
        {
          FileStream streamWriter = File.Create(unzipFile);
				
          int size = 2048;
          byte[] data = new byte[2048];
          while (true) 
          {
            size = s.Read(data, 0, data.Length);
            if (size > 0) 
            {
              streamWriter.Write(data, 0, size);
            } 
            else 
            {
              break;
            }
          }
				
          streamWriter.Close();
        }
      }
      s.Close();

    }

    /// <summary>
    /// unzip a file into specified directory recursively.
    /// </summary>
    /// <param name="zipFilename"></param>
    /// <param name="unzipDirectory"></param>
    public static void UnzipDir(string zipFilename, string unzipDirectory)
    {
      ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilename));
		
      ZipEntry theEntry;
      while ((theEntry = s.GetNextEntry()) != null) 
      {
			
        //Console.WriteLine(theEntry.Name);
        Logger.WriteLine(theEntry.Name);
	    string fullName = unzipDirectory +"\\" + theEntry.Name;
      
        string directoryName = Path.GetDirectoryName(fullName);
        string fileName      = Path.GetFileName(fullName);
			
//        string directoryName = Path.GetDirectoryName(theEntry.Name);
//        string fileName      = Path.GetFileName(theEntry.Name);
//			
        // create directory
        Directory.CreateDirectory(directoryName);
			
        if (fileName != String.Empty) 
        {
          FileStream streamWriter = File.Create(fullName);
				
          int size = 2048;
          byte[] data = new byte[2048];
          while (true) 
          {
            size = s.Read(data, 0, data.Length);
            if (size > 0) 
            {
              streamWriter.Write(data, 0, size);
            } 
            else 
            {
              break;
            }
          }
				
          streamWriter.Close();
        }
      }
      s.Close();
    }
  }
}

