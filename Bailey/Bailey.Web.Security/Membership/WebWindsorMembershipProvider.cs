#region license
// Copyright (c) 2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Web;

using Castle.Windsor;

namespace Bailey.Web
{
    public class WebWindsorMembershipProvider : WindsorMembershipProvider
    {
        public override IWindsorContainer GetContainer()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                throw new Exception("No HttpContext");
            }

            var accessor = context.ApplicationInstance as IContainerAccessor;
            if (accessor == null)
            {
                throw new Exception("The global HttpApplication instance needs to implement " + typeof(IContainerAccessor).FullName);
            }

            if (accessor.Container == null)
            {
                throw new Exception("HttpApplication has no container initialized");
            }

            return accessor.Container;
        }
    }
}