﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ImageTools.Infrastructure;
using ImageTools.Infrastructure.Messages;
using ImageTools.Model;
using ImageTools.Utilities;

namespace ImageTools.ViewModel
{
    public class SelectFolderViewModel : ViewModelBase
    {
        private readonly IBreadcrumbGenerator _breadcrumbGenerator;
        private readonly IFolderManager _folderManager;
        private readonly IMessenger _messenger;
        private Folder _currentFolder;
        private Folder _selectedFolder;

        public SelectFolderViewModel(IBreadcrumbGenerator breadcrumbGenerator, IFolderManager folderManager, IMessenger messenger)
        {
            _breadcrumbGenerator = breadcrumbGenerator ?? throw new ArgumentNullException(nameof(breadcrumbGenerator));
            _folderManager = folderManager ?? throw new ArgumentNullException(nameof(folderManager));
            _messenger = messenger;

            DrillDownFolderCommand = new RelayCommand<Folder>(DrillDownFolderExecuted);
            NavigateBackCommand = new RelayCommand(NavigateBackExecuted, NavigateBackCanExecute);
            NavigateToBreadcrumbCommand = new RelayCommand<Folder>(NavigateToBreadcrumbExecuted);
            SetSelectedFolderCommand = new RelayCommand<Folder>(SetSelectedFolderExecuted);
            DeselectFolderCommand = new RelayCommand(DeselectFolderExecuted);
            UseFolderCommand = new RelayCommand(UseFolderExecuted);

            Breadcrumbs = new ObservableCollection<Folder>();
            Folders = new ObservableCollection<Folder>();
            CurrentFolder = Folder.Default;
        }
        
        public ICommand DrillDownFolderCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand NavigateToBreadcrumbCommand { get; }

        public ICommand SetSelectedFolderCommand { get; }

        public ICommand DeselectFolderCommand { get; }
        
        public ICommand UseFolderCommand { get; }

        public FolderType CurrentFolderType { get; set; }

        public Folder CurrentFolder
        {
            get => _currentFolder;
            set
            {
                if (Set(ref _currentFolder, value))
                {
                    LoadFolders(_currentFolder.Path);
                    LoadBreadcrumbs(_currentFolder);
                }
            }
        }
        
        public Folder SelectedFolder
        {
            get => _selectedFolder;
            set => Set(ref _selectedFolder, value);
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
                CurrentFolder = folder;
            }
        }

        private bool NavigateBackCanExecute()
        {
            return CurrentFolder != null && CurrentFolder.Name != "My computer";
        }

        private void NavigateBackExecuted()
        {
            CurrentFolder = CurrentFolder.ParentFolder;
        }

        private void NavigateToBreadcrumbExecuted(Folder folder)
        {
            CurrentFolder = folder;
        }

        private void SetSelectedFolderExecuted(Folder folder)
        {
            SelectedFolder = folder;
        }

        private void DeselectFolderExecuted()
        {
            var selectedFolder = Folders.FirstOrDefault(f => f.IsSelected);

            if (selectedFolder != null)
            {
                selectedFolder.IsSelected = false;
            }

            SelectedFolder = null;
        }

        private void UseFolderExecuted()
        {
            var selectedFolder = Folders.FirstOrDefault(f => f.IsSelected);
            SelectedFolder = selectedFolder ?? CurrentFolder;

            SendFolderSelectedMessage();
        }

        private void SendFolderSelectedMessage()
        {
            _messenger.Send(new FolderSelectedMessage(CurrentFolderType, SelectedFolder.Path));
        }
    }
}
