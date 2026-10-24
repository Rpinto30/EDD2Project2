using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2EDD2.Files
{
    internal class FileLoader
    {
        public static string[]? GetCSVData(string path)
        {
            if (Path.Exists(path))
            {
                return File.ReadAllLines(path).Skip(1).ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
