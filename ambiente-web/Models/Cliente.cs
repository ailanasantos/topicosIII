using System.ComponentModel.DataAnnotations;

namespace ambiente_web.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [StringLength(14)]
        public string? CPF { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(15)]
        public string? Telefone { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(200)]
        public string? Endereco { get; set; }

        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}