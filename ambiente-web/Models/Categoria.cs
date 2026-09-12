using System.ComponentModel.DataAnnotations;

namespace ambiente_web.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}