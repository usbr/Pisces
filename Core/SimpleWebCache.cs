using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Configuration;

namespace Reclamation.Core
{
    /// <summary>
    /// Caches results of web queries
    /// </summary>
    public static class SimpleWebCache
    {

        public static bool Available(string url)
        {
            int idx = CacheIndex.IndexOf(url);
            if (idx < 0)
                return false;
            string filename = CacheIndex[idx-1];
            return FileUtility.IsFileNewEnough(filename);
        }



        private static string WebTempPath()
        {
            string s = FileUtility.GetTempPath();
            CleanupCache(s);
            return s;
        }

        


        static bool s_cacheIsClear = false;
        private static void CleanupCache(string path)
        {
            if (s_cacheIsClear)
                return;

            try
            {
                s_cacheIsClear = true;
                string[] files  =Directory.GetFiles(path);
                foreach (string  f in files)
                {
                    if (!FileUtility.IsFileNewEnough(f))
                    {
                        Logger.WriteLine("Deleting " + f +" from cache"); 
                        File.Delete(f);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        

        /// <summary>
        /// Saves an lines of data coresponding to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static void Save(string url, string[] data)
        {
            TextFile tf = CacheIndex;
            string dataFile = FileUtility.GetTempFileName(".tmp");
            tf.Add(dataFile);
            tf.Add(url);
            tf.SaveAs(tf.FileName);

            TextFile df = new TextFile(data);
            df.SaveAs(dataFile);
            Logger.WriteLine("Caching " + url);
            Logger.WriteLine("\"" + dataFile+"\"");
        }
        public static string[] Read(string url)
        {

            TextFile tf = CacheIndex;
            int idx = tf.IndexOf(url);
            if (idx < 0)
                throw new FileNotFoundException("error in cache");
            TextFile df = new TextFile(tf[idx - 1]);
            
           Logger.WriteLine("Cache hit for "+url);
           Logger.WriteLine("\"" + tf[idx - 1]+"\"");
            return df.FileData;
        }

        private static TextFile CacheIndex
        {
            get
            {

                string filename = Path.Combine(WebTempPath() ,"simple_web_cache"+DateTime.Now.ToString("yyyy-MM-dd")+".txt");
                if (File.Exists(filename))
                {
                    return new TextFile(filename);
                }
                File.Create(filename).Close();
                TextFile tf = new TextFile(filename);
                return tf;
            }
        }

    }
}
