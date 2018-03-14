using System;
using GalaSoft.MvvmLight;
using ImageTools.Utilities;

namespace ImageTools.Model
{
    public class Folder : ObservableObject
    {
        public static Folder Root => new Folder(@"C:\", @"c:\");
        public static Folder Default => new Folder("Desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        public static Folder None => new Folder(string.Empty, string.Empty);

        private bool _isSelected;
        private Folder _parentFolder;

        public Folder(string name, string path)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (path == null) throw new ArgumentNullException(nameof(path));

            Name = name;
            Path = path;
        }

        public string Name { get; }

        public string Path { get; }
        
        public Folder ParentFolder
        {
            get
            {
                if (_parentFolder == null)
                {
                    _parentFolder = new FolderManager().GetParentFolder(Path);
                }

                return _parentFolder;
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }
    }
}
