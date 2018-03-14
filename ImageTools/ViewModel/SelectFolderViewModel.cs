using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImageTools.Infrastructure;
using ImageTools.Model;
using ImageTools.Utilities;

namespace ImageTools.ViewModel
{
    public class SelectFolderViewModel : ViewModelBase
    {
        private readonly IBreadcrumbGenerator _breadcrumbGenerator;
        private readonly IFolderManager _folderManager;
        private Folder _selectedFolder;

        public SelectFolderViewModel(IBreadcrumbGenerator breadcrumbGenerator, IFolderManager folderManager)
        {
            if (breadcrumbGenerator == null) throw new ArgumentNullException(nameof(breadcrumbGenerator));
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));

            _breadcrumbGenerator = breadcrumbGenerator;
            _folderManager = folderManager;

            DrillDownFolderCommand = new RelayCommand<Folder>(DrillDownFolderExecuted);
            NavigateBackCommand = new RelayCommand(NavigateBackExecuted, NavigateBackCanExecute);
            NavigateToBreadcrumbCommand = new RelayCommand<Folder>(NavigateToBreadcrumbExecuted);

            Breadcrumbs = new ObservableCollection<Folder>();
            Folders = new ObservableCollection<Folder>();
            SelectedFolder = Folder.Default;
        }
        
        public ICommand DrillDownFolderCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand NavigateToBreadcrumbCommand { get; }
        
        public Folder SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                if (Set(ref _selectedFolder, value))
                {
                    LoadFolders(_selectedFolder.Path);
                    LoadBreadcrumbs(_selectedFolder);
                }
            }
        }

        public ObservableCollection<Folder> Breadcrumbs { get; } 

        public ObservableCollection<Folder> Folders { get; }

        private void LoadFolders(string parentFolderPath)
        {
            Folders.Clear();
            foreach (var subFolder in _folderManager.GetSubFolders(parentFolderPath))
            {
                Folders.Add(subFolder);
            }
        }

        private void LoadBreadcrumbs(Folder selectedFolder)
        {
            Breadcrumbs.Clear();
            Breadcrumbs.AddRange(_breadcrumbGenerator.FolderPathToBreadcrumbs(selectedFolder.Path));
        }

        private void DrillDownFolderExecuted(Folder folder)
        {
            if (Directory.Exists(folder.Path))
            {
                SelectedFolder = folder;
            }
        }

        private bool NavigateBackCanExecute()
        {
            return SelectedFolder != null && !string.IsNullOrEmpty(SelectedFolder.ParentFolder.Path);
        }

        private void NavigateBackExecuted()
        {
            SelectedFolder = SelectedFolder.ParentFolder;
        }

        private void NavigateToBreadcrumbExecuted(Folder folder)
        {
            SelectedFolder = folder;
        }
    }
}
