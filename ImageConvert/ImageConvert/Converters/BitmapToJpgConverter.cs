﻿using System.Drawing;
using System.Drawing.Imaging;

namespace ImageConvert.Converters
{
    public class BitmapToJpgConverter : ImageConverterBase, IImageConverter
    {
        public void Convert(string sourceFilePath, string targetFilePath)
        {
            _bitmap = new Bitmap(sourceFilePath);
            var imageCodecInfo = GetImageCodecInfo("image/jpeg");

            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 50L);
            _bitmap.Save(targetFilePath, imageCodecInfo, parameters);
        }        
    }
}
