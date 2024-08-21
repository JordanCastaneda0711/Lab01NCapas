using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer.Interfaces
{
    public interface IProductProxy
    {
        Task<Product> CreateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Product product);
    }
}