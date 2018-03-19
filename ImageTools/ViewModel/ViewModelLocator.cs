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
            container.Register(Component.For<MainWindowViewModel>());
            container.Register(Component.For<CompressImagesViewModel>());
            container.Register(Component.For<SelectFolderViewModel>().LifestyleTransient());
        }

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public CompressImagesViewModel CompressImagesViewModel => Container.Resolve<CompressImagesViewModel>();

        public SelectFolderViewModel SelectFolderViewModel => Container.Resolve<SelectFolderViewModel>();
    }
}