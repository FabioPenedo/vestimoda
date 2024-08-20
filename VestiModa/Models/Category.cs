using System.ComponentModel.DataAnnotations;

namespace VestiModa.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
        [Required(ErrorMessage = "Informe o nome da categoria")]
        [Display(Name = "Nome")]
        public string Name { get; set; } = string.Empty;


        [StringLength(200, ErrorMessage = "O tamanho máximo é 200 caracteres")]
        [Required(ErrorMessage = "Informe a descrição da categoria")]
        [Display(Name = "Descrição")]
        public string Description { get; set; } = string.Empty;


        public List<Product> Products { get; set;} = [];

    }
}
