using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PopovDev
{
    public static class YandereLang
    {
        /// <summary>
        ///  Набор инструкций YandereLang
        /// </summary>
        private static List<string> YandereInstructions = new List<string> { "if", "else", "{", "}", "(", ")", "[", "]", ";","this", "=", "==", ",", "true", "false", "." };

        /// <summary>
        ///  Преобразует строку в код Yandere Style
        /// </summary>
        /// <param name="str">Входная строка в нормальном формате</param>
        /// <returns>Строка в яндере формате</returns>
        public static string Serialize(string str)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(str);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
                sbBytes.AppendFormat("{0:X2}", b);
            var YandereString = string.Join(" ", sbBytes.ToString().Select(x => x.ToString()).Select(x => int.Parse(x, NumberStyles.HexNumber)).Select(x => YandereInstructions[x]).ToList());
            return YandereString;
        }

        /// <summary>
        ///  Преобразует YandereDev код в нормальную строку
        /// </summary>
        /// <param name="YandereString">Входная строка в конченом формате</param>
        /// <returns>Строка в нормальном формате</returns>
        public static string Deserialize(string YandereString)
        {
            if (YandereString.Split().Any(x => !YandereInstructions.Contains(x)))
                throw new InvalidYandereDevString(YandereString);

            var Hex = string.Join("", YandereString.Split().Select(x => YandereInstructions.IndexOf(x).ToString("X")));
            int numberChars = Hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(Hex.Substring(i, 2), 16);
            return Encoding.UTF8.GetString(bytes);
        }

        [Serializable]
        /// <summary>
        /// Исключение возникающие когда Yandere dev string содержит инструкцию которую не знает YandereDev
        /// </summary>
        private class InvalidYandereDevString : Exception
        {
            public InvalidYandereDevString(string YandereString) : base($"Invalid YandereDev String {YandereString}")
            {
            }
        }
    }
}
