using System.IO;

namespace ImageTools.Renamer
{
    public interface IImitatingFileNameGenerator : IFileNameGenerator
    {
    }

    public class ImitatingFileNameGenerator : IImitatingFileNameGenerator
    {
        public string GetTargetFilePath(string originalFilePath, string targetFolderPath)
        {
            return Path.Combine(targetFolderPath, Path.GetFileName(originalFilePath));
        }
    }
}       
