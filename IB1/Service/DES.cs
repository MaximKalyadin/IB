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
        /// <summary>
        /// ключ для шифрованиия и разшифрования файлов
        /// </summary>
        static readonly string sKey = "adminMax";

        /// <summary>
        /// шифрование файла
        /// </summary>
        /// <param name="source">путь файла котого надо шифровать</param>
        /// <param name="pathEncrypt">путь в какой файл зашифровать</param>
        public static void EncryptFile(string source, string pathEncrypt)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypt = new FileStream(pathEncrypt, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(sKey),
                IV = Encoding.ASCII.GetBytes(sKey)
            };
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(fsEncrypt, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 0];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Close();

            fsInput.Close();
            fsEncrypt.Close();
        }

        /// <summary>
        /// расшифрование файла
        /// </summary>
        /// <param name="source">путь файла котого надо расшифровать</param>
        /// <param name="pathEncrypt">путь в какой файл расшифровать</param>
        public static void DecryptFile(string source, string pathEncrypt)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypt = new FileStream(pathEncrypt, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(sKey),
                IV = Encoding.ASCII.GetBytes(sKey)
            };
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fsEncrypt, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 0];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptoStream.Close();

            fsInput.Close();
            fsEncrypt.Close();
        }

        /// <summary>
        /// Хеширование паролей методом sha
        /// </summary>
        /// <param name="s">строка пароля</param>
        /// <returns></returns>
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
