﻿using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocadoraDbContext : DbContext
    {
        public LocadoraDbContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LocadoraDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations
                .AddFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Properties().Where(c => c.PropertyType == typeof(string))
                .Configure(c => c.IsRequired().IsUnicode(false));

            modelBuilder.Properties().Where(c => c.PropertyType == typeof(DateTime))
                .Configure(c => c.HasColumnType("datetime2"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
