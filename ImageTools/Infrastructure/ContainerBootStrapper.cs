using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ImageTools.Renamer;
using ImageTools.Utilities;
using ImageTools.ViewModel;

namespace ImageTools.Infrastructure
{
    public class ContainerBootStrapper
    {
        public void RegisterContainer(IWindsorContainer container)
        {
            container.Register(Component.For<ViewModelLocator>().Instance(new ViewModelLocator(container)));
            container.Register(Component.For<IFolderManager>().ImplementedBy<FolderManager>());
            container.Register(Component.For<IEquipmentDetector>().ImplementedBy<EquipmentDetector>());
            container.Register(Component.For<IImagePropertyExtractor>().ImplementedBy<ImagePropertyExtractor>());
            container.Register(Component.For<IFormatFileNameGenerator>().ImplementedBy<FormatFileNameGenerator>());
            container.Register(Component.For<IImitatingFileNameGenerator>().ImplementedBy<ImitatingFileNameGenerator>());
        }
    }
}
