using DAL;
using Entities.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.Identity.Client;


CreateAsync().GetAwaiter().GetResult();
RetrieveAsync().GetAwaiter().GetResult();
UpdateAsync().GetAwaiter().GetResult();
FilterAsync().GetAwaiter().GetResult();
DeleteAsync().GetAwaiter().GetResult();


Console.ReadKey();


//Crear un objeto
static async Task CreateAsync()
{
    //Add customer
    Customer customer = new Customer()
    {
        FirstName = "Jordan",
        LastName = "Castañeda",
        City = "Bogotá",
        Country = "Colombia",
        Phone = "3022169956"
    };

    using (var repository = RepositoryFactory.CreateRepository())
    {
        try
        {
            var createdCustomer = await repository.CreateAsync(customer);
            Console.WriteLine($"Added Customer: {createdCustomer.LastName} {createdCustomer.FirstName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task RetrieveAsync()
{
    using (var repository = RepositoryFactory.CreateRepository())
    {
        try
        {
            Expression<Func<Customer, bool>> criteria = c => c.FirstName == "Jordan" && c.LastName == "Castañeda";
            var customer = await repository.RetreiveAsync(criteria);
            if (customer != null)
            {
                Console.WriteLine($"Retrived customer: {customer.FirstName} \t{customer.LastName} \tCity: {customer.City} \tCountry: {customer.Country}");
            }
            else
            {
                Console.WriteLine("Customer not exist");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task UpdateAsync()
{
    //Supuesto: Existe el objeto a modificar

    using (var repository = RepositoryFactory.CreateRepository())
    {
        var customerToUpdate = await repository.RetreiveAsync<Customer>(c => c.Id == 78);
        if (customerToUpdate != null)
        {
            customerToUpdate.FirstName = "Liu";
            customerToUpdate.LastName = "Wong";
            customerToUpdate.City = "Toronto";
            customerToUpdate.Country = "Canada";
            customerToUpdate.Phone = "+1437 8524655";
        }

        try
        {
            bool updated = await repository.UpdateAsync(customerToUpdate);
            if (updated)
            {
                Console.WriteLine("Customer updated succesfully");
            }
            else
            {
                Console.WriteLine("Customer update failed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task FilterAsync()
{
    using (var repository = RepositoryFactory.CreateRepository())
    {
        Expression<Func<Customer, bool>> criteria = c => c.Country == "USA";

        var customers = await repository.FilterAsync(criteria);

        if (customers != null)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer: {customer.FirstName} {customer.LastName}\t\tFrom: {customer.City}");
            }
        }
        else
        {
            Console.WriteLine("No customers found");
        }
    }
}

static async Task DeleteAsync()
{
    using (var repository = RepositoryFactory.CreateRepository())
    {
        Expression<Func<Customer, bool>> criteria = customer => customer.Id == 93;
        var customerToDelete = await repository.RetreiveAsync(criteria);
        if (customerToDelete != null)
        {
            bool deleted = await repository.DeleteAsync(customerToDelete);
            Console.WriteLine(deleted ? "Customer deleted succesfully" : "Failed to delete customer");
        }
    }
}