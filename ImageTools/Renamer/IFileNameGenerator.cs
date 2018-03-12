namespace ImageTools.Renamer
{
    public interface IFileNameGenerator
    {
        string GetTargetFilePath(string originalFilePath, string targetFolderPath);
    }
}
