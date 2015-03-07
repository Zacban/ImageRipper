using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ImageUtil {
    public static class ImageUtil {
        public static Byte[] GetImageStreamFromUrl(string url) {
            Byte[] byteStream = null;

            try {
                var urlRequest = (HttpWebRequest)WebRequest.Create(url);

                using (var response = (HttpWebResponse)urlRequest.GetResponse()) {
                    using (var reader = new BinaryReader(response.GetResponseStream())) {
                        byteStream = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    }
                }

            }
            catch (System.NotSupportedException nse) {

            }
            catch (ArgumentException ae) {

            }
            catch (ObjectDisposedException ode) {

            }
            catch (SecurityException se) {

            }
            catch (UriFormatException ufe) {

            }
            catch (ProtocolViolationException pve) {

            }
            catch (WebException we) {

            }
            catch (InvalidOperationException ioe) {

            }
            
            finally {

            }

            return byteStream;
        }

        public static bool SaveImageStreamToFolder(string base64String, string filename) {
            Byte[] byteStream = null;
            try {
                byteStream = Convert.FromBase64String(base64String);
            }
            catch (ArgumentNullException ane) {
                return false;
            }
            catch (FormatException fe) {
                return false;
            }

            return SaveImageStreamToFolder(byteStream, filename);
        }

        public static bool SaveImageStreamToFolder(Byte[] byteStream, string filename) {
            var success = false;

            var dirPath = filename.Substring(0, filename.LastIndexOf('\\'));
            var directory = new DirectoryInfo(dirPath);
            if (!directory.Exists)
                directory.Create();

            try {
                using (var fStream = new FileStream(filename, FileMode.Create)) {
                    fStream.Write(byteStream, 0, byteStream.Length);
                }
            }
            catch (ArgumentNullException ane) {

            }
            catch (ArgumentOutOfRangeException aoore) {

            }
            catch (ArgumentException ae) {

            }
            catch (NotSupportedException nse) {

            }
            catch (SecurityException se) {

            }
            catch (FileNotFoundException fnfe) {

            }
            catch (DirectoryNotFoundException dnfe) {

            }
            catch (PathTooLongException ptle) {

            }
            catch (IOException ie) {

            }
            
            return success;
        }
    }
}
