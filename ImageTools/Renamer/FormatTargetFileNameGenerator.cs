using System;
using System.IO;
using System.Linq;
using ImageTools.Utilities;

namespace ImageTools.Renamer
{
    public interface IFormatTargetFileNameGenerator : ITargetFileNameGenerator
    {
        ImageOptions Options { get; set; }
    }

    public class FormatTargetFileNameGenerator : IFormatTargetFileNameGenerator
    {
        private readonly IImagePropertyExtractor _imagePropertyExtractor;

        public FormatTargetFileNameGenerator(IImagePropertyExtractor imagePropertyExtractor)
        {
            if (imagePropertyExtractor == null) throw new ArgumentNullException(nameof(imagePropertyExtractor));

            _imagePropertyExtractor = imagePropertyExtractor;
        }

        public ImageOptions Options { get; set; }

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
            string fileName = Options.FileFormat;
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
            var equipmentName = _imagePropertyExtractor.GetEquipmentName(filePath);
            var dateTaken = _imagePropertyExtractor.GetOriginalCreationDateTime(filePath);

            var equipment = Options.EquipmentList.FirstOrDefault(e => e.Name == equipmentName);

            if (equipment != null)
            {
                dateTaken = dateTaken.AddHours(equipment.HourOffset);
            }

            return dateTaken;
        }
    }
}
