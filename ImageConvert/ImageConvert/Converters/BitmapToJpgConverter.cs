using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageConvert.Converters
{
    public class BitmapToJpgConverter : IImageConverter
    {
        public void Convert(string sourceFilePath, string targetFilePath)
        {
            var bitmap = new Bitmap(sourceFilePath);
            var imageCodecInfo = GetImageCodecInfo("image/jpeg");

            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100);
            bitmap.Save(targetFilePath, imageCodecInfo, parameters);
        }

        private ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return encoders.FirstOrDefault(x => x.MimeType == mimeType);
        }
    }
}
