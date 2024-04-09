using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Contracts
{
    public interface ICustomerService
    {
        string GetUserId(string userEmail);
    }
}
