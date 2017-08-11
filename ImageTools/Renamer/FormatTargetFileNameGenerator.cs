using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageTools.Renamer
{
    public class FormatTargetFileNameGenerator : ITargetFileNameGenerator
    {
        private readonly string _format;

        public FormatTargetFileNameGenerator(string format)
        {
            _format = format;
        }

        public string GetTargetFilePath(string originalFilePath, string targetFolderPath)
        {
            string fileName = InsertDateFormatValues(originalFilePath);
            var extension = Path.GetExtension(originalFilePath);

            int index = 1;

            string uniqueFileName;

            do
            {
                string uniquifier = index == 1 ? string.Empty : "_" + index;
                uniqueFileName = Path.Combine(targetFolderPath, fileName + uniquifier + extension);
                index++;
            }
            while (File.Exists(uniqueFileName));

            return uniqueFileName;
        }

        private string InsertDateFormatValues(string originalFilePath)
        {
            string fileName = _format;
            DateTime imageDateTaken = GetImageDateTaken(originalFilePath);

            fileName = fileName.Replace("ss", imageDateTaken.TimeOfDay.Seconds.ToString("00"));
            fileName = fileName.Replace("mm", imageDateTaken.TimeOfDay.Minutes.ToString("00"));
            fileName = fileName.Replace("hh", imageDateTaken.TimeOfDay.Hours.ToString("00"));
            fileName = fileName.Replace("dd", imageDateTaken.Day.ToString("00"));
            fileName = fileName.Replace("MM", imageDateTaken.Month.ToString("00"));
            fileName = fileName.Replace("yyyy", imageDateTaken.Year.ToString());
            return fileName;
        }

        private DateTime GetImageDateTaken(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    try
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(36867);
                        string dateTaken = new Regex(":").Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        return DateTime.Parse(dateTaken);
                    }
                    catch (Exception)
                    {
                        return File.GetCreationTime(filePath);
                    }
                }
            }
        }
    }
}
