using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageConvert.Converters
{
    public abstract class ImageConverterBase : IDisposable
    {
        protected Bitmap _bitmap;

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
