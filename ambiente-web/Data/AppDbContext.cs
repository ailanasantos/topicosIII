using Microsoft.EntityFrameworkCore;
using ambiente_web.Models;

namespace ambiente_web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(iv => iv.VendaId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoId);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.ClienteId);

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Bebidas", Descricao = "Sucos, refrigerantes, águas" },
                new Categoria { Id = 2, Nome = "Laticínios", Descricao = "Leite, queijo, manteiga" },
                new Categoria { Id = 3, Nome = "Padaria", Descricao = "Pães, bolos, biscoitos" },
                new Categoria { Id = 4, Nome = "Hortifruti", Descricao = "Frutas e verduras" },
                new Categoria { Id = 5, Nome = "Carnes", Descricao = "Carnes bovinas, suínas, aves" }
            );
        }
    }
}