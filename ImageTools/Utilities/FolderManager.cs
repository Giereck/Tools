using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageTools.Model;

namespace ImageTools.Utilities
{
    public interface IFolderManager
    {
        IList<string> GetJpgFilesFromFolder(string folderPath);

        List<Folder> GetSubFolders(string parentFolderPath);

        Folder GetParentFolder(string folderPath);
    }

    public class FolderManager : IFolderManager
    {
        public IList<string> GetJpgFilesFromFolder(string folderPath)
        {
            return
                Directory.GetFiles(folderPath)
                    .Where(f => Path.GetExtension(f).Equals(".jpg", StringComparison.OrdinalIgnoreCase))
                    .ToList();
        }

        public List<Folder> GetSubFolders(string parentFolderPath)
        {
            List<Folder> subFolders = new List<Folder>();

            if (string.IsNullOrEmpty(parentFolderPath))
            {
                parentFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            subFolders.AddRange(Directory.GetDirectories(parentFolderPath).Select(GetFolder));
            return subFolders;
        }

        public Folder GetParentFolder(string folderPath)
        {
            if (folderPath == null) throw new ArgumentNullException(nameof(folderPath));

            Folder parentFolder;

            if (Directory.Exists(folderPath))
            {
                var parentDirectory = Directory.GetParent(folderPath);
                parentFolder = GetFolder(parentDirectory.ToString());
            }
            else
            {
                parentFolder = Folder.None;
            }

            return parentFolder;
        }

        private Folder GetFolder(string folderPath)
        {
            return new Folder(Path.GetFileNameWithoutExtension(folderPath), folderPath);
        }
    }
}
