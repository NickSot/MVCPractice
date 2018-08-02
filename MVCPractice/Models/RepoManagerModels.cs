using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace MVCPractice.Models
{
    public class FileModel
    {
        public string fileName { get; set; }

        public string filePath { get; set; }

        public HttpPostedFileBase file { get; set; }
    }

    public class FolderModel {

        public string folderName { get; set; }

        public string folderPath { get; set; }

        public IEnumerable<FileModel> files;
    }

    //public delegate FileContentResult func(byte[] fileData, string type, string filename);

    public interface IRepoManagerServices {
        //delegate FileResult func(byte[] fileData, string type, string fileName);
        void UploadFile(HttpPostedFileBase file, string path);
        //FileResult DownloadFile(string path, string fileName, Controllers.RepoManagerController.func func);
        string EncryptString(string value);
        string DecryptString(string value);
    }

    public class RepoManagerServices : IRepoManagerServices {

        public void UploadFile(HttpPostedFileBase file, string path) { 

            file.SaveAs(path + @"\" + file.FileName);
        }

        public string EncryptString(string value) {
            byte[] valueBytes = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                valueBytes[i] = (byte)value[i];
            }

            RijndaelManaged aes = new RijndaelManaged();

            //string IV = "1234567891234567";
            //string key = "1234567891234567";

            byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7};
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7 };


            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.Zeros;

            MemoryStream ms = new MemoryStream();

            var cStream = new CryptoStream(ms, aes.CreateEncryptor(key, IV), CryptoStreamMode.Write);

            cStream.Write(valueBytes, 0, valueBytes.Length);

            byte[] encrVal = ms.ToArray();

            cStream.Close();
            ms.Close();

            string encrStr = null;

            for (int i = 0; i < encrVal.Length; i++)
            {
                encrStr += (char)encrVal[i];
            }

            return encrStr;
        }

        public string DecryptString(string value)
        {
            byte[] valueBytes = new byte[value.Length];

            for (int i = 0; i < value.Length; i++) {
                valueBytes[i] = (byte)value[i];
            }

            RijndaelManaged aes = new RijndaelManaged();

            byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7 };
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7 };

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.Zeros;

            MemoryStream ms = new MemoryStream(valueBytes, 0, valueBytes.Length);

            var cStream = new CryptoStream(ms, aes.CreateDecryptor(key, IV), CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(cStream);

            byte [] decrData = ASCIIEncoding.Convert(Encoding.ASCII, Encoding.UTF8, Encoding.ASCII.GetBytes(reader.ReadToEnd()));

            string decrStr = Encoding.UTF8.GetString(decrData);

            /*for (int i = 0; i < decrData.Length; i++) { 
                decrStr += (char)decrData[i];
            }*/

            reader.Close();
            cStream.Close();
            ms.Close();

            return decrStr;
        }

        /*public FileResult DownloadFile(string path, string fileName, Controllers.RepoManagerController.func func) {

            byte[] fileData = File.ReadAllBytes(path + fileName);
            
            return func(fileData,  fileName);
        }
         * */
    }
}