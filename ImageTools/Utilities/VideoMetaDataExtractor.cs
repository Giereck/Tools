using System;
using System.Globalization;
using MediaInfo;

namespace ImageTools.Utilities
{
    public interface IVideoMetaDataExtractor : IMetaDataExtractor
    {
    }

    public class VideoMetaDataExtractor : IVideoMetaDataExtractor
    {
        public DateTime GetOriginalCreationDateTime(string filePath)
        {
            DateTime creationDateTime;

            using (var mediaInfo = new MediaInfo.MediaInfo())
            {
                mediaInfo.Open(filePath);
                var rawValue = mediaInfo.Get(StreamKind.Video, 0, "Encoded_Date");
                var dateString = rawValue.Replace("UTC", string.Empty);
                creationDateTime = DateTime.TryParse(dateString, null, DateTimeStyles.AssumeUniversal, out DateTime dateTime) ? dateTime : DateTime.Now;
            }

            return creationDateTime;
        }

        public string GetEquipmentName(string filePath)
        {
            return "Unknown";
        }
    }
}
