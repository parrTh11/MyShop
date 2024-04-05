using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class WishList : BaseEntity
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
