using System;
using ImageTools.Model;

namespace ImageTools.Infrastructure.Messages
{
    public class FolderSelectedMessage
    {
        public FolderSelectedMessage(FolderType folderType, string folderPath)
        {
            FolderType = folderType;
            FolderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
        }

        public FolderType FolderType { get; }

        public string FolderPath { get; set; }
    }
}