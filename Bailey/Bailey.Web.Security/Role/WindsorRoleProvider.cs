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
using System.Collections;
using System.Collections.Specialized;
using System.Web.Security;

using Castle.Windsor;

namespace Bailey.Web
{
    public abstract class WindsorRoleProvider : RoleProvider
    {
        private string providerId;


        public override string ApplicationName { get; set; }


        public abstract IWindsorContainer GetContainer();

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(providerId))
            {
                throw new Exception("Please configure the providerId for the role provider " + name);
            }

            WithProvider(provider => provider.Initialize(name, config));
        }

        private RoleProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().Resolve<RoleProvider>(providerId, new Hashtable());
                if (provider == null)
                {
                    throw new Exception(string.Format("Component '{0}' does not inherit RoleProvider", providerId));
                }
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving RoleProvider " + providerId, e);
            }
        }

        private T WithProvider<T>(Func<RoleProvider, T> f)
        {
            var provider = GetProvider();
            try
            {
                return f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        private void WithProvider(Action<RoleProvider> f)
        {
            var provider = GetProvider();
            try
            {
                f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(provider => provider.AddUsersToRoles(usernames, roleNames));
        }

        public override void CreateRole(string roleName)
        {
            WithProvider(provider => provider.CreateRole(roleName));
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return WithProvider(provider => provider.DeleteRole(roleName, throwOnPopulatedRole));
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return WithProvider(provider => provider.FindUsersInRole(roleName, usernameToMatch));
        }

        public override string[] GetAllRoles()
        {
            return WithProvider(provider => provider.GetAllRoles());
        }

        public override string[] GetRolesForUser(string username)
        {
            return WithProvider(provider => provider.GetRolesForUser(username));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return WithProvider(provider => provider.GetUsersInRole(roleName));
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return WithProvider(provider => provider.IsUserInRole(username, roleName));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(provider => provider.RemoveUsersFromRoles(usernames, roleNames));
        }

        public override bool RoleExists(string roleName)
        {
            return WithProvider(provider => provider.RoleExists(roleName));
        }
    }
}
