using System;
using System.Web.Security;

using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Infrastructure.Abstract;
using MvcRefactorTest.Common;

namespace MvcRefactorTest.Infrastructure.Concrete
{
    public class CustomMembershipProvider : MembershipProvider, ICustomMembershipProvider
    {
        private readonly IUserRepository _userRepository;

        public CustomMembershipProvider(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Authenticate(string username, string password)
        {
            var result = this.ValidateUser(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }

            return result;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();

            // var success = false;

            // try
            // {
            // if (this._userRepository.ChangePassword(username, newPassword)) success = true;
            // }
            // catch (Exception ex)
            // {
            // LogFactory.GetLogger().Error(ex.Message, ex.InnerException);
            // }

            // return success;
        }

        public override bool ChangePasswordQuestionAndAnswer(
            string username, 
            string password, 
            string newPasswordQuestion, 
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(
            string username, 
            string password, 
            string email, 
            string passwordQuestion, 
            string passwordAnswer, 
            bool isApproved, 
            object providerUserKey, 
            out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var isValid = false;

            this._userRepository.ValidateUser(username, password, out isValid);

            return isValid;
        }
    }
}