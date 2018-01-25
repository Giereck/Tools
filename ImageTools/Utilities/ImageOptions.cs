using System.Collections.Generic;

namespace ImageTools.Utilities
{
    public class ImageOptions
    {
        public ImageOptions()
        {
            EquipmentList = new List<Equipment>();
        }

        public List<Equipment> EquipmentList { get; } 

        public string FileFormat { get; set; }
    }
}
