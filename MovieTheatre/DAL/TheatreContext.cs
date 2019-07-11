﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MovieTheatre.Models;


namespace MovieTheatre.DAL
{
    public class Context : DbContext
    {

        public Context() : base("TheatreContext")
        {

        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<Context>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
