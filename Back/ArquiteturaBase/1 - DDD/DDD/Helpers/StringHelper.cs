using System.Text.RegularExpressions;

namespace DDD.Helpers
{
    public static class StringHelper
    {
        public static bool IsMatchRegex(this string s, string regex)
        {
            var match = Regex.Match(s, regex);
            return match.Success;
        }
        public static bool ApenasNumeros(this string s)
        {
            return IsMatchRegex(s, @"^[0-9]*$");
        }
        public static string RemoveMascara(this string s)
        {
            return Regex.Replace(s, "[^0-9]", "");
        }

        public static bool ContemMaiuscula(this string s)
        {
            return IsMatchRegex(s, @"[A-Z]");
        }
        public static bool ContemMinuscula(this string s)
        {
            return IsMatchRegex(s, @"[a-z]");
        }

        public static bool ContemUmCaracterEspecial(this string s)
        {
            return IsMatchRegex(s, @"[^0-9A-Za-z]");
        }
        public static bool ContemNumeros(this string s)
        {
            return IsMatchRegex(s, @"[0-9]");
        }
        public static bool ContemUmCaracterEspecialParaSenha(this string s)
        {
            return IsMatchRegex(s, @"[._$*#@]");
        }
    }
}
