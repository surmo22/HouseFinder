using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Data.Buildings;
using HouseFinderBackEnd.Services.WatchListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseFinderBackEnd.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListService _watchListService;

        public WatchListController(IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }


        [HttpGet("watchlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Property>>> GetUserWatchList()
        {
            try
            {
                var watchList = await _watchListService.GetUserWatchList(User);
                return Ok(watchList);
            }
            catch (ArgumentNullException) 
            {
                return Unauthorized("You should be authorized to get your watch list");
            }
            catch (InvalidOperationException)
            {
                return NotFound("User was not found");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while getting the watch list");
            }          
        }

        [HttpPost("watchlist/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToWatchList(int propertyId)
        {
            try
            {
                await _watchListService.AddPropertyToUserWatchList(User, propertyId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred {ex.Message}");
            }
        }
        [HttpPost("watchlist/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFromWatchList(int propertyId)
        {
            try
            {
                await _watchListService.RemovePropertyFromUserWatchList(User, propertyId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred {ex.Message}");
            }
        }


    }
}
