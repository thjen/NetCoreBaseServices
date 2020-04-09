using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseArchitect.Utility
{
    public class Utils
    {
        #region Methods

        public static string CleanString(string dirtyString)
        {
            if (dirtyString == null || dirtyString.Trim() == string.Empty)
                return string.Empty;
            else
            {
                dirtyString = dirtyString.Trim();
                return System.Text.RegularExpressions.Regex.Replace(dirtyString, @"\s+", " ");
            }
        }

        public static string GetDateMinuteString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy HH:mm");
        }

        public static string GetDateTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy hh:mm:ss");
        }

        public static string GetDateTimeString_24H(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static bool GetDictionaryKeyByValue(Dictionary<bool, string> dicData, string value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static int GetDictionaryKeyByValue(Dictionary<int, string> dicData, string value)
        {
            var item = dicData.FirstOrDefault(c => string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));
            return item.Key;
        }

        public static string GetDictionaryValue(Dictionary<bool, string> dicInput, bool? key)
        {
            if (key != null && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }

        public static string GetDictionaryValue(Dictionary<int, string> dicInput, int? key)
        {
            if (key != null && dicInput.ContainsKey(key.Value))
                return dicInput[key.Value];

            return string.Empty;
        }

        public static string GetMonthString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("MM/yyyy");
        }

        public static string GetTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("HH:mm");
        }

        public static string RandomString(int size)
        {
            Random rnd = new Random();
            string srds = "";
            string[] str = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            for (int i = 0; i < size; i++)
            {
                srds = srds + str[rnd.Next(0, 61)];
            }
            return srds.ToUpper();
        }

        public static decimal Sum(params decimal?[] values)
        {
            decimal? result = values.Sum(c => c);
            return result == null ? 0 : result.Value;
        }

        #endregion
    }
}
