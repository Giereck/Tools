﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImageTools.Compressor;
using Cursors = System.Windows.Input.Cursors;

namespace ImageTools.ViewModel
{
    public class CompressImagesViewModel : ViewModelBase
    {
        private string _sourceFolder;
        private string _targetFolder;
        private long _selectedQuailty;
        private int _numberOfImages;
        private int _numberOfCompressedImages;
        private bool _isCompressing;

        public CompressImagesViewModel()
        {
            _numberOfImages = 0;
            _numberOfCompressedImages = 0;
            
            SelectSourceFolderCommand = new RelayCommand(SelectSourceFolderExecute);    
            SelectTargetFolderCommand = new RelayCommand(SelectTargetFolderExecute);    
            CompressImagesCommand = new RelayCommand(CompressImagesExecute, CompressImagesCanExecute);
        }
        
        public List<long> QualityValues => new List<long> {50, 60, 70, 80, 90, 100};

        public ICommand SelectSourceFolderCommand { get; }

        public ICommand SelectTargetFolderCommand { get; }

        public ICommand CompressImagesCommand { get; }

        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                Set(ref _sourceFolder, value);

                if (!string.IsNullOrWhiteSpace(_sourceFolder) && Directory.Exists(_sourceFolder))
                {
                    NumberOfImages = GetJpgFilesFromFolder(_sourceFolder).Count;
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

            var filesPaths = GetJpgFilesFromFolder(SourceFolder);

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

        private async Task CompressImagesAsync(List<string> filesPaths)
        {
            await CompressImages(filesPaths);
        }

        private Task CompressImages(List<string> filesPaths)
        {
            Task task = Task.Factory.StartNew(
                () =>
                {
                    foreach (string filePath in filesPaths)
                    {
                        if (CompressImage(filePath))
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
        
        private bool CompressImage(string filePath)
        {
            bool success;

            string targetFilePath = Path.Combine(TargetFolder, Path.GetFileName(filePath));

            using (var converter = new JpgCompressor(SelectedQuality))
            {
                try
                {
                    converter.Compress(filePath, targetFilePath);
                    success = true;
                }
                catch (Exception ex)
                {
                    var message = $"An error occured while trying to compress image: '{filePath}'. The reason was: '{ex.Message}'";
                    MessageBox.Show(message, "Image tools", MessageBoxButtons.OK);
                    success = false;
                }
            }

            return success;
        }

        private List<string> GetJpgFilesFromFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath).Where(f => Path.GetExtension(f).Equals(".jpg", StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
