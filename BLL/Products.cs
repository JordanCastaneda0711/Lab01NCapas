using DAL;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Products
    {
        public async Task<Product> CreateAsync(Product product)
        {
            Product productResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre del producto ya existe
                Product productSearch = await repository.RetrieveAsync<Product>(p => p.ProductName == product.ProductName);
                if (productSearch == null)
                {
                    // No existe, podemos crearlo
                    productResult = await repository.CreateAsync(product);
                }
                else
                {
                    // Lanzar una excepción para notificar que el producto ya existe
                    ProductExceptions.ThrowProductAlreadyExistsException(productSearch.ProductName);
                }
            }
            return productResult!;
        }

        public async Task<Product> RetrieveByIDAsync(int id)
        {
            Product result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Product product = await repository.RetrieveAsync<Product>(p => p.Id == id);

                // Check if product was found
                if (product == null)
                {
                    // Lanzar una excepción para indicar que el producto no existe
                    ProductExceptions.ThrowInvalidProductIdException(id);
                }

                return product;
            }
        }

        public async Task<List<Product>> RetrieveAllAsync()
        {
            List<Product> result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Definir el criterio de filtro para obtener todos los productos.
                Expression<Func<Product, bool>> allProductsCriteria = p => true;
                result = await repository.FilterAsync<Product>(allProductsCriteria);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del producto no exista ya
                Product productSearch = await repository.RetrieveAsync<Product>(p => p.ProductName == product.ProductName && p.Id != product.Id);
                if (productSearch == null)
                {
                    // No existe, podemos actualizar el producto
                    result = await repository.UpdateAsync(product);
                }
                else
                {
                    // Lanzar una excepción para notificar que el producto ya existe
                    ProductExceptions.ThrowProductAlreadyExistsException(productSearch.ProductName);
                }
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;
            // Buscar un producto para confirmar que existe
            var product = await RetrieveByIDAsync(id);
            if (product != null)
            {
                // Eliminar el producto
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    result = await repository.DeleteAsync(product);
                }
            }
            else
            {
                // Lanzar una excepción para indicar que el producto no existe
                ProductExceptions.ThrowInvalidProductIdException(id);
            }
            return result;
        }
    }
}