using Microsoft.EntityFrameworkCore;
using Stage_API.Data;
using System;

namespace Stage_API.Tests
{
    internal static class ContextHelper
    {
        internal static InternshipContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<InternshipContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new InternshipContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
    }
}
