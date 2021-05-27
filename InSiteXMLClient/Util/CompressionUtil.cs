using System;
using System.IO;
using System.Text;

namespace Camstar.Util
{
  public sealed class CompressionUtil
  {
    public static string StringToCompressedBase64(string text)
    {
      //Deflater deflater = new Deflater(1, true);
      //MemoryStream memoryStream = new MemoryStream();
      //byte[] numArray = new byte[256];
      //deflater.SetInput(new UnicodeEncoding().GetBytes(text));
      //deflater.Flush();
      //int count;
      //do
      //{
      //  count = deflater.Deflate(numArray, 0, numArray.Length);
      //  memoryStream.Write(numArray, 0, count);
      //}
      //while (count > 0);
      //return Convert.ToBase64String(memoryStream.ToArray());
      return null;
    }

    public static string CompressedBase64ToString(string compressed)
    {
            //byte[] buffer = new byte[1024];
            //MemoryStream memoryStream = new MemoryStream();
            //Inflater inflater = new Inflater(true);
            //inflater.SetInput(Convert.FromBase64String(compressed));
            //int count;
            //do
            //{
            //  count = inflater.Inflate(buffer, 0, buffer.Length);
            //  memoryStream.Write(buffer, 0, count);
            //}
            //while (count > 0);
            //string str = new UnicodeEncoding().GetString(memoryStream.ToArray(), 0, (int) memoryStream.Length);
            //memoryStream.Close();
            //return str;
            return null;
        }
  }
}
