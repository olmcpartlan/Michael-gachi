using System.Collections.Generic;
using Dojodachi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Dojodachi.Models {
    public class DojodachiContext : DbContext {

        public DojodachiContext(DbContextOptions options) : base(options) {}

        public DbSet<Pet> Pets {get;set;}
        
    }
}