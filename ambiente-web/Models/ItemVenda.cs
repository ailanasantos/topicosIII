using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ambiente_web.Models
{
    public class ItemVenda
    {
        public int Id { get; set; }

        [Display(Name = "Venda")]
        public int VendaId { get; set; }

        [ForeignKey("VendaId")]
        public Venda Venda { get; set; }

        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "Preço Unitário")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}