using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public partial class PedidosContext : DbContext
{
    public PedidosContext()
    {
    }

    public PedidosContext(DbContextOptions<PedidosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClienteDataModel> Clientes { get; set; }

    public virtual DbSet<ItensPedidoDataModel> ItensPedidos { get; set; }

    public virtual DbSet<PedidoDataModel> Pedidos { get; set; }

    public virtual DbSet<ProdutoDataModel> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:PedidosConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClienteDataModel>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0871DEA05A7");

            entity.Property(e => e.ClienteId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nome).HasMaxLength(100);
        });

        modelBuilder.Entity<ItensPedidoDataModel>(entity =>
        {
            entity.HasKey(e => e.ItemPedidoId).HasName("PK__ItensPed__8433C88F3782C68F");

            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Pedido)
                .WithMany(p => p.ItensPedidos)
                .HasForeignKey(d => d.PedidoId)
                .IsRequired()
                .HasConstraintName("FK_ItensPedidos_Pedidos");

            entity.HasOne(d => d.Produto).WithMany(p => p.ItensPedidos)
                .HasForeignKey(d => d.ProdutoId)
                .HasConstraintName("FK_ItensPedidos_Produtos");
        });

        modelBuilder.Entity<PedidoDataModel>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedidos__09BA14308B3904DD");

            entity.Property(e => e.PedidoId).ValueGeneratedNever();
            entity.Property(e => e.DataPedido)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Imposto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");
        });

        modelBuilder.Entity<ProdutoDataModel>(entity =>
        {
            entity.HasKey(e => e.ProdutoId).HasName("PK__Produtos__9C8800E3F8076962");

            entity.Property(e => e.ProdutoId).ValueGeneratedNever();
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}