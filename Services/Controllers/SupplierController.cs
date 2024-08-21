using BLL;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using SLC;

namespace Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase, ISupplierService
    {
        private readonly Suppliers _bll; // Dependency injection for better testability

        public SupplierController(Suppliers bll)
        {
            _bll = bll;
        }

        // POST: api/<SupplierController>
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateAsync([FromBody] Supplier toCreate)
        {
            try
            {
                var supplier = await _bll.CreateAsync(toCreate);
                return CreatedAtRoute("RetrieveSupplier", new { id = supplier.Id }, supplier); // Use CreatedAtRoute for 201 Created
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // DELETE api/<SupplierController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _bll.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Supplier not found or deletion failed."); // Informative message for unsuccessful deletion
                }
                return NoContent(); // Use NoContent for successful deletions with no content to return
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET: api/<SupplierController>
        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetAll()
        {
            try
            {
                var result = await _bll.RetrieveAllAsync();
                return Ok(result); // Use IActionResult for more flexibility (200 OK)
            }
            catch (SupplierExceptions ex) // Catch specific business logic exceptions
            {
                return BadRequest(ex.Message); // Return 400 Bad Request with error message
            }
            catch (Exception ex) // Catch unhandled exceptions for logging and generic error response
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET api/<SupplierController>/5
        [HttpGet("{id}", Name = "RetrieveSupplier")]
        public async Task<ActionResult<Supplier>> RetrieveAsync(int id)
        {
            try
            {
                var supplier = await _bll.RetrieveByIDAsync(id);
                if (supplier == null)
                {
                    return NotFound("Supplier not found."); // Use NotFound result for missing resources
                }
                return Ok(supplier);
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // PUT api/<SupplierController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Supplier toUpdate)
        {
            // Asigna el ID recibido en la URL al objeto a actualizar
            toUpdate.Id = id;

            try
            {
                var result = await _bll.UpdateAsync(toUpdate);
                if (!result)
                {
                    return NotFound("Supplier not found or update failed."); // Informative message for failed updates
                }
                return NoContent(); // Use NoContent for successful updates with no content to return
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
