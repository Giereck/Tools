using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageTools.Compressor
{
    public class JpgCompressor : IDisposable
    {
        private readonly long _quality;
        private  Bitmap _bitmap;
        
        public JpgCompressor(long quality)
        {
            _quality = quality; // default value was 50
        }

        public void Compress(string sourceFilePath, string targetFilePath)
        {
            _bitmap = new Bitmap(sourceFilePath);
            var imageCodecInfo = GetImageCodecInfo("image/jpeg");

            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, _quality);
            _bitmap.Save(targetFilePath, imageCodecInfo, parameters);
        }

        private ImageCodecInfo GetImageCodecInfo(string mimeType)
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
