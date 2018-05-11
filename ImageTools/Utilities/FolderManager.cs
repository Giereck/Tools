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
                //parentFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var logicalDrives = Directory.GetLogicalDrives();

                foreach (var drive in logicalDrives)
                {
                    subFolders.Add(new Folder(drive.ToUpper(), drive));
                }
            }
            else
            {
                var accessibleDirectories = DiscardUnauthorizedDirectories(Directory.GetDirectories(parentFolderPath));
                subFolders.AddRange(accessibleDirectories.Select(ConvertToFolder));
            }

            
            return subFolders;
        }

        private IList<string> DiscardUnauthorizedDirectories(string[] directoryPaths)
        {
            return directoryPaths.Where(HasAccessToDirectory).ToList();
        }
        
        public Folder GetParentFolder(string folderPath)
        {
            if (folderPath == null) throw new ArgumentNullException(nameof(folderPath));
            
            if(string.IsNullOrEmpty(folderPath)) return Folder.None;

            var parentDirectory = Directory.GetParent(folderPath);

            var parentFolder = parentDirectory == null ? Folder.None : ConvertToFolder(parentDirectory.ToString());
            return parentFolder;
        }

        private Folder ConvertToFolder(string folderPath)
        {
            return new Folder(Path.GetFileName(folderPath), folderPath);
        }

        private bool HasAccessToDirectory(string directoryPath)
        {
            try
            {
                Directory.GetDirectories(directoryPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
