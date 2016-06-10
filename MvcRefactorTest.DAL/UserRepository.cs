namespace MvcRefactorTest.DAL
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using MvcRefactorTest.Common;
    using MvcRefactorTest.DAL.Interface;
    using MvcRefactorTest.Domain;
    using MvcRefactorTest.Domain.db;

    #endregion

    [LoggingAspect]
    public class UserRepository : IUserRepository
    {
        private readonly dbContext _context;

        public UserRepository(dbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Get All users
        /// </summary>
        /// <param name="userList">user List</param>
        /// <returns>Returns true if success, else false</returns>
        public bool GetAllUsers(out IList<User> userList)
        {
            var succeed = false;

            userList = this._context.User.ToList();
            succeed = true;

            return succeed;
        }

        /// <summary>
        ///     Get All users by active flag
        /// </summary>
        /// <param name="active">is Enabled</param>
        /// <param name="userList">user List</param>
        /// <returns>Returns true if success, else false</returns>
        public bool GetAllUsersBy(bool active, out IList<User> userList)
        {
            var succeed = false;

            userList = this._context.User.Where(p => p.IsEnabled == active).ToList();
            succeed = true;

            return succeed;
        }

        /// <summary>
        ///     Get user By Id.
        /// </summary>
        /// <param name="id">user Id.</param>
        /// <param name="userObj">Object to be retrieved.</param>
        /// <returns>Returns true if success, else false.</returns>
        public bool GetUserBy(int id, out User userObj)
        {
            var succeed = false;

            userObj = this._context.User.First(p => p.id == id);
            succeed = true;

            return succeed;
        }

        /// <summary>
        ///     Get user By Name
        /// </summary>
        /// <param name="name">user Name</param>
        /// <param name="userObj">Object to be retrieved</param>
        /// <returns>Returns true if success, else false</returns>
        public bool GetUserBy(string name, out User userObj)
        {
            var succeed = false;

            userObj = this._context.User.First(p => p.Name == name);
            succeed = true;

            return succeed;
        }

        // return succeed;
        // }
        /// <summary>
        ///     Validate User.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">User password.</param>
        /// <param name="isValid">Is user valid.</param>
        /// <returns>Returns true if success, else false.</returns>
        public bool ValidateUser(string userName, string password, out bool isValid)
        {
            return isValid = this._context.User.First(p => p.Name == userName && p.Password == password) != null ? true : false;
        }

        /// <summary>
        ///     Change Password.
        /// </summary>
        /// <param name="fullName">User full name.</param>
        /// <param name="password">User password.</param>
        /// <returns>Return true if success, else false.</returns>
        public bool ChangePassword(string fullName, string password)
        {
            var succeed = false;

            var userObj = this._context.User.First(p => p.Name == fullName);
            if (userObj != null)
            {
                userObj.Password = password;
            }

            _context.SaveChanges();

            return succeed = true;
        }
    }
}