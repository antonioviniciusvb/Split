using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Split.Util
{
    public class IO
    {
        public static bool createDirectory(string path)
        {
            if (System.IO.Directory.Exists(path))
                return true;

            try
            {
                System.IO.Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static bool removeDirectory(string path)
        {
            if (!(System.IO.Directory.Exists(path)))
                return false;

            try
            {
                System.IO.Directory.Delete(path,true);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static string Hex_To_Ansi(string hex)
        {
            StringBuilder Ansi = new StringBuilder();

            var json = hex.Trim();

            try
            {
                for (int j = 0; j < json.Length - 1; j++)
                {
                    int value = int.Parse($"{json[j]}{json[j + 1]}", NumberStyles.HexNumber);
                    Ansi.Append(char.ConvertFromUtf32(value));
                    j++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return Ansi.ToString();
        }

        public static string String_To_Hex(string text)
        {

            StringBuilder Hex = new StringBuilder();

            try
            {
                byte[] caracteres = new byte[System.Text.Encoding.Unicode.GetByteCount(text)];
                caracteres = Encoding.Unicode.GetBytes(text);

                foreach (byte caractere in caracteres)
                {
                    if (caractere != 0)
                        Hex.Append($"{caractere.ToString("X")}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Hex.ToString();
        }
    }
}
