using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageTools.Utilities
{
    public interface IImagePropertyExtractor
    {
        DateTime GetOriginalCreationDateTime(string filePath);

        string GetEquipmentName(string filePath);
    }

    public class ImagePropertyExtractor : IImagePropertyExtractor
    {
        private readonly Encoding Encoding = new UTF8Encoding();

        public DateTime GetOriginalCreationDateTime(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(fs, false, false))
                {
                    try
                    {
                        var propertyItem = image.GetPropertyItem(0x9003);
                        string dateTaken = new Regex(":").Replace(Decode(propertyItem.Value), "-", 2);
                        return DateTime.Parse(dateTaken);
                    }
                    catch (Exception)
                    {
                        return File.GetCreationTime(filePath);
                    }
                }
            }
        }

        public string GetEquipmentName(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(fs, false, false))
                {
                    List<string> tempList = new List<string>();
                    tempList.Add(GetEquipmentManufacturer(image));
                    tempList.Add(GetEquipmentModel(image));
                    
                    return string.Join(" - ", tempList);
                }
            }
        }

        private string GetEquipmentManufacturer(Image image)
        {
            var propertyItem = GetPropertyItem(0x010F, image);
            return propertyItem == null ? "Unknown manufacturer" : Decode(propertyItem.Value);
        }

        private string GetEquipmentModel(Image image)
        {
            var propertyItem = GetPropertyItem(0x0110, image);
            return propertyItem == null ? "Unknown model" : Decode(propertyItem.Value);
        }
        
        private string Decode(byte[] bytes)
        {
            return Encoding.GetString(bytes).Trim('\0');
        }

        private PropertyItem GetPropertyItem(int propertyItemId, Image image)
        {
            PropertyItem propertyItem;

            try
            {
                propertyItem = image.GetPropertyItem(propertyItemId);
            }
            catch (Exception)
            {
                propertyItem = null;
            }

            return propertyItem;
        }
    }
}
