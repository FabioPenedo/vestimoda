using System.ComponentModel.DataAnnotations;

namespace VestiModa.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; } = string.Empty;



        [Required(ErrorMessage = "Informe o email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter com um email válido")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; } = string.Empty;



        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;
    }
}
