using System.ComponentModel.DataAnnotations;

namespace VasosInteligentes.Models
{
    public class User
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public string? Celular { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="E-mail inválido")]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
