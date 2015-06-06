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
    public abstract class WindsorMembershipProvider : MembershipProvider
    {
        private string providerId;

        public abstract IWindsorContainer GetContainer();

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(providerId))
            {
                throw new Exception("Please configure the providerId for the membership provider " + name);
            }

            WithProvider(provider => provider.Initialize(name, config));
        }

        private MembershipProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().Resolve<MembershipProvider>(providerId, new Hashtable());
                if (provider == null)
                {
                    throw new Exception(string.Format("Component '{0}' does not inherit MembershipProvider", providerId));
                }
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving MembershipProvider " + providerId, e);
            }
        }

        private T WithProvider<T>(Func<MembershipProvider, T> f)
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

        private void WithProvider(Action<MembershipProvider> f)
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


        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var provider = GetProvider();
            try
            {
                return provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return WithProvider(provider => provider.ChangePasswordQuestionAndAnswer(username, password, newPasswordAnswer, newPasswordAnswer));
        }

        public override string GetPassword(string username, string answer)
        {
            return WithProvider(provider => provider.GetPassword(username, answer));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WithProvider(provider => provider.ChangePassword(username, oldPassword, newPassword));
        }

        public override string ResetPassword(string username, string answer)
        {
            return WithProvider(provider => provider.ResetPassword(username, answer));
        }

        public override void UpdateUser(MembershipUser user)
        {
            WithProvider(provider => provider.UpdateUser(user));
        }

        public override bool ValidateUser(string username, string password)
        {
            return WithProvider(provider => provider.ValidateUser(username, password));
        }

        public override bool UnlockUser(string userName)
        {
            return WithProvider(provider => provider.UnlockUser(userName));
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return WithProvider(provider => provider.GetUser(providerUserKey, userIsOnline));
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return WithProvider(provider => provider.GetUser(username, userIsOnline));
        }

        public override string GetUserNameByEmail(string email)
        {
            return WithProvider(provider => provider.GetUserNameByEmail(email));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return WithProvider(provider => provider.DeleteUser(username, deleteAllRelatedData));
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return WithProvider(provider => provider.GetNumberOfUsersOnline());
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return WithProvider(provider => provider.EnablePasswordRetrieval); }
        }

        public override bool EnablePasswordReset
        {
            get { return WithProvider(provider => provider.EnablePasswordReset); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return WithProvider(provider => provider.RequiresQuestionAndAnswer); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return WithProvider(provider => provider.MaxInvalidPasswordAttempts); }
        }

        public override int PasswordAttemptWindow
        {
            get { return WithProvider(provider => provider.PasswordAttemptWindow); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return WithProvider(provider => provider.RequiresUniqueEmail); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return WithProvider(provider => provider.PasswordFormat); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return WithProvider(provider => provider.MinRequiredPasswordLength); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return WithProvider(provider => provider.MinRequiredNonAlphanumericCharacters); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return WithProvider(provider => provider.PasswordStrengthRegularExpression); }
        }

    }
}
