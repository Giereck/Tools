using ImageTools.Model;

namespace ImageTools.Infrastructure.Messages
{
    public class SetFolderTypeModeMessage
    {
        public SetFolderTypeModeMessage(FolderType folderType)
        {
            FolderType = folderType;
        }

        public FolderType FolderType { get; }
    }
}
