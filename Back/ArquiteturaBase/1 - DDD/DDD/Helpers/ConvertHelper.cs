using System;

namespace DDD.Helpers
{
    public static class ConvertHelper
    {
        public static string CalculadoraBytes(long bytes)
        {
            string[] tamanhos = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < tamanhos.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            string result = String.Format("{0:0.##} {1}", len, tamanhos[order]);

            return result;
        }
        public static string CalcularSegundos(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            return time.ToString(@"hh\:mm\:ss");
        }
    }
}
