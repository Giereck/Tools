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
                breadcrumbs.AddRange(GenerateBreadcrumbsFromFolderNames(folderNames));
            }

            return breadcrumbs;
        }

        private IEnumerable<Folder> GenerateBreadcrumbsFromFolderNames(string[] folderNames)
        {
            var breadcrumbs = new List<Folder>();

            for (int i = 1; i <= folderNames.Length; i++)
            {
                var batch = folderNames.Take(i).ToList();
                var aggregateResult = batch.Aggregate((a, b) => a + "\\" + b);

                breadcrumbs.Add(new Folder(batch.Last(), aggregateResult));
            }

            return breadcrumbs;
        }
    }
}
