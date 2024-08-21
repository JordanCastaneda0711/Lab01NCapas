using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SupplierExceptions : Exception
    {
        private SupplierExceptions(string message) : base(message)
        {
        }

        public static void ThrowSupplierAlreadyExistsException(string companyName)
        {
            throw new SupplierExceptions($"Un proveedor con el nombre '{companyName}' ya existe.");
        }

        public static void ThrowInvalidSupplierDataException(string message)
        {
            throw new SupplierExceptions(message);
        }

        public static void ThrowInvalidSupplierIdException(int id)
        {
            throw new SupplierExceptions($"Proveedor con ID {id} no existe.");
        }
    }
}
