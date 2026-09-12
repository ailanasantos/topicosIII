using System.ComponentModel.DataAnnotations;

namespace ambiente_web.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Perfil")]
        [StringLength(30)]
        public string Perfil { get; set; } = "Operador";

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}