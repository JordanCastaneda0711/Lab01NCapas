using DAL;
using BLL.Exceptions;
using Entities;
using System.Collections.Generic;
using System.Linq;
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
                //Buscar si el nombre del cliente existe
                Customer customerSearch = await repository.RetreiveAsync<Customer>(c => c.FirstName == customer.FirstName);
                if(customerSearch == null)
                {
                    //No existe, podemos crearlo
                    customerResult = await repository.CreateAsync(customer);
                }
                else
                {
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return customerResult;
        }

        public async Task<Customer> RetrieveByIdAsync(int id)
        {
            Customer result = null;

            using(var repository = RepositoryFactory.CreateRepository())
            {
                Customer customer = await repository.RetreiveAsync<Customer>(c => c.Id == id);

                //revisar si el cliente se encontró
                if(customer == null)
                {
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
                //Define eñ criterio de filtro para obtener todos los clientes
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
                //Validar que el nombre del cliente no exista
                Customer customerSearch = await repository.RetreiveAsync<Customer>(c => c.FirstName == customer.FirstName && c.Id != customer.Id);
                if (customerSearch == null)
                {
                    //No existe
                    Result = await repository.UpdateAsync(customer);
                }
                else
                {
                    CustomerExceptions.ThrowCustomerAlreadyExistsException(customerSearch.FirstName, customerSearch.LastName);
                }
            }
            return Result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool Result = false;

            //Buscar a un cliente para ver si tiene Ordenes de compra
            var customer = await RetrieveByIdAsync(id);
            if(customer != null)
            {
                //Eliminar el cliente
                using(var repository = RepositoryFactory.CreateRepository())
                {
                    Result = await repository.DeleteAsync(customer);
                }
            }
            else
            {
                CustomerExceptions.ThrowInvalidCustomerIdException(id);
            }
            return Result;
        }
    }
}
