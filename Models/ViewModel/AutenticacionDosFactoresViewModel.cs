using System.ComponentModel.DataAnnotations;

namespace fileuploadweb.Models.ViewModel
{
    public class AutenticacionDosFactoresViewModel
    {
        // Para el acceso (login)
        [Required]
        [Display(Name = "Codigo del autenticador")]
        public string Code { get; set; } = string.Empty;
        // Para el registro
        public string Token { get; set; } = string.Empty;
    }
}