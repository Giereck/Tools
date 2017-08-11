using System.IO;

namespace ImageTools.Renamer
{
    public class ImitatingTargetFileNameGenerator : ITargetFileNameGenerator
    {
        public string GetTargetFilePath(string originalFilePath, string targetFolderPath)
        {
            return Path.Combine(targetFolderPath, Path.GetFileName(originalFilePath));
        }
    }
}       
