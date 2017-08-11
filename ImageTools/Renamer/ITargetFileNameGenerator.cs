namespace ImageTools.Renamer
{
    public interface ITargetFileNameGenerator
    {
        string GetTargetFilePath(string originalFilePath, string targetFolderPath);
    }
}
