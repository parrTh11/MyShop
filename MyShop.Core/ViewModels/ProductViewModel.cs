using MyShop.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Core.ViewModels
{
    public class ProductViewModel : BaseEntity
    {

        [DisplayName("Product Name")]
        [StringLength(20)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        
        public string Image { get; set; }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
