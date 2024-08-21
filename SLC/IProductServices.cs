using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface IProductService
    {
        Task<ActionResult<Product>> CreateAsync([FromBody] Product toCreate);

        Task<IActionResult> DeleteAsync(int id);

        Task<ActionResult<List<Product>>> GetAll();

        Task<ActionResult<Product>> RetrieveAsync(int id);

        Task<IActionResult> UpdateAsync(int id, [FromBody] Product toUpdate);
    }
}
