using System.Collections.Generic;
using ImageTools.Model;

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
