using System.Windows;
using Castle.Windsor;
using ImageTools.Infrastructure;

namespace ImageTools
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootStrapper = new ContainerBootStrapper();
            var container = new WindsorContainer();
            bootStrapper.RegisterContainer(container);
            Container.Initialize(container);
        }
    }
}
