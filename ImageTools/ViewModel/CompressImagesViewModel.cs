using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ImageTools.Compressor;
using ImageTools.Infrastructure;
using ImageTools.Infrastructure.Messages;
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
        private int _numberOfProcessedFiles;
        private bool _isCompressing;

        public CompressImagesViewModel(IFolderManager folderManager, IEquipmentDetector equipmentDetector, IMessenger messenger)
        {
            if (folderManager == null) throw new ArgumentNullException(nameof(folderManager));
            if (equipmentDetector == null) throw new ArgumentNullException(nameof(equipmentDetector));

            _folderManager = folderManager;
            _equipmentDetector = equipmentDetector;

            _numberOfImages = 0;
            _numberOfProcessedFiles = 0;
            RenameFormat = "yyyyMMdd_hhmmss";

            SelectSourceFolderCommand = new RelayCommand(SelectSourceFolderExecute);
            SelectTargetFolderCommand = new RelayCommand(SelectTargetFolderExecute);
            CompressImagesCommand = new RelayCommand(CompressImagesExecute, CompressImagesCanExecute);

            DetectedSourceEquipmentList = new ObservableCollection<Equipment>();

            messenger.Register<FolderSelectedMessage>(this, FolderSelectedMessageHandler);
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

        public int NumberOfProcessedFiles
        {
            get { return _numberOfProcessedFiles; }
            set { Set(ref _numberOfProcessedFiles, value); }
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
            NumberOfProcessedFiles = 0;

            var filePaths = _folderManager.GetMediaFilesFromFolder(SourceFolder);

            try
            {
                Mouse.SetCursor(Cursors.Wait);
                IsCompressing = true;
                CompressImagesAsync(filePaths);
            }
            finally
            {
                Mouse.SetCursor(Cursors.Arrow);
                IsCompressing = false;
            }
        }

        private async Task CompressImagesAsync(IList<string> filePaths)
        {
            await CompressImages(filePaths);
        }

        private Task CompressImages(IList<string> filePaths)
        {
            Task task = Task.Factory.StartNew(
                () =>
                {
                    var filePathGenerator = GetFilePathGenerator();
                    
                    foreach (string filePath in filePaths)
                    {
                        var targetFilePath = filePathGenerator.GetTargetFilePath(filePath, TargetFolder);

                        if (ShouldCompressImage(filePath))
                        {
                            try
                            {
                                using (var imageCompressor = new JpgCompressor(SelectedQuality))
                                {
                                    imageCompressor.Compress(filePath, targetFilePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                var message = $"An error occurred while trying to compress image: '{filePath}'. The reason was: '{ex.Message}'";
                                MessageBox.Show(message, "Image tools", MessageBoxButtons.OK);
                                break;
                            }
                        }
                        else
                        {
                            File.Copy(filePath, targetFilePath);
                        }

                        NumberOfProcessedFiles++;
                        
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

        private bool ShouldCompressImage(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }

            return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase);
        }

        private IFileNameGenerator GetFilePathGenerator()
        {
            IFileNameGenerator filePathGenerator;

            if (ShouldRenameFiles)
            {
                var imageOptions = new ImageOptions();
                imageOptions.FileFormat = RenameFormat;
                imageOptions.EquipmentList.AddRange(DetectedSourceEquipmentList);
                var formatTargetFileNameGenerator = Container.Resolve<IFormatFileNameGenerator>();
                formatTargetFileNameGenerator.Options = imageOptions;
                filePathGenerator = formatTargetFileNameGenerator;
            }
            else
            {
                filePathGenerator = Container.Resolve<IImitatingFileNameGenerator>();
            }

            return filePathGenerator;
        }

        private void AnalyzeSourceFolder()
        {
            var jpgFilePaths = _folderManager.GetMediaFilesFromFolder(_sourceFolder);
            NumberOfImages = jpgFilePaths.Count;

            DetectEquipment();
        }

        private void DetectEquipment()
        {
            DetectedSourceEquipmentList.Clear();
            DetectedSourceEquipmentList.AddRange(_equipmentDetector.DetectEquipment(_sourceFolder));
        }

        private void FolderSelectedMessageHandler(FolderSelectedMessage message)
        {
            switch (message.FolderType)
            {
                case FolderType.Source:
                    SourceFolder = message.FolderPath;
                    break;
                case FolderType.Target:
                    TargetFolder = message.FolderPath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
