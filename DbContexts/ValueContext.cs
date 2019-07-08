using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Models;

namespace TestApi.DbContexts
{
    public class ValueContext: DbContext
    {
        public ValueContext(DbContextOptions<ValueContext> options): base(options) { }

        public DbSet<Value> Values { get; set; }
    }
}
