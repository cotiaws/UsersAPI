using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Infra.Data.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            //nome da tabela do banco de dados
            builder.ToTable("TB_ROLE");

            //chave primária
            builder.HasKey(r => r.Id);

            //mapeamento dos campos da tabela
            builder.Property(r => r.Id)
                .HasColumnName("ID"); //nome do campo
            
            builder.Property(r => r.Name)
                .HasColumnName("NAME") //nome do campo
                .HasMaxLength(25) //tamanho máximo de caracteres
                .IsRequired(); //not null

            //definindo o campo 'Name' como unico na tabela
            builder.HasIndex(r => r.Name).IsUnique();
        }
    }
}
