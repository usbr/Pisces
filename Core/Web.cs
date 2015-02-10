using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Net.NetworkInformation;
namespace Reclamation.Core
{
    public static class Web
    {
        public delegate void ProgressEventHandler(object sender, BytesReadEventArgs e);
        public static event ProgressEventHandler OnProgress;


        private static string UserAgent()
        {
            string s = System.AppDomain.CurrentDomain.FriendlyName;
            s = s + " hydromet@usbr.gov";
            return s;
           
        }


        public static string[] GetPage(String url)
        {
            return GetPage(url, false);
        }

        public static string[] GetPage(String url, bool useCache)
        {
            return GetPage(url, useCache, "", "");
        }


        /// <summary>
        /// Gets page using GET method.
        /// </summary>
        public static string[] GetPage(String url, bool useCache,string username, string password)
        {

            WebResponse result = null;

            Logger.WriteLine(url);
            StringBuilder stringBuilder = new StringBuilder("", 4000);
            try
            {
                if (useCache && SimpleWebCache.Available(url))
                {
                    return SimpleWebCache.Read(url);
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                SetupProxy(req);
                req.UserAgent = UserAgent();
                if (username != "" && password != "")
                {
                    NetworkCredential nc = new NetworkCredential(username, password);
                    CredentialCache c = new CredentialCache();
                    c.Add(new Uri(url), "NTLM", nc);
                    req.Credentials = c;
                }
            

                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                // Console.WriteLine("\r\nResponse stream received");
                if (true)
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);
                    int bytesRead = count * 16;
                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        stringBuilder.Append(str);
                        count = sr.Read(read, 0, 256);
                        bytesRead += count * 16;
                        if (OnProgress != null)
                        {
                            OnProgress(null, new BytesReadEventArgs("reading", bytesRead));
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("\r\nDebug0:The request URI could not be found or was malformed " + ex.Message);
                if (result != null)
                {
                    Console.WriteLine("Debug1:");
                    Console.WriteLine("Debug2: "+result.ToString());
                }
                throw ex;
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }

            stringBuilder.Replace("\r", "");
            string[] rval = stringBuilder.ToString().Split(new char[] { '\n' });

            if (useCache)
            {
                SimpleWebCache.Save(url, rval);
            }

            return rval;
        }

        public static void GetTextFile(string url, string outputFilename, bool useCache)
        {
            var lines = GetPage(url, useCache);

            File.WriteAllLines(outputFilename, lines);

        }
        public static void GetFile(string url, string outputFilename)
        {
            GetFile(url, outputFilename, "", "");
        }

        public static void GetFile(string url, string outputFilename, string username, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = UserAgent();
            if (username != "" && password != "")
            {
                NetworkCredential nc = new NetworkCredential(username, password);
                CredentialCache c = new CredentialCache();
                c.Add(new Uri(url), "NTLM", nc);
                request.Credentials = c;
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Logger.WriteLine("Content length is "+ response.ContentLength);
            Logger.WriteLine("Content type is "+ response.ContentType);
            //response.get
            
            Stream s = response.GetResponseStream();

            byte[] bytes = Web.ReadFully(s);

            FileStream fs = new FileStream(outputFilename, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
        }

        public static string[] GetPage(String url, String payload)
        {
           return GetPage(url, payload, false);
        }
        public static string[] GetPage(String url, String payload, bool useCache)
        {
            Logger.WriteLine(url);
            Logger.WriteLine(payload);
            if (useCache && SimpleWebCache.Available(url+payload))
            {
                
                return SimpleWebCache.Read(url+payload);
            }

            StringBuilder stringBuilder = new StringBuilder("", 4000);
            WebResponse result = null;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.UserAgent = UserAgent();

                SetupProxy(req);
                
              //  req.Headers.Add("user-agent",UserAgent());
                req.ContentType = "application/x-www-form-urlencoded";
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;

                if (payload != null)
                {
                    int i = 0, j;
                    while (i < payload.Length)
                    {
                        j = payload.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            UrlEncoded.Append(HttpUtility.UrlEncode(payload.Substring(i, payload.Length - i)));
                            break;
                        }
                        UrlEncoded.Append(HttpUtility.UrlEncode(payload.Substring(i, j - i)));
                        UrlEncoded.Append(payload.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.UTF8.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }


                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);

                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    //Console.Write(str);
                    stringBuilder.Append(str);
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine(e.ToString());
                stringBuilder.Append(e.Message);
                Logger.WriteLine("\r\nThe request URI could not be found or was malformed");
                throw new Exception(url + " POSTDATA=" + payload + " " + e.Message);
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            stringBuilder.Replace("\r", "");
            string[] rval = stringBuilder.ToString().Split(new char[] { '\n' });

            if (useCache)
            {
                SimpleWebCache.Save(url + payload, rval);   
            }
            return rval;
        }

      

        /// <summary>
        /// http://www.yoda.arachsys.com/csharp/
        /// 
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        private static byte[] ReadFully(Stream stream)
        {
           int initialLength = 32768;

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }


        private static string[] GetPageAuth(String url, bool useCache, ICredentials credentials)
        {
            WebResponse result = null;

            StringBuilder stringBuilder = new StringBuilder("", 4000);
            try
            {
                if (useCache && SimpleWebCache.Available(url))
                {
                    return SimpleWebCache.Read(url);
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = UserAgent();
                
                req.Credentials = credentials;

                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                // Console.WriteLine("\r\nResponse stream received");
                if (true)
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);
                    int bytesRead = count * 16;
                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        stringBuilder.Append(str);
                        count = sr.Read(read, 0, 256);
                        bytesRead += count * 16;
                        if (OnProgress != null)
                        {
                            OnProgress(null, new BytesReadEventArgs("reading", bytesRead));
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("\r\nThe request URI could not be found or was malformed " + ex.Message);
                throw ex;
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }

            stringBuilder.Replace("\r", "");
            string[] rval = stringBuilder.ToString().Split(new char[] { '\n' });

            if (useCache)
            {
                SimpleWebCache.Save(url, rval);
            }

            return rval;
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
        private static void SetupProxy(HttpWebRequest request)
        {
            if (IsRunningOnMono())
                return;
            if (NetworkUtility.Intranet)
                return; // assuming internal network does not have proxy. 100% performance boost
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = (System.Net.NetworkCredential)CredentialCache.DefaultCredentials;
                request.Proxy = proxy;
        }

    }
}
