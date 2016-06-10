using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using MvcRefactorTest.BL;
using MvcRefactorTest.DAL;
using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Domain;

using NUnit.Framework;

namespace MvcRefactorTest.NUnitTests.BL
{
    [TestFixture]
    public class ContactServiceFixture
    {
        private Contact _contact;

        private ContactService _contactService;

        private Mock<IContactRepository> _mockContactRepository;

        [TestFixtureSetUp]
        public void InitializeTests()
        {
            _contact = new Contact
                           {
                               Address = "Greater London",
                               DateCreated = DateTime.Now,
                               GeneralEmail = "mytestAccount@test.com",
                               IsDeleted = false,
                               id = 1,
                               MainPhone = "+38825498755"
                           };

            _mockContactRepository = new Mock<IContactRepository>();
        }

        [TestFixtureTearDown]
        public void TearDownObjects()
        {
            _contact = null;
            _contactService = null;
            _mockContactRepository = null;
        }

        [Test]
        public void WhenMethodGetContactDetailsIsCalledReturnContact()
        {
            _mockContactRepository.Setup(c => c.GetContactDetails(out _contact)).Returns(true);

            _contactService = new ContactService(_mockContactRepository.Object);

            Contact contactObject;
            bool success = _contactService.GetContactDetails(out contactObject);

            Assert.That(true, Is.EqualTo(success));
        }
    }
}