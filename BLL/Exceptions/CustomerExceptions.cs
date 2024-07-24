using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class CustomerExceptions : Exception
    {
        private CustomerExceptions(string message) : base(message) 
        { 

        }

        public static void ThrowCustomerAlreadyExistsException(string firstname, string lastname)
        {
            throw new CustomerExceptions($"A client with the name already exists {firstname} {lastname}.");
        }

        public static void ThrowInvalidCustomerDataException(string message)
        {
            throw new CustomerExceptions(message);
        }

        public static void ThrowInvalidCustomerIdException(int id)
        {
            throw new CustomerExceptions($"The client with id {id} was not found or dont exist");
        }

    }
}
