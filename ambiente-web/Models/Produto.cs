using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ambiente_web.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Preço é obrigatório")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Estoque é obrigatório")]
        [Display(Name = "Estoque")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque não pode ser negativo")]
        public int Estoque { get; set; }

        [Display(Name = "Estoque Mínimo")]
        public int EstoqueMinimo { get; set; } = 5;

        [Display(Name = "Código de Barras")]
        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}