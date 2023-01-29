using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardingHouseSystem
{
    public class FileHandler
    {
        public static void DisposeFile(string fileName)
        {
            System.IO.File.Delete(fileName);
        }

        public static void DisposeAllFile(List<string> fileNames)
        {
            foreach (string item in fileNames)
            {
                System.IO.File.Delete(item);
            }
        }

        public static string GenerateNewFileName(string fileName)
        {
            return System.String.Concat(Guid.NewGuid().ToString("N").Select(c => (char)(c + 17))).ToLower() + new System.IO.FileInfo(fileName).Extension;
        }
    }
}
