using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ConexionContext : DbContext
    {
        public ConexionContext(DbContextOptions<ConexionContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Card> Card { get; set; }

    
    }
}
