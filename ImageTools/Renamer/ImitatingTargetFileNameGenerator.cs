using System.IO;

namespace ImageTools.Renamer
{
    public interface IImitatingTargetFileNameGenerator : ITargetFileNameGenerator
    {
    }

    public class ImitatingTargetFileNameGenerator : IImitatingTargetFileNameGenerator
    {
        public string GetTargetFilePath(string originalFilePath, string targetFolderPath)
        {
            return Path.Combine(targetFolderPath, Path.GetFileName(originalFilePath));
        }
    }
}       
