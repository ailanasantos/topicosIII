using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ambiente_web.Models
{
    public class Venda
    {
        public int Id { get; set; }

        [Display(Name = "Data da Venda")]
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        [Display(Name = "Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Display(Name = "Forma de Pagamento")]
        [StringLength(50)]
        public string FormaPagamento { get; set; } = "Dinheiro";

        [Display(Name = "Status")]
        [StringLength(30)]
        public string Status { get; set; } = "Concluída";

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        public ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}