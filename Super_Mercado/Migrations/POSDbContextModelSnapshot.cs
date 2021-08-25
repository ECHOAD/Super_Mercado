﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Super_Mercado.Data;

namespace Super_Mercado.Migrations
{
    [DbContext(typeof(POSDbContext))]
    partial class POSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entidades.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Entidades.ImagenWebPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ImagenesWebPages");
                });

            modelBuilder.Entity("Entidades.Orden_Detalle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("Id_Producto")
                        .HasColumnType("int");

                    b.Property<int>("Id_orden")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id_Producto");

                    b.HasIndex("Id_orden");

                    b.ToTable("Ordenes_Detalles");
                });

            modelBuilder.Entity("Entidades.Ordenes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Usuario")
                        .HasColumnType("int");

                    b.Property<double?>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Id_Usuario");

                    b.ToTable("Ordenes");
                });

            modelBuilder.Entity("Entidades.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Categoria")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id_Categoria");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Entidades.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Expiry_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Usuario")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Id_Usuario");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Entidades.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Role_Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entidades.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Cedula")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("Id_Rol")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Telefono")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("User")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("Id_Rol");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Entidades.UsuarioDireccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Usuario")
                        .HasColumnType("int");

                    b.Property<decimal>("Latitud")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Longitud")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Id_Usuario");

                    b.ToTable("UsuariosDirecciones");
                });

            modelBuilder.Entity("Entidades.Orden_Detalle", b =>
                {
                    b.HasOne("Entidades.Producto", "Productos")
                        .WithMany()
                        .HasForeignKey("Id_Producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Ordenes", "Ordenes")
                        .WithMany()
                        .HasForeignKey("Id_orden")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ordenes");

                    b.Navigation("Productos");
                });

            modelBuilder.Entity("Entidades.Ordenes", b =>
                {
                    b.HasOne("Entidades.Usuario", "Usuarios")
                        .WithMany()
                        .HasForeignKey("Id_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Entidades.Producto", b =>
                {
                    b.HasOne("Entidades.Categoria", "Categorias")
                        .WithMany()
                        .HasForeignKey("Id_Categoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorias");
                });

            modelBuilder.Entity("Entidades.RefreshToken", b =>
                {
                    b.HasOne("Entidades.Usuario", "Usuario")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("Id_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Entidades.Usuario", b =>
                {
                    b.HasOne("Entidades.Role", "Roles")
                        .WithMany()
                        .HasForeignKey("Id_Rol");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Entidades.UsuarioDireccion", b =>
                {
                    b.HasOne("Entidades.Usuario", "Usuarios")
                        .WithMany()
                        .HasForeignKey("Id_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Entidades.Usuario", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
