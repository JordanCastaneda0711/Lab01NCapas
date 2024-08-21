using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CustomerExceptions : Exception
    {
        private CustomerExceptions(string message) : base(message)
        {

        }

        public static void ThrowCustomerAlreadyExitsException(string firstName, string lastName)
        {
            throw new CustomerExceptions($"Un cliente con ese nombre ya existe{firstName} {lastName}.");

        }

        public static void ThrowInvalidCustomerDataException(string message)
        {

            throw new CustomerExceptions(message);
        }

        public static void ThrowInvalidCustomerIdException(int id)
        {
            throw new CustomerExceptions($"Cliente con ID {id} no existe.");
        }


    }
}
