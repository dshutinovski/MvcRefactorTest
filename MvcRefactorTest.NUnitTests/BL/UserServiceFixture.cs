using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using MvcRefactorTest.BL;
using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Domain;

using NUnit.Framework;

namespace MvcRefactorTest.NUnitTests.BL
{
    [TestFixture]
    public class UserServiceFixture
    {
        private Mock<IUserRepository> _mockUserRepository;

        private IList<User> _userList;

        private User _userObj;

        /// <summary>
        ///     Initialize unit tests
        /// </summary>
        /// <param name="userList">userList</param>
        /// <param name="userObj">userObj</param>
        /// <param name="mockUserRepository">UserRepository Mock</param>
        [TestFixtureSetUp]
        public void InitializeUnitTests()
        {
            // create some mock products to play with
            this._userObj = new User
                                {
                                    Name = "Chris Smith",
                                    id = 2,
                                    Password = "pass",
                                    Role = "Developer",
                                    IsEnabled = true
                                };

            this._userList = new List<User>
                                 {
                                     this._userObj, 
                                     new User
                                         {
                                             Name = "Awin George", 
                                             Password = "pass", 
                                             Role = "Developer", 
                                             IsEnabled = false, 
                                             id = 3
                                         }, 
                                     new User
                                         {
                                             Name = "Richard Child", 
                                             Password = "pass", 
                                             Role = "Developer", 
                                             IsEnabled = true, 
                                             id = 4
                                         }
                                 };

            this._mockUserRepository = new Mock<IUserRepository>();
        }

        [TestFixtureTearDown]
        public void TearDownObjects()
        {
            this._userList = null;
            this._userObj = null;
            this._mockUserRepository = null;
        }

        [Test]
        public void WhenMethodGetAllUsersIsCalledUersReturnUsersTest()
        {
            // Return all users
            this._mockUserRepository.Setup(mr => mr.GetAllUsers(out this._userList)).Returns(true);

            // setup of our Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);
            IList<User> testUser;
            var success = target.GetAllUsers(out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That(3, Is.EqualTo(testUser.Count));
            Assert.AreNotEqual(null, testUser);
            Assert.That(
                false,
                Is.EqualTo(testUser.Where(p => p.Name == "Awin George").Select(p => p.IsDeleted).SingleOrDefault()));
        }

        [Test]
        public void WhenMethodGetAllUsersByActiveFlagTrueIsCalledUersReturnUsersTest()
        {
            // Return all users
            this._mockUserRepository.Setup(mr => mr.GetAllUsersBy(true, out this._userList)).Returns(true);

            // setup of our Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);
            IList<User> testUser;
            var success = target.GetAllUsersBy(true, out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That(3, Is.EqualTo(testUser.Count));
            Assert.That(null, Is.Not.EqualTo(testUser));
            Assert.That(
                false,
                Is.EqualTo(testUser.Where(p => p.Name == "Awin George").Select(p => p.IsDeleted).SingleOrDefault()));
        }

        [Test]
        public void WhenMethodGetAllUsersByActiveFlagFalseIsCalledUersReturnUsersTest()
        {
            // Return all users
            this._mockUserRepository.Setup(mr => mr.GetAllUsersBy(false, out this._userList)).Returns(true);

            // setup of our Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);
            IList<User> testUser;
            var success = target.GetAllUsersBy(false, out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That(3, Is.EqualTo(testUser.Count));
            Assert.That(null, Is.Not.EqualTo(testUser));
            Assert.That(
                false,
                Is.EqualTo(testUser.Where(p => p.Name == "Awin George").Select(p => p.IsDeleted).SingleOrDefault()));
        }

        [Test]
        public void WhenMethodGetUserByIdIsCalledReturnUserTest()
        {
            // Return a user by Id
            this._mockUserRepository.Setup(mr => mr.GetUserBy(It.IsAny<int>(), out this._userObj)).Returns(true);

            // setup of our Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);
            User testUser;
            var success = target.GetUserBy(2, out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That("Chris Smith", Is.EqualTo(testUser.Name));
            Assert.That("Richard Child", Is.Not.EqualTo(testUser.Name));
            Assert.That(2, Is.EqualTo(testUser.id));
        }

        [Test]
        [Combinatorial]
        public void WhenMethodGetUserByNameIsCalledReturnUserTest(
            [Values("Richard Child", "Chris Smith", "Awin George", "", null)] string userName)
        {
            // return a user by Name
            this._mockUserRepository.Setup(mr => mr.GetUserBy(It.IsIn("Chris Smith"), out this._userObj)).Returns(true);

            // setup of Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);
            User testUser;
            var success = target.GetUserBy(userName, out testUser);

            // assert
            if (testUser != null && userName == testUser.Name)
            {
                Assert.That(userName, Is.EqualTo(testUser.Name));
                Assert.That(true, Is.EqualTo(success));
            }
            else
            {
                Assert.That(false, Is.EqualTo(success));
            }
        }

        [Test]
        [Combinatorial]
        public void WhenUserNameAndPasswordIsProvidedValidateUserTest(
            [Values("Richard Child", "Chris Smith", "Awin George", "", null)] string userName,
            [Values("pass", "Test Password", "", null)] string password)
        {
            // return valid user
            var Valid = true;
            this._userObj = this._userList.FirstOrDefault(p => p.Name == userName && p.Password == password);
            this._mockUserRepository.Setup(
                mr =>
                mr.ValidateUser(
                    It.IsIn(this._userObj != null ? this._userObj.Name : string.Empty),
                    It.IsIn(this._userObj != null ? this._userObj.Password : string.Empty),
                    out Valid)).Returns(true);

            // setup of Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);

            var success = target.ValidateUser(userName, password, out Valid);

            // assert
            if (success)
            {
                Assert.IsTrue(Valid);
            }
            else Assert.IsFalse(success);
        }

        [Test]
        [Combinatorial]
        public void WhenUserNameAndPasswordIsNullOrEmptyValidateUserTest(
            [Values("", null)] string userName,
            [Values("pass", "Test Password", "", null)] string password)
        {
            // return valid user
            var isValid = true;
            this._userObj = this._userList.FirstOrDefault(p => p.Name == userName && p.Password == password);
            this._mockUserRepository.Setup(
                mr =>
                mr.ValidateUser(
                    It.IsIn(this._userObj != null ? this._userObj.Name : string.Empty),
                    It.IsIn(this._userObj != null ? this._userObj.Password : string.Empty),
                    out isValid)).Returns(false);

            // setup of Mock User Repository
            var target = new UserService(this._mockUserRepository.Object);

            var success = target.ValidateUser(userName, password, out isValid);

            // assert
            Assert.IsFalse(success);
        }
    }
}