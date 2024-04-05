using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class ProductCategoryViewModel : BaseEntity
    {
        [Required]
        public string Category { get; set; }
    }
}
