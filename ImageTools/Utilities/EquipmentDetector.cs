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
        private readonly IImagePropertyExtractor _imagePropertyExtractor;

        public EquipmentDetector(IFolderManager folderManager, IImagePropertyExtractor imagePropertyExtractor)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));
            if (imagePropertyExtractor == null) throw new ArgumentNullException(nameof(imagePropertyExtractor));

            _folderManager = folderManager;
            _imagePropertyExtractor = imagePropertyExtractor;
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

            var filePaths = _folderManager.GetJpgFilesFromFolder(folderPath);

            foreach (var filePath in filePaths)
            {
                var equipmentName = _imagePropertyExtractor.GetEquipmentName(filePath);
                uniqueEquipment.Add(equipmentName);
            }

            return uniqueEquipment;
        }
    }
}
