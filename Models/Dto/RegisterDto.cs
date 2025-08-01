using System.ComponentModel.DataAnnotations;

namespace fileuploadweb.Models.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string email { get; set; } = default!;
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string fullName { get; set; } = default!;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe de tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string password { get; set; } = default!;

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden")]
        public string confirmPassword { get; set; } = default!;
    }
}