using System;
using System.Collections.Generic;
using System.Linq;
using ImageTools.Model;

namespace ImageTools.Utilities
{
    public interface IEquipmentDetector
    {
        IList<Equipment> DetectEquipment(string folderPath);
    }

    public class EquipmentDetector : IEquipmentDetector
    {
        private readonly IFolderManager _folderManager;
        private readonly IMetaDataExtractor _imageMetaDataExtractor;

        public EquipmentDetector(IFolderManager folderManager, IMetaDataExtractor imageMetaDataExtractor)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));
            if (imageMetaDataExtractor == null) throw new ArgumentNullException(nameof(imageMetaDataExtractor));

            _folderManager = folderManager;
            _imageMetaDataExtractor = imageMetaDataExtractor;
        }

        public IList<Equipment> DetectEquipment(string folderPath)
        {
            List<Equipment> detectedEquipment = new List<Equipment>();
            var uniqueEquipment = DetectUniqueEquipment(folderPath);

            detectedEquipment.AddRange(uniqueEquipment.Select(equipmentName => new Equipment(equipmentName)));
            return detectedEquipment;
        }

        private HashSet<string> DetectUniqueEquipment(string folderPath)
        {
            HashSet<string> uniqueEquipment = new HashSet<string>();

            var filePaths = _folderManager.GetMediaFilesFromFolder(folderPath);

            foreach (var filePath in filePaths)
            {
                var equipmentName = _imageMetaDataExtractor.GetEquipmentName(filePath);
                uniqueEquipment.Add(equipmentName);
            }

            return uniqueEquipment;
        }
    }
}
