﻿using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Camstar.Util
{
    public class RC2StringProvider
    {
        private const string cCSPNAME = "Microsoft Base Cryptographic Provider v1.0";
        private const string cHASHNAME = "MD5";
        private const string cALGNAME = "RC2";

        public string Encrypt(string strPassword, string strMessage)
        {
            CspParameters cspParams = new CspParameters(1, "Microsoft Base Cryptographic Provider v1.0");
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, null, "MD5", 1, cspParams);
            byte[] rgbIV = new byte[8];
            byte[] numArray = passwordDeriveBytes.CryptDeriveKey("RC2", "MD5", 0, rgbIV);
            RC2CryptoServiceProvider cryptoServiceProvider = new RC2CryptoServiceProvider();
            cryptoServiceProvider.Key = numArray;
            cryptoServiceProvider.IV = rgbIV;
            byte[] bytes = new UnicodeEncoding().GetBytes(strMessage);
            ICryptoTransform encryptor = cryptoServiceProvider.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] b = memoryStream.ToArray();
            cryptoStream.Close();
            return this.ConvertToHex(b);
        }

        public string Decrypt(string strPassword, string strMessage)
        {
            if (strMessage == null || strMessage.Length == 0)
                return null;
            CspParameters cspParams = new CspParameters(1, "Microsoft Base Cryptographic Provider v1.0");
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, (byte[])null, "MD5", 1, cspParams);
            byte[] rgbIV = new byte[8];
            byte[] numArray = passwordDeriveBytes.CryptDeriveKey("RC2", "MD5", 0, rgbIV);
            RC2CryptoServiceProvider cryptoServiceProvider = new RC2CryptoServiceProvider();
            cryptoServiceProvider.Key = numArray;
            cryptoServiceProvider.IV = rgbIV;
            byte[] buffer = this.ConvertFromHex(strMessage);
            ICryptoTransform decryptor = cryptoServiceProvider.CreateDecryptor();
            MemoryStream memoryStream = new MemoryStream(buffer.Length);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            memoryStream.Write(buffer, 0, buffer.Length);
            memoryStream.Position = 0L;
            string end = new StreamReader((Stream)cryptoStream, Encoding.Unicode).ReadToEnd();
            cryptoStream.Close();
            return end;
        }

        private string ConvertToHex(byte[] EncMsg)
        {
            string str = "";
            int length = EncMsg.Length;
            if (length > 0)
            {
                for (long index = 0; index < (long)length; ++index)
                    str += EncMsg[index].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

        private byte[] ConvertFromHex(string HexString)
        {
            if (HexString == null || HexString.Length == 0)
                return (byte[])null;
            int length = HexString.Length / 2;
            byte[] numArray = new byte[length];
            for (int index = 0; index < length; ++index)
            {
                string s = HexString.Substring(index * 2, 2);
                try
                {
                    numArray[index] = (byte)int.Parse(s, NumberStyles.HexNumber);
                }
                catch (FormatException ex)
                {
                    throw new CryptographicException("Input parameter was not a valid hex-encoded string", (Exception)ex);
                }
            }
            return numArray;
        }
    }
}