using DAL;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL
{
    public class Suppliers
    {
        public async Task<Supplier> CreateAsync(Supplier supplier)
        {
            Supplier supplierResult = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Buscar si el nombre del proveedor ya existe
                Supplier supplierSearch = await repository.RetrieveAsync<Supplier>(s => s.CompanyName == supplier.CompanyName);
                if (supplierSearch == null)
                {
                    // No existe, podemos crearlo
                    supplierResult = await repository.CreateAsync(supplier);
                }
                else
                {
                    // Lanzar una excepción para notificar que el proveedor ya existe
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.CompanyName);
                }
            }
            return supplierResult!;
        }

        public async Task<Supplier> RetrieveByIDAsync(int id)
        {
            Supplier result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Supplier supplier = await repository.RetrieveAsync<Supplier>(s => s.Id == id);

                // Verificar si el proveedor fue encontrado
                if (supplier == null)
                {
                    // Lanzar una excepción para indicar que el proveedor no existe
                    SupplierExceptions.ThrowInvalidSupplierIdException(id);
                }

                return supplier;
            }
        }

        public async Task<List<Supplier>> RetrieveAllAsync()
        {
            List<Supplier> result = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Definir el criterio de filtro para obtener todos los proveedores.
                Expression<Func<Supplier, bool>> allSuppliersCriteria = s => true;
                result = await repository.FilterAsync<Supplier>(allSuppliersCriteria);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del proveedor no exista ya
                Supplier supplierSearch = await repository.RetrieveAsync<Supplier>(s => s.CompanyName == supplier.CompanyName && s.Id != supplier.Id);
                if (supplierSearch == null)
                {
                    // No existe, podemos actualizar el proveedor
                    result = await repository.UpdateAsync(supplier);
                }
                else
                {
                    // Lanzar una excepción para notificar que el proveedor ya existe
                    SupplierExceptions.ThrowSupplierAlreadyExistsException(supplierSearch.CompanyName);
                }
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = false;
            // Buscar un proveedor para confirmar que existe
            var supplier = await RetrieveByIDAsync(id);
            if (supplier != null)
            {
                // Eliminar el proveedor
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    result = await repository.DeleteAsync(supplier);
                }
            }
            else
            {
                // Lanzar una excepción para indicar que el proveedor no existe
                SupplierExceptions.ThrowInvalidSupplierIdException(id);
            }
            return result;
        }
    }
}
