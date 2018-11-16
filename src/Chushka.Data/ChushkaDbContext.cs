﻿using Chushka.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chushka.Data
{
    public class ChushkaDbContext : IdentityDbContext<ChushkaUser>
    {
        public ChushkaDbContext(DbContextOptions<ChushkaDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}