using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Moq;

using MvcRefactorTest.DAL;
using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Domain;
using MvcRefactorTest.Domain.db;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace MvcRefactorTest.NUnitTests.DAL
{
    [TestFixture]
    public class UserRepositoryFixture
    {
        private Mock<dbContext> _dbContextMock;

        private Mock<IDbSet<User>> _dbSetMock;

        private bool _isValid;

        private IQueryable<User> _userList;

        private User _userObj;

        private UserRepository _userRepository;

        [TestFixtureSetUp]
        public void InitializeUnitTests()
        {
            // create some mock products to play with
            _isValid = true;

            _userObj = new User
                           {
                               Name = "Chris Smith",
                               Password = "pass",
                               Role = "Developer",
                               IsEnabled = true,
                               id = 2
                           };

            _userList =
                new List<User>
                    {
                        _userObj, 
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
                    }.AsQueryable();

            _dbContextMock = new Mock<dbContext>();

            _dbSetMock = new Mock<IDbSet<User>>();

            _dbSetMock.Setup(m => m.Provider).Returns(_userList.Provider);
            _dbSetMock.Setup(m => m.Expression).Returns(_userList.Expression);
            _dbSetMock.Setup(m => m.ElementType).Returns(_userList.ElementType);
            _dbSetMock.Setup(m => m.GetEnumerator()).Returns(_userList.GetEnumerator());
        }

        [TestFixtureTearDown]
        public void TearDownObjects()
        {
            _userList = null;
            _dbContextMock = null;
            _userObj = null;
            _dbContextMock = null;
        }

        [Test]
        public void WhenMethodGetAllActiveUsersIsCalledReturnUsers()
        {
            // return a user by Name
            _dbContextMock.Setup(x => x.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;

            IUserRepository userRepository = new UserRepository(target);
            IList<User> testUserList;
            var success = userRepository.GetAllUsersBy(true, out testUserList);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That(2, Is.EqualTo(testUserList.Count));
        }

        [Test]
        public void WhenMethodGetAllUersIsCalledReturnUsers()
        {
            // Return all users
            _dbContextMock.Setup(x => x.User).Returns(_dbSetMock.Object);

            // setup of our Mock User Repository
            dbContext target = _dbContextMock.Object;
            IList<User> testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.GetAllUsers(out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That(3, Is.EqualTo(testUser.Count));
            Assert.That(null, Is.Not.EqualTo(testUser));
            Assert.That(
                false,
                Is.EqualTo(testUser.Where(p => p.Name == "Awin George").Select(p => p.IsDeleted).First()));
        }

        [Test]
        public void WhenMethodGetUserByIdTestIsCalledReturnUser()
        {
            // Return a user by Id
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of our Mock User Repository
            var target = _dbContextMock.Object;
            User testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.GetUserBy(2, out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            Assert.That("Chris Smith", Is.EqualTo(testUser.Name));
            Assert.That("Richard Child", Is.Not.EqualTo(testUser.Name));
            Assert.That(2, Is.EqualTo(testUser.id));
        }

        [Test]
        [TestCase("Richard Child")]
        [TestCase("Chris Smith")]
        [TestCase("Awin George")]
        public void WhenMethodGetUserByNameTestIsCalledByUsernameReturnUser(string userName)
        {
            // return a user by Name
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            User testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.GetUserBy(userName, out testUser);

            // assert
            Assert.That(true, Is.EqualTo(success));
            if (userName == testUser.Name)
            {
                Assert.That(userName, Is.EqualTo(testUser.Name));
            }
            else
            {
                Assert.That(userName, Is.Not.EqualTo(testUser.Name));
            }
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void WhenMethodGetUserByIsCalledByNullOrEmptyUsernameReturnFalse(string userName)
        {
            // return a user by Name
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            User testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.GetUserBy(userName, out testUser);

            // assert
            Assert.That(false, Is.EqualTo(success));
        }

        [Test]
        [TestCase("Richard Child", "Richards New Password")]
        [TestCase("Chris Smith", "Sam Smith")]
        [TestCase("Awin George", "Yugo")]
        public void WhenMethodChangePasswordIsCalledByValidUsernameAndValidPasswordIsProvidedReturnTrue(string userName, string password)
        {
            // return a user by Name
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            User testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.ChangePassword(userName, password);

            // assert
            Assert.That(true, Is.EqualTo(success));
        }

        [Test]
        [Combinatorial]
        public void WhenMethodChangePasswordIsCalledByNullOrEmptyOrInvalidUsernameReturnFalse(
            [Values("John Pirce", "Edward Bolton", "", null)]string userName,
            [Values("Johns Password", "Boltons Password", null)]string password)
        {
            // return a user by Name
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            User testUser;
            _userRepository = new UserRepository(target);
            var success = _userRepository.ChangePassword(userName, password);

            // assert
            Assert.That(false, Is.EqualTo(success));
        }

        [Test]
        [Combinatorial]
        public void WhenUserNameAndPasswordIsProvidedValidateUserTest(
            [Values("Richard Child", "Chris Smith", "Awin George")] string userName,
            [Values("pass", "Test Password")] string password)
        {
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            bool isValid;
            _userRepository = new UserRepository(target);
            bool success = _userRepository.ValidateUser(userName, password, out isValid);

            // assert
            if (success)
            {
                Assert.That(true, Is.EqualTo(success));

                if (isValid) Assert.IsTrue(isValid);
                else Assert.IsFalse(isValid);
            }
            else Assert.That(false, Is.EqualTo(success));
        }

        [Test]
        [Combinatorial]
        public void WhenUserNameIsNulOrEmptyValidateUserTest(
            [Values("", null)] string userName,
            [Values("pass", "Test Password", "", null)] string password)
        {
            _dbContextMock.Setup(mockContext => mockContext.User).Returns(_dbSetMock.Object);

            // setup of Mock User Repository
            var target = _dbContextMock.Object;
            bool isValid;
            _userRepository = new UserRepository(target);
            bool success = _userRepository.ValidateUser(userName, password, out isValid);

            // assert
            Assert.That(false, Is.EqualTo(success));
        }
    }
}