using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using ImageTools.Renamer;
using ImageTools.Utilities;
using ImageTools.ViewModel;

namespace ImageTools.Infrastructure
{
    public class ContainerBootStrapper
    {
        public void RegisterContainer(IWindsorContainer container)
        {
            container.Register(Component.For<IMessenger>().Instance(Messenger.Default));
            container.Register(Component.For<ViewModelLocator>().Instance(new ViewModelLocator(container)));
            container.Register(Component.For<IBreadcrumbGenerator>().ImplementedBy<BreadcrumbGenerator>());
            container.Register(Component.For<IFolderManager>().ImplementedBy<FolderManager>());
            container.Register(Component.For<IEquipmentDetector>().ImplementedBy<EquipmentDetector>());
            container.Register(Component.For<IMetaDataExtractor>().ImplementedBy<MediaMetaDataExtrator>());
            container.Register(Component.For<IImageMetaDataExtractor>().ImplementedBy<ImageMetaDataExtractor>());
            container.Register(Component.For<IVideoMetaDataExtractor>().ImplementedBy<VideoMetaDataExtractor>());
            container.Register(Component.For<IFormatFileNameGenerator>().ImplementedBy<FormatFileNameGenerator>());
            container.Register(Component.For<IImitatingFileNameGenerator>().ImplementedBy<ImitatingFileNameGenerator>());
        }
    }
}
