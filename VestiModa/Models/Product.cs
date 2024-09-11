using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VestiModa.Models
{
    public class Product
    {
        public int ProductId { get; set; }



        [Required(ErrorMessage = "O nome do produto deve ser informado")]
        [Display(Name = "Nome")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2}")]
        public string Name { get; set; } = string.Empty;



        [Required(ErrorMessage = "A descrição do produto deve ser informada")]
        [Display(Name = "Descrição")]
        [MinLength(20, ErrorMessage = "A descrição deve ter no mínimo {1} caracteres")]
        [MaxLength(200, ErrorMessage = "A descrição pode exceder {1} caracteres")]
        public string Description { get; set; } = string.Empty;



        [Required(ErrorMessage = "Informe o preço do produto")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 999.99, ErrorMessage = "O preço deve estar entre 1 e 999,99")]
        public decimal Price { get; set; }



        [Required(ErrorMessage = "Informe a quantidade no estoque")]
        [Display(Name = "Estoque")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser um valor positivo.")]
        public int StockQuantity { get; set; } = 0;



        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

    }
}
