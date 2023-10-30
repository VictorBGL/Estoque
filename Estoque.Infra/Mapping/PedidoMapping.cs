using Estoque.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infra.Mapping
{
    public class PedidoCompraMapping : IEntityTypeConfiguration<PedidoCompra>
    {
        public void Configure(EntityTypeBuilder<PedidoCompra> builder)
        {
            builder.ToTable("PedidoCompra");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PedidoCompraId");

            builder.HasOne(p => p.Fornecedor)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(p => p.FornecedorId);

            builder.HasOne(p => p.Comprador)
                .WithMany(p => p.Compras)
                .HasForeignKey(p => p.CompradorId);
        }
    }

    public class PedidoVendaMapping : IEntityTypeConfiguration<PedidoVenda>
    {
        public void Configure(EntityTypeBuilder<PedidoVenda> builder)
        {
            builder.ToTable("PedidoVenda");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PedidoVendaId");

            builder.HasOne(p => p.Vendedor)
                .WithMany(p => p.Vendas)
                .HasForeignKey(p => p.VendedorId);
        }
    }

    public class PedidoVendaProdutoMapping : IEntityTypeConfiguration<PedidoVendaProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoVendaProduto> builder)
        {
            builder.ToTable("PedidoVendaProduto");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PedidoVendaProdutoId");

            builder.HasOne(p => p.Produto)
                .WithMany(p => p.PedidosVenda)
                .HasForeignKey(p => p.ProdutoId);

            builder.HasOne(p => p.PedidoVenda)
               .WithMany(p => p.Produtos)
               .HasForeignKey(p => p.PedidoVendaId);
        }
    }

    public class PedidoCompraProdutoMapping : IEntityTypeConfiguration<PedidoCompraProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraProduto> builder)
        {
            builder.ToTable("PedidoCompraProduto");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("PedidoCompraProdutoId");

            builder.HasOne(p => p.Produto)
                .WithMany(p => p.PedidosCompra)
                .HasForeignKey(p => p.ProdutoId);

            builder.HasOne(p => p.PedidoCompra)
               .WithMany(p => p.Produtos)
               .HasForeignKey(p => p.PedidoCompraId);
        }
    }
}