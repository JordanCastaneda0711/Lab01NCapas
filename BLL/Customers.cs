using DAL;
using Entities.Models;
using System.Linq.Expressions;

namespace BLL
{
    public class Customers
    {
        public async Task<Customer> CreateAsync(Customer customer)
        {
            Customer customerResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre de cliente existe
                Customer customerSearch = await repository.RetrieveAsync<Customer>(c => c.FirstName == customer.FirstName);
                if (customerSearch == null)
                {
                    // No existe, podemos crearlo
                    customerResult = await repository.CreateAsync(customer);
                }
                else
                {
                    // Podríamos aquí lanzar una excepción
                    // para notificar que el Cliente ya existe.
                    // Podríamos incluso crear una capa de Excepciones
                    // personalizadas y consumirla desde otras
                    // capas.
                    CustomerExceptions.ThrowCustomerAlreadyExitsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return customerResult!;
        }

        public async Task<Customer> RetrieveByIDAsync(int id)
        {
            Customer result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Customer customer = await repository.RetrieveAsync<Customer>(c => c.Id == id);

                // Check if customer was found
                if (customer == null)
                {
                    // Throw a CustomerNotFoundException (assuming you have this class)
                    CustomerExceptions.ThrowInvalidCustomerIdException(id);
                }

                return customer;
            }
        }

        public async Task<List<Customer>> RetrieveAllAsync()
        {
            List<Customer> Result = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Define el criterio de filtro para obtener todos los clientes.
                Expression<Func<Customer, bool>> allCustomersCriteria = x => true;
                Result = await r.FilterAsync<Customer>(allCustomersCriteria);
            }

            return Result;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            bool Result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que no exista otro cliente con el mismo nombre y apellido
                Customer customerSearch =
                    await repository.RetrieveAsync<Customer>
                    (c => c.FirstName == customer.FirstName
                    && c.LastName == customer.LastName
                    && c.Id != customer.Id);

                if (customerSearch == null)
                {
                    // No existe duplicado, procedemos a actualizar
                    Result = await repository.UpdateAsync(customer);
                }
                else
                {
                    // Cliente con el mismo nombre ya existe, lanzar excepción
                    CustomerExceptions.ThrowCustomerAlreadyExitsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return Result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool Result = false;
            // Buscar un cliente para ver si tiene Orders (Ordenes de Compra)
            var customer = await RetrieveByIDAsync(id);
            if (customer != null)
            {
                // Eliminar el cliente
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    Result = await repository.DeleteAsync(customer);
                }
            }
            else
            {
                // Podemos implementar alguna lógica
                // para indicar que el producto no existe
                CustomerExceptions.ThrowInvalidCustomerIdException(id);
            }
            return Result;
        }



    }
}
