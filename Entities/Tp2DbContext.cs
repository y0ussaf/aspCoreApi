using Entities.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Entities
{
    public class Tp2DbContext : DbContext
    {
        public Tp2DbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        

        }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
