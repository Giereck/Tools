namespace ImageTools.Compressor
{
    public class JpgCompressor : ImageCompressor
    {
        public JpgCompressor(long quality) : base(quality, "image/jpeg")
        {
        }
    }
}
