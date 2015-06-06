using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace Bailey.Web.Http
{
    sealed class WindsorDependencyScope : IDependencyScope
    {
        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;

        public WindsorDependencyScope(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;
            _scope = container.BeginScope();
        }

        public object GetService(Type type)
        {
            return _container.Kernel.HasComponent(type) ? _container.Resolve(type) : null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return _container.ResolveAll(type).Cast<object>().ToArray();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
