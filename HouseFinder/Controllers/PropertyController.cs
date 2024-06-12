using HouseFinderBackEnd.Data.Buildings;
using HouseFinderBackEnd.Services.PropertyService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseFinderBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties(int page = 1, int pageSize = 10)
        {
            return Ok(await _propertyService.GetProperties(page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(int id)
        {
            var property = await _propertyService.GetProperty(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Property>> PostProperty(Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newProperty = await _propertyService.PostProperty(property);
                return CreatedAtAction("GetProperty", new { id = newProperty.Id }, newProperty);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "An error occurred while updating the database. Please try again." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again." });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                await _propertyService.DeleteProperty(id);
            }
            catch(InvalidOperationException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
