using Microsoft.EntityFrameworkCore;
using ambiente_web.Models;

namespace ambiente_web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Substitua 'Produto' pelo nome da sua classe Model se ela tiver outro nome
        public DbSet<Produto> Produtos { get; set; }
    }
}