using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2EDD2.Files
{

    public class CsvManager
    {
        public static void ClearCSV(string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
        }

        public static void CreateCsvFile(string filePath, string[] headers, string[] data)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            using (var writer = File.AppendText(filePath))
            {
                if (fileInfo.Exists && fileInfo.Length == 0)
                    writer.WriteLine(string.Join("|", headers));

                writer.WriteLine(string.Join("|", data));
            }
        }

        public static void CreateCsvFileWrite(string filePath, string[] headers, string[][] data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(string.Join("|", headers));
                foreach (var row in data)
                {
                    writer.WriteLine(string.Join("|", row));
                }
            }
        }

        public static string[][]? ReadCsvFile(string filePath)
        {
            if (!Path.Exists(filePath)) return null;
            var lines = File.ReadAllLines(filePath).Skip(1).ToArray();

            var data = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                data[i] = lines[i].Split('|');
            }
            return data;
        }
    }
}
