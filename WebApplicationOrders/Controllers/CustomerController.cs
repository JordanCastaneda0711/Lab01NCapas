using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using ProxyService;

namespace WebApplicationOrders.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerProxy _proxy;

        public CustomersController()
        {
            this._proxy = new CustomerProxy();
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _proxy.GetAllAsync();
            return View(customers);
        }

        //Create
        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, City, Country, Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.CreateAsync(customer);
                    if (result == null)
                    {
                        return RedirectToAction("Error", new { message = "El cliente con el mismo nombre y apellido ya existe." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(customer);
        }

        //Edit
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int Id)
        {
            var customer = await _proxy.GetByIdAsync(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, City, Country, Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.UpdateAsync(id, customer);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edicion porque hay duplicidad de nombre con otro cliente" });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(customer);
        }

        //Details
        // GET: /Customer/Details/5
        public async Task<IActionResult> Details(int Id)
        {
            var customer = await _proxy.GetByIdAsync(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //Delete
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int Id)
        {
            var customer = await _proxy.GetByIdAsync(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            try
            {
                var result = await _proxy.DeleteAsync(Id);
                if (!result)
                {
                    return RedirectToAction("Error", new { message = "No se puede eliminar el cliente porque tiene facturas asociadas" });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        //Error
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
