using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductExceptions : Exception
    {
        private ProductExceptions(string message) : base(message)
        {
        }

        public static void ThrowProductAlreadyExistsException(string productName)
        {
            throw new ProductExceptions($"Un producto con el nombre '{productName}' ya existe.");
        }

        public static void ThrowInvalidProductDataException(string message)
        {
            throw new ProductExceptions(message);
        }

        public static void ThrowInvalidProductIdException(int id)
        {
            throw new ProductExceptions($"Producto con ID {id} no existe.");
        }
    }
}
