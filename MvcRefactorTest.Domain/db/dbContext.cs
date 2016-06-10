using System.Data.Entity;

namespace MvcRefactorTest.Domain.db
{
    using System;

    public class dbContext : DbContext, IDisposable
    {
        public dbContext()
            : base("MvcRefactorTest")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual IDbSet<Contact> Contact { get; set; }

        public virtual IDbSet<Log> Log { get; set; }

        public virtual IDbSet<User> User { get; set; }
    }
}