using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ImageTools.Infrastructure;

namespace ImageTools.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
        }

        public ViewModelLocator(IWindsorContainer container)
        {
            container.Register(Component.For<MainViewModel>());
            container.Register(Component.For<CompressImagesViewModel>());
            container.Register(Component.For<SelectFolderViewModel>());
        }

        public MainViewModel MainViewModel => Container.Resolve<MainViewModel>();

        public CompressImagesViewModel CompressImagesViewModel => Container.Resolve<CompressImagesViewModel>();

        public SelectFolderViewModel SelectFolderViewModel => Container.Resolve<SelectFolderViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}