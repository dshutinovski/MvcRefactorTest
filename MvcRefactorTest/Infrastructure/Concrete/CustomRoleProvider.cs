using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

using MvcRefactorTest.BL;
using MvcRefactorTest.BL.Interface;
using MvcRefactorTest.DAL;
using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Domain;
using MvcRefactorTest.Common;

namespace MvcRefactorTest.Infrastructure.Concrete
{
    using MvcRefactorTest.Domain.db;

    using Ninject;
    using Ninject.Parameters;

    public class CustomRoleProvider : RoleProvider
    {
        private readonly IUserRepository _userRepository;

        private readonly IUserService _userService;

        /// <summary>
        ///     Initialize Constructor
        /// </summary>
        public CustomRoleProvider()
        {
            IKernel kernel = new StandardKernel();
            var samurai =
                this._userRepository =
                kernel.Get<UserRepository>(new ConstructorArgument("context", kernel.Get<dbContext>()));
            this._userService = kernel.Get<UserService>(new ConstructorArgument("userRepository", _userRepository));
        }

        /// <summary>
        ///     Application Name
        /// </summary>
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Find User in Role.
        /// </summary>
        /// <param name="roleName">Role Name.</param>
        /// <param name="usernameToMatch">User name to be mached.</param>
        /// <returns>Find all of Users in the role.</returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            IList<User> userList;
            if (this._userService.GetAllUsers(out userList))
            {
                if (userList != null)
                {
                    return userList.Where(p => p.Role == roleName).Select(p => p.Name).ToArray();
                }
            }

            return new string[] { };
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Get roles for User.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>Get roles for user as array of strings.</returns>
        public override string[] GetRolesForUser(string username)
        {
            User userObj;
            if (this._userService.GetUserBy(username, out userObj))
            {
                if (userObj != null)
                {
                    return userObj.Role != null ? new[] { userObj.Role } : new string[] { };
                }
            }

            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Is User In Role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns>Returns true if exists, else false</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            User userObj;
            if (this._userService.GetUserBy(username, out userObj))
            {
                if (userObj != null)
                {
                    return userObj.Role != null && userObj.Role == roleName;
                }
            }

            return false;
        }

        /// <summary>
        ///     Remove Users from Roles.
        /// </summary>
        /// <param name="usernames">User names to be removed.</param>
        /// <param name="roleNames">Remove users from these roles</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();

            // try
            // {
            // foreach (var user in usernames)
            // {
            // User userObj;
            // if (!this._userService.GetUserBy(user, out userObj)) continue;
            // foreach (var role in roleNames)
            // {
            // if (userObj == null) continue;
            // userObj.Role = string.Empty;
            // this._userService.RemoveUserFromRole(user, role, out userObj);
            // }
            // }
            // }
            // catch (Exception ex)
            // {
            // LogFactory.GetLogger().Error(ex.Message, ex.InnerException);
            // }
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}