namespace ImageTools.Infrastructure.Messages
{
    public class TargetFolderSelectedMessage
    {
        public TargetFolderSelectedMessage(string targetFolderPath)
        {
            TargetFolderPath = targetFolderPath;
        }

        public string TargetFolderPath { get; }
    }
}