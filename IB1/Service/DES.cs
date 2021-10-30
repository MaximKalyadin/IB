using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows;

namespace IB1.Service
{
    public static class DES
    {
        static string sKey = "adminMax";

        public static void EncryptFile(string source, string pathEncrypt)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypt = new FileStream(pathEncrypt, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.ASCII.GetBytes(sKey);
            DES.IV = Encoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(fsEncrypt, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 0];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Close();

            fsInput.Close();
            fsEncrypt.Close();
        }

        public static void DecryptFile(string source, string pathEncrypt)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypt = new FileStream(pathEncrypt, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Key = Encoding.ASCII.GetBytes(sKey);
            DES.IV = Encoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fsEncrypt, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 0];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Close();

            fsInput.Close();
            fsEncrypt.Close();
        }

        public static string ToSHA256(string s)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
