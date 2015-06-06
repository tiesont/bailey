using System;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.MicroKernel;

namespace Bailey.Web.Mvc
{
    public class WindsorControllerFactory : System.Web.Mvc.DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return kernel.Resolve(controllerType) as IController;
        }
    }
}
