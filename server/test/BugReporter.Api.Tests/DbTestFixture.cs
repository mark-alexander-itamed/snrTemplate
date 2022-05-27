using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BugReporter.Api.Tests
{
    public abstract class DbTestFixture
    {
        private readonly string _databaseId = Guid.NewGuid().ToString();

        private BugReporterDbContext NewDb()
        {
            var opt = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(_databaseId)
                .Options;

            return new BugReporterDbContext(opt);
        }

        private readonly Lazy<BugReporterDbContext> _lazyDb;
        public BugReporterDbContext DbContext => _lazyDb.Value;
        public BugReporterDbContext CleanDbContext => NewDb();

        public DbTestFixture()
        {
            _lazyDb = new Lazy<BugReporterDbContext>(NewDb);
        }

        public void AddEntity<T>(T ent) where T : class
        {
            DbContext.Set<T>().Add(ent);
            DbContext.SaveChanges();
        }

        public void AddEntities<T>(IEnumerable<T> entities) where T : class
        {
            DbContext.Set<T>().AddRange(entities);
            DbContext.SaveChanges();
        }

        public void AddMany<T>(params T[] entities) where T : class => AddEntities(entities);
    }
}