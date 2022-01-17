using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Camstar.Util
{
    public class DESStringProvider
    {
        private static DESCryptoServiceProvider _des = new DESCryptoServiceProvider();

        protected DESStringProvider()
        {
        }

        public static string Encrypt(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            byte[] rgbKey = new byte[8]
            {
        (byte) 77,
        (byte) 32,
        (byte) 67,
        (byte) 44,
        (byte) 19,
        (byte) 56,
        (byte) 7,
        (byte) 89
            };
            byte[] rgbIV = new byte[8]
            {
        (byte) 78,
        (byte) 2,
        (byte) 36,
        (byte) 127,
        (byte) 9,
        (byte) 12,
        (byte) 77,
        (byte) 30
            };
            byte[] bytes = Encoding.Unicode.GetBytes(s.ToCharArray());
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, DESStringProvider._des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            string empty = string.Empty;
            try
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] array = memoryStream.ToArray();
                empty = Encoding.Unicode.GetString(array, 0, array.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return empty;
        }

        public static string Decrypt(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            byte[] rgbKey = new byte[8]
            {
        (byte) 77,
        (byte) 32,
        (byte) 67,
        (byte) 44,
        (byte) 19,
        (byte) 56,
        (byte) 7,
        (byte) 89
            };
            byte[] rgbIV = new byte[8]
            {
        (byte) 78,
        (byte) 2,
        (byte) 36,
        (byte) 127,
        (byte) 9,
        (byte) 12,
        (byte) 77,
        (byte) 30
            };
            byte[] bytes = Encoding.Unicode.GetBytes(s.ToCharArray());
            MemoryStream memoryStream = new MemoryStream(bytes);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, DESStringProvider._des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Read);
            byte[] numArray = new byte[bytes.Length];
            string empty = string.Empty;
            try
            {
                int count = cryptoStream.Read(numArray, 0, numArray.Length);
                empty = Encoding.Unicode.GetString(numArray, 0, count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return empty;
        }
    }
}