using System;
using System.IO;

namespace ImageTools.Utilities
{
    public class MediaMetaDataExtrator : IMetaDataExtractor
    {
        private readonly IImageMetaDataExtractor _imageMetaDataExtractor;
        private readonly IVideoMetaDataExtractor _videoMetaDataExtractor;

        public MediaMetaDataExtrator(IImageMetaDataExtractor imageMetaDataExtractor, IVideoMetaDataExtractor videoMetaDataExtractor)
        {
            _imageMetaDataExtractor = imageMetaDataExtractor ?? throw new ArgumentNullException(nameof(imageMetaDataExtractor));
            _videoMetaDataExtractor = videoMetaDataExtractor ?? throw new ArgumentNullException(nameof(videoMetaDataExtractor));
        }

        public DateTime GetOriginalCreationDateTime(string filePath)
        {
            return GetExtractor(filePath).GetOriginalCreationDateTime(filePath);
        }
        
        public string GetEquipmentName(string filePath)
        {
            return GetExtractor(filePath).GetEquipmentName(filePath);
        }

        private IMetaDataExtractor GetExtractor(string filePath)
        {
            switch (filePath)
            {
                case var _ when IsImage(filePath):
                    return _imageMetaDataExtractor;
                case var _ when IsVideo(filePath):
                    return _videoMetaDataExtractor;
                default:
                    throw new ArgumentException("Specified file is not a valid type.");
            }
        }

        private bool IsImage(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }

            return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsVideo(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }

            return extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase);
        }
    }
}
