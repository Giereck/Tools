using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Encoder = System.Drawing.Imaging.Encoder;

namespace ImageTools.Compressor
{
    public abstract class ImageCompressor : IDisposable
    {
        protected readonly long _imageQuality;
        private readonly string _mimeType;
        private Bitmap _bitmap;

        protected ImageCompressor(long imageQuality, string mimeType)
        {
            _imageQuality = imageQuality;
            _mimeType = mimeType;
        }
        
        public void Compress(string sourceFilePath, string targetFilePath)
        {
            _bitmap = new Bitmap(sourceFilePath);
            var imageCodecInfo = GetImageCodecInfo(_mimeType);

            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, _imageQuality);
            _bitmap.Save(targetFilePath, imageCodecInfo, parameters);
        }

        protected ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return encoders.FirstOrDefault(x => x.MimeType == mimeType);
        }

        public void Dispose()
        {
            _bitmap?.Dispose();
        }
    }
}
