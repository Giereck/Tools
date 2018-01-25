using GalaSoft.MvvmLight;

namespace ImageTools.Utilities
{
    public class Equipment : ObservableObject
    {
        private string _name;
        private int _hourOffset;

        public Equipment(string equipmentName)
        {
            Name = equipmentName;
            HourOffset = 0;
        }

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public int HourOffset
        {
            get { return _hourOffset; }
            set { Set(ref _hourOffset, value); }
        }
    }
}
