using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using Castle.Windsor;

namespace Bailey.Web.Http
{
    public class WindsorHttpDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        public WindsorHttpDependencyResolver(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        public object GetService(Type type)
        {
            return this.container.Kernel.HasComponent(type) ? this.container.Resolve(type) : null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return this.container.ResolveAll(type).Cast<object>().ToArray();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this.container);
        }

        public void Dispose()
        {
        }
    }
}
