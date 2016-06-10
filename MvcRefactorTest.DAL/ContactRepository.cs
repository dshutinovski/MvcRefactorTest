using System;
using System.Linq;

using log4net;

using MvcRefactorTest.Common;
using MvcRefactorTest.DAL.Interface;
using MvcRefactorTest.Domain;
using MvcRefactorTest.Domain.db;

namespace MvcRefactorTest.DAL
{
    [LoggingAspect]
    public class ContactRepository : IContactRepository
    {
        private dbContext _context;

        /// <summary>
        ///     Get Contact Details
        /// </summary>
        /// <returns>Returns true if success, else false</returns>
        public bool GetContactDetails(out Contact contactObj)
        {
            var succeed = false;
            contactObj = null;

            using (this._context = new dbContext())
            {
                contactObj = this._context.Contact.FirstOrDefault();
                succeed = true;
            }

            return succeed;
        }
    }
}