// TC3Context.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TC3.Models
{
    public partial class TC3Context : DbContext
    {
        public TC3Context()
        {
        }

        public TC3Context(DbContextOptions<TC3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CountryCode> CountryCodes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=(local);database=TC3;trusted_connection=true");
            }
            */
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<CountryCode>(entity =>
            {
                entity.HasKey(e => e.CountryCodeId)
                    .HasName("PK__Country___7AD449105DAED90D");

                entity.ToTable("Country_Codes", "TC3");

                entity.Property(e => e.CountryCodeId)
                    .HasColumnName("country_code_id")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__Customer__CD65CB85EDA0483B");

                entity.ToTable("Customers", "TC3");

                entity.HasIndex(e => e.Email)
                    .HasDatabaseName("UQ__Customer__AB6E61640D22A709")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Confirmation)
                    .HasColumnName("confirmation")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CountryCodeId)
                    .HasColumnName("country_code_id")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StateProvince)
                    .IsRequired()
                    .HasColumnName("state_province")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CountryCode)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CountryCodeId)
                    .HasConstraintName("FK__Customers__count__3A81B327");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.ProductTypeId)
                    .HasName("PK__Product___6EED3ED686314CCA");

                entity.ToTable("Product_Types", "TC3");

                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("product_name")
                    .IsUnique();

                entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Products__47027DF5874353D2");

                entity.ToTable("Products", "TC3");

                entity.HasIndex(e => e.ProductTypeId)
                    .HasDatabaseName("product_type");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(7, 2)");

                entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__produc__44FF419A");
            });

            modelBuilder.Entity<SalesOrderItem>(entity =>
            {
                entity.HasKey(e => e.SalesOrderItemId)
                    .HasName("PK__Sales_Or__607EC2BFEBA1C766");

                entity.ToTable("Sales_Order_Items", "TC3");

                entity.Property(e => e.SalesOrderItemId).HasColumnName("sales_order_item_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(7, 2)");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.SalesOrderId).HasColumnName("sales_order_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales_Ord__produ__4CA06362");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderItems)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sales_Ord__sales__4BAC3F29");
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(e => e.SalesOrderId)
                    .HasName("PK__Sales_Or__ED58649F35F123C1");

                entity.ToTable("Sales_Orders", "TC3");

                entity.Property(e => e.SalesOrderId).HasColumnName("sales_order_id");

                entity.Property(e => e.CardExpires)
                    .HasColumnName("card_expires")
                    .HasColumnType("date");

                entity.Property(e => e.CardNumber)
                    .HasColumnName("card_number")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CardAuthorized)
                    .HasColumnName("card_authorized")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Filled)
                    .HasColumnName("filled")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Sales_Ord__custo__47DBAE45");
            });
        }
    }
}
