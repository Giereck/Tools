using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ImageTools.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CompressImagesViewModel>();
            SimpleIoc.Default.Register<ArrangeImagesViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CompressImagesViewModel CompressImagesViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CompressImagesViewModel>();
            }
        }

        public ArrangeImagesViewModel ArrangeImagesViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ArrangeImagesViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}