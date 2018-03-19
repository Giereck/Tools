using System;
using System.Windows;
using System.Windows.Controls;
using ImageTools.Model;
using ImageTools.ViewModel;

namespace ImageTools.View
{
    public partial class SelectFolderView : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(SelectFolderView), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FolderTypeProperty = DependencyProperty.Register(
            "FolderType", typeof(FolderType), typeof(SelectFolderView), new PropertyMetadata(default(FolderType), FolderTypeChanged));

        public SelectFolderView()
        {
            InitializeComponent();
            RadioButtonGroup = Guid.NewGuid().ToString();
        }

        public string RadioButtonGroup { get; }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public FolderType FolderType
        {
            get { return (FolderType) GetValue(FolderTypeProperty); }
            set { SetValue(FolderTypeProperty, value); }
        }

        private static void FolderTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((SelectFolderView)d).DataContext is SelectFolderViewModel viewModel)
            {
                viewModel.CurrentFolderType = (FolderType) e.NewValue;
            }
        }
    }
}
