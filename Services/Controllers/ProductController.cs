using BLL;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using SLC;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase, IProductService
    {
        private readonly Products _bll; // Dependency injection for better testability

        public ProductController(Products bll)
        {
            _bll = bll;
        }

        // POST: api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateAsync([FromBody] Product toCreate)
        {
            try
            {
                var product = await _bll.CreateAsync(toCreate);
                return CreatedAtRoute("RetrieveAsync", new { id = product.Id }, product); // Use CreatedAtRoute for 201 Created
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _bll.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Product not found or deletion failed."); // Informative message for unsuccessful deletion
                }
                return NoContent(); // Use NoContent for successful deletions with no content to return
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                var result = await _bll.RetrieveAllAsync();
                return Ok(result); // Use IActionResult for more flexibility (200 OK)
            }
            catch (ProductExceptions ex) // Catch specific business logic exceptions
            {
                return BadRequest(ex.Message); // Return 400 Bad Request with error message
            }
            catch (Exception ex) // Catch unhandled exceptions for logging and generic error response
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}", Name = "RetrieveProduct")]
        public async Task<ActionResult<Product>> RetrieveAsync(int id)
        {
            try
            {
                var product = await _bll.RetrieveByIDAsync(id);
                if (product == null)
                {
                    return NotFound("Product not found."); // Use NotFound result for missing resources
                }
                return Ok(product);
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product toUpdate)
        {
            // Asigna el ID recibido en la URL al objeto a actualizar
            toUpdate.Id = id;

            try
            {
                var result = await _bll.UpdateAsync(toUpdate);
                if (!result)
                {
                    return NotFound("Product not found or update failed."); // Informative message for failed updates
                }
                return NoContent(); // Use NoContent for successful updates with no content to return
            }
            catch (ProductExceptions ex)
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