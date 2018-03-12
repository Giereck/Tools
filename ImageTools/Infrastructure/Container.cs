using System;
using Castle.Windsor;

namespace ImageTools.Infrastructure
{
    public static class Container
    {
        private static IWindsorContainer _container;

        public static void Initialize(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            _container = container;
        }

        public static T Resolve<T>()
        {
            if (_container == null)
            {
                throw new Exception("Container must be initialized before use.");
            }

            return _container.Resolve<T>();
        }
    }
}
