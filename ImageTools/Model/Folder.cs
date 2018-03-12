using System;
using GalaSoft.MvvmLight;

namespace ImageTools.Model
{
    public class Folder : ObservableObject
    {
        private bool _isSelected;

        public Folder(string name, string path)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (path == null) throw new ArgumentNullException(nameof(path));

            Name = name;
            Path = path;
        }

        public string Name { get; }

        public string Path { get; }
        
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }
    }
}
