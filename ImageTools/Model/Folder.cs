using System;
using GalaSoft.MvvmLight;
using ImageTools.Utilities;

namespace ImageTools.Model
{
    public class Folder : ObservableObject
    {
        public static Folder Root => new Folder("My computer", string.Empty);
        public static Folder Default => new Folder("Desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        public static Folder None => new Folder(string.Empty, string.Empty);

        private string _path;
        private bool _isSelected;
        private Folder _parentFolder;

        public Folder(string name, string path)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Name { get; }

        public string Path
        {
            get => _path;
            private set
            {
                if (IsRootFolderPath(value) && !value.EndsWith("\\"))
                {
                    value += @"\";
                }

                _path = value;
            }
        }

        private bool IsRootFolderPath(string folderPath)
        {
            var value = folderPath.Replace(":", "").Replace("\\","").ToLower();
            return value.Length == 1;
        }

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
