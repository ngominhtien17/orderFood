using System.Text.RegularExpressions;

namespace OrderFood.Extension
{
    public static class Extension
    {
        public static string ToVnd(this double donGia)
        {
            return donGia.ToString("#,##0") + "d";
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join("", words);
            }
            return result;
        }
        //public static string ToUrlFriendly(this string url)
        //{
        //    var result = url.ToLower().Trim();
        //    result = Regex.Replace(result, "áàaàäääääääääääää", "a");
        //    result = Regex.Replace(result, "éèêéêêêêêée", "e");
        //    result = Regex.Replace(result, "ooooooooooooooooo", "o");
        //    result = Regex.Replace(result, "úùyúũưứừữ", "u");
        //    result = Regex.Replace(result, "íiiii", "i");
        //    result = Regex.Replace(result, "ýyyyy", "y");
        //    result = Regex.Replace(result, "d", "d");
        //    result = Regex.Replace(result, "[^a-z0-9-]", "");
        //    result = Regex.Replace(result, "(-)+", "-");
        //    return result;
        //}
    }
}
