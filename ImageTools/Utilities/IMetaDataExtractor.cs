using System;

namespace ImageTools.Utilities
{
    public interface IMetaDataExtractor
    {
        DateTime GetOriginalCreationDateTime(string filePath);

        string GetEquipmentName(string filePath);
    }
}