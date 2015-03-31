using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Reclamation.Core
{
    public static class AssemblyUtility
    {

        public static DateTime CreationDate()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileInfo fi = new FileInfo(asm.Location);
            //Console.WriteLine(fi.CreationTime);
            return fi.CreationTime;
        }

        public static string GetAssemblyPath(string assemblyName)
        {
            Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assem in assems)
            {
                //file:///C:/Documents and Settings/ktarbet/My Documents/project/Reclamation/TimeSeries/bin/Debug/Reclamation.TimeSeries.DLL
                if (String.Compare(assem.FullName.Split(',')[0], assemblyName, true) == 0)
                {
                    //    Console.WriteLine("CodeBase: " + assem.CodeBase);
                    //    Console.WriteLine("Location: " + assem.Location);
                    //    Console.WriteLine("FullName: " + assem.FullName);

                    string path = System.IO.Path.GetDirectoryName(assem.CodeBase);
                    path = path.Replace("file:\\", "");

                    //Console.WriteLine("path:  " + path);
                    return path;



                    // search for test Data Path
                }
            }
            return "";
            //
        }

    }
}
