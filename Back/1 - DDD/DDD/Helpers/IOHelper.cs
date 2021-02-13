using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDD.Helpers
{
    public static class IOHelper
    {
        public static void CriarDiretorio(string diretorio)
        {
            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);
        }
        public static void CriarCSV(IEnumerable<string> linhas, string nomeArquivo)
        {
            using (FileStream fs = File.Create(nomeArquivo))
            {
                foreach (var item in linhas)
                {
                    AddText(fs, item);
                }
            }
        }

        public static MemoryStream CriarMemoryCSVEncoding(IEnumerable<string> linhas)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("iso-8859-1"));
            foreach (var item in linhas)
            {
                writer.Write(item);
            }
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        public static List<string> LeitorDeArquivo(string arquivo)
        {
            List<string> collection = new List<string>();

            var encoding = Path.GetExtension(arquivo).ToUpper() == ".CSV" ? Encoding.GetEncoding("iso-8859-1") : Encoding.Default;

            foreach (var line in System.IO.File.ReadLines(arquivo, encoding))
            {
                if (!string.IsNullOrEmpty(line))
                    collection.Add(line);
            }

            return collection;
        }
    }
}
