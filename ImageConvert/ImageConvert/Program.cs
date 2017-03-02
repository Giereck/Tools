using System;
using System.Linq;
using System.IO;
using ImageConvert.Converters;

namespace ImageConvert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Input ");

            string sourceFolderPath = @"E:\Users\Giereck\Pictures\New Zealand 2017";
            string targetFolderPath = @"E:\Users\Giereck\Pictures\New Zealand 2017\Jpg";

            var filesPaths = Directory.GetFiles(sourceFolderPath).Where(f => Path.GetExtension(f).Equals(".jpg", StringComparison.OrdinalIgnoreCase)).ToList();
            Console.WriteLine($"Processing {filesPaths.Count} files..");

            foreach(string filePath in filesPaths)
            {
                string targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(filePath));

                using (var converter = new BitmapToJpgConverter())
                {
                    try
                    {
                        converter.Convert(filePath, targetFilePath);
                        Console.WriteLine($"File {Path.GetFileName(filePath)} was converted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex);
                    }
                }                                   
            }

            Console.WriteLine("All done3!");
            Console.ReadKey();            
        }
    }
}
