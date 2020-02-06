using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappings
{
    internal class FuncionarioMapConfig : EntityTypeConfiguration<Funcionario>
    {
        public FuncionarioMapConfig()
        {
            this.ToTable("FUNCIONARIOS");
            this.Property(p => p.Nome).HasMaxLength(50);
            this.Property(p => p.Senha).HasMaxLength(50);
            this.Property(p => p.Telefone).IsFixedLength().HasMaxLength(11);
            this.Property(p => p.CPF).IsFixedLength().HasMaxLength(14);
            this.Property(p => p.Email).HasMaxLength(50);
        }
    }
}
