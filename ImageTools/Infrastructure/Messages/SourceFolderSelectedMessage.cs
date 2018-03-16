using System;

namespace ImageTools.Infrastructure.Messages
{
    public class SourceFolderSelectedMessage
    {
        public SourceFolderSelectedMessage(string sourceFolderPath)
        {
            SourceFolderPath = sourceFolderPath ?? throw new ArgumentNullException(nameof(sourceFolderPath));
        }

        public string SourceFolderPath { get; }
    }
}