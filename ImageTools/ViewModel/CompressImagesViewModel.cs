﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImageTools.Compressor;
using ImageTools.Infrastructure;
using ImageTools.Model;
using ImageTools.Renamer;
using ImageTools.Utilities;
using Cursors = System.Windows.Input.Cursors;

namespace ImageTools.ViewModel
{
    public class CompressImagesViewModel : ViewModelBase
    {
        private readonly IFolderManager _folderManager;
        private readonly IEquipmentDetector _equipmentDetector;
        private string _sourceFolder;
        private string _targetFolder;
        private long _selectedQuailty;
        private bool _shouldRenameFiles;
        private string _renameFormat;
        private int _numberOfImages;
        private int _numberOfCompressedImages;
        private bool _isCompressing;

        public CompressImagesViewModel(IFolderManager folderManager, IEquipmentDetector equipmentDetector)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));
            if (equipmentDetector == null) throw new ArgumentNullException(nameof(equipmentDetector));

            _folderManager = folderManager;
            _equipmentDetector = equipmentDetector;

            _numberOfImages = 0;
            _numberOfCompressedImages = 0;
            RenameFormat = "yyyyMMdd_hhmmss";

            SelectSourceFolderCommand = new RelayCommand(SelectSourceFolderExecute);
            SelectTargetFolderCommand = new RelayCommand(SelectTargetFolderExecute);
            CompressImagesCommand = new RelayCommand(CompressImagesExecute, CompressImagesCanExecute);

            DetectedSourceEquipmentList = new ObservableCollection<Equipment>();
        }

        public List<long> QualityValues => new List<long> {50, 60, 70, 80, 90, 100};

        public ICommand SelectSourceFolderCommand { get; }

        public ICommand SelectTargetFolderCommand { get; }

        public ICommand CompressImagesCommand { get; }

        public ObservableCollection<Equipment> DetectedSourceEquipmentList { get; }

        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                Set(ref _sourceFolder, value);

                if (!string.IsNullOrWhiteSpace(_sourceFolder) && Directory.Exists(_sourceFolder))
                {
                    AnalyzeSourceFolder();
                }
            }
        }

        public string TargetFolder
        {
            get { return _targetFolder; }
            set { Set(ref _targetFolder, value); }
        }

        public long SelectedQuality
        {
            get { return _selectedQuailty; }
            set { Set(ref _selectedQuailty, value); }
        }

        public bool ShouldRenameFiles
        {
            get { return _shouldRenameFiles; }
            set { Set(ref _shouldRenameFiles, value); }
        }

        public string RenameFormat
        {
            get { return _renameFormat; }
            set { Set(ref _renameFormat, value); }
        }

        public int NumberOfImages
        {
            get { return _numberOfImages; }
            set { Set(ref _numberOfImages, value); }
        }

        public int NumberOfCompressedImages
        {
            get { return _numberOfCompressedImages; }
            set { Set(ref _numberOfCompressedImages, value); }
        }

        public bool IsCompressing
        {
            get { return _isCompressing; }
            set { Set(ref _isCompressing, value); }
        }

        private void SelectSourceFolderExecute()
        {
            var selectedFolder = GetFolder(string.IsNullOrEmpty(SourceFolder) ? TargetFolder : SourceFolder);

            if (!string.IsNullOrEmpty(selectedFolder))
            {
                SourceFolder = selectedFolder;
            }
        }

        private void SelectTargetFolderExecute()
        {
            var selectedFolder = GetFolder(string.IsNullOrEmpty(TargetFolder) ? SourceFolder : TargetFolder);

            if (!string.IsNullOrEmpty(selectedFolder))
            {
                TargetFolder = selectedFolder;
            }
        }

        private string GetFolder(string startPath)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = startPath;
                DialogResult result = dialog.ShowDialog();
                return result == DialogResult.OK ? dialog.SelectedPath : string.Empty;
            }
        }

        private bool CompressImagesCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SourceFolder))
                return false;

            if (string.IsNullOrWhiteSpace(TargetFolder))
                return false;

            if (SelectedQuality == 0)
                return false;

            return true;
        }

        private void CompressImagesExecute()
        {
            NumberOfCompressedImages = 0;

            var filesPaths = _folderManager.GetJpgFilesFromFolder(SourceFolder);

            try
            {
                Mouse.SetCursor(Cursors.Wait);
                IsCompressing = true;
                CompressImagesAsync(filesPaths);
            }
            finally
            {
                Mouse.SetCursor(Cursors.Arrow);
                IsCompressing = false;
            }
        }

        private async Task CompressImagesAsync(IList<string> filesPaths)
        {
            await CompressImages(filesPaths);
        }

        private Task CompressImages(IList<string> filesPaths)
        {
            Task task = Task.Factory.StartNew(
                () =>
                {
                    ITargetFileNameGenerator filePathGenerator;
                    if (ShouldRenameFiles)
                    {
                        var imageOptions = new ImageOptions();
                        imageOptions.FileFormat = RenameFormat;
                        imageOptions.EquipmentList.AddRange(DetectedSourceEquipmentList);
                        var formatTargetFileNameGenerator = Container.Resolve<IFormatTargetFileNameGenerator>();
                        formatTargetFileNameGenerator.Options = imageOptions;
                        filePathGenerator = formatTargetFileNameGenerator;
                    }
                    else
                    {
                        filePathGenerator = Container.Resolve<IImitatingTargetFileNameGenerator>();
                    }

                    foreach (string filePath in filesPaths)
                    {
                        if (CompressImage(filePath, filePathGenerator))
                        {
                            NumberOfCompressedImages++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }).ContinueWith(
                    t =>
                    {
                        if (t.Exception != null)
                        {
                            MessageBox.Show(t.Exception.ToString());
                        }
                    });
            return task;
        }

        private bool CompressImage(string filePath, ITargetFileNameGenerator filePathGenerator)
        {
            bool success;

            using (var converter = new JpgCompressor(SelectedQuality))
            {
                try
                {
                    converter.Compress(filePath, filePathGenerator.GetTargetFilePath(filePath, TargetFolder));
                    success = true;
                }
                catch (Exception ex)
                {
                    var message =
                        $"An error occured while trying to compress image: '{filePath}'. The reason was: '{ex.Message}'";
                    MessageBox.Show(message, "Image tools", MessageBoxButtons.OK);
                    success = false;
                }
            }

            return success;
        }
        
        private void AnalyzeSourceFolder()
        {
            var jpgFilePaths = _folderManager.GetJpgFilesFromFolder(_sourceFolder);
            NumberOfImages = jpgFilePaths.Count;

            DetectEquipment();
        }

        private void DetectEquipment()
        {
            DetectedSourceEquipmentList.Clear();
            DetectedSourceEquipmentList.AddRange(_equipmentDetector.DetectEquipment(_sourceFolder));
        }
    }
}
