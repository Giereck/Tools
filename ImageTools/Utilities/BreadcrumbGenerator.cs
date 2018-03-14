using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using ImageTools.Model;

namespace ImageTools.Utilities
{
    public interface IBreadcrumbGenerator
    {
        IEnumerable<Folder> FolderPathToBreadcrumbs(string folderPath);
    }

    public class BreadcrumbGenerator : IBreadcrumbGenerator
    {
        public IEnumerable<Folder> FolderPathToBreadcrumbs(string folderPath)
        {
            List<Folder> breadcrumbs = new EditableList<Folder>();

            if (string.IsNullOrEmpty(folderPath))
            {
                breadcrumbs.Add(Folder.Root);
            }
            else
            {
                var folderNames = folderPath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                
                for (int i = 0; i < folderNames.Length; i++)
                {
                    List<string> pathParts = new List<string>();

                    for (int j = 0; j <= i; j++)
                    {
                        pathParts.Add(folderNames[j]);
                    }

                    breadcrumbs.Add(new Folder(pathParts.Last(), string.Join("\\", pathParts)));
                }
            }

            return breadcrumbs;
        }
    }
}
