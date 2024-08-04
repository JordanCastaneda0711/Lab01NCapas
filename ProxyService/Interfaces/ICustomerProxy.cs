using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyService.Interfaces
{
    public interface ICustomerProxy
    {
        Task<Customer> CreateAsync(Customer customer);

        Task<bool> DeleteAsync(int id);

        Task<List<Customer>> GetAllAsync();

        Task<Customer> GetByIdAsync(int id);

        Task<bool> UpdateAsync(int id, Customer customer);
    }
}
