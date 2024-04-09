using MyShop.Core;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class CustomerService : ICustomerService
    {
        IRepository<Customer> customerContext;
        public CustomerService(IRepository<Customer> customerContext)
        {
            this.customerContext = customerContext;
        }
        public string GetUserId(string userEmail)
        {
            var user = customerContext.Collection().FirstOrDefault(x => x.Email == userEmail);
            return (user == null) ? "" : user.UserId;
        }
    }
}
