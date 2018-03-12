using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageTools.Model;

namespace ImageTools.Utilities
{
    public interface IFolderManager
    {
        List<Folder> GetSubFolders(string parentFolderPath);
    }

    public class FolderManager : IFolderManager
    {
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

        private Folder GetFolder(string folderPath)
        {
            return new Folder(Path.GetFileNameWithoutExtension(folderPath), folderPath);
        }
    }
}
