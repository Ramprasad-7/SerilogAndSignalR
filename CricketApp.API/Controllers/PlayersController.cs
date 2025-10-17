using CricketApp.API.Common.Constants;
using CricketApp.API.Common.CricketApp.API.Common;
using CricketApp.API.DTOs;
using CricketApp.API.Hubs;
using CricketApp.API.Interfaces;
using CricketApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;


namespace CricketApp.API.Controllers
{
    [ApiController]
    [Route(RouteMapConstants.BaseControllerRoute)]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;
        private readonly IHubContext<AppHub> _hub;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(IPlayerService service, IHubContext<AppHub> hub, ILogger<PlayerController> logger)
        {
            _service = service;
            _hub = hub;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null)
        {
            try
            {
                var players = await _service.GetAllPlayersAsync(search);
                return Ok(new ApiResponse<IEnumerable<Player>>
                {
                    Success = true,
                    Message = "Players fetched successfully.",
                    Data = players
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching players");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An unexpected error occurred while fetching players.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var player = await _service.GetPlayerByIdAsync(id);
                if (player == null)
                {
                    _logger.LogWarning("Player not found with id {Id}", id);
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Player with id {id} not found."
                    });
                }

                return Ok(new ApiResponse<PlayerDto>
                {
                    Success = true,
                    Message = "Player fetched successfully.",
                    Data = player
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching player with id {Id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlayerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid player data.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var player = await _service.CreatePlayerAsync(dto);
                await _hub.Clients.All.SendAsync(HubEvents.PlayerCreated, player);

                return Ok(new ApiResponse<PlayerDto>
                {
                    Success = true,
                    Message = "Player created successfully.",
                    Data = player
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating player");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error occurred while creating player.",
                    Data = ex.Message
                });
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlayerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid player data.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var updated = await _service.UpdatePlayerAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("Player not found for update with id {Id}", id);
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Player with id {id} not found."
                    });
                }

                await _hub.Clients.All.SendAsync(HubEvents.PlayerUpdated, updated);
                return Ok(new ApiResponse<PlayerDto>
                {
                    Success = true,
                    Message = "Player updated successfully.",
                    Data = updated
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating player with id {Id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error occurred while updating player.",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeletePlayerAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Player not found for delete with id {Id}", id);
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Player with id {id} not found."
                    });
                }

                await _hub.Clients.All.SendAsync(HubEvents.PlayerDeleted, id);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Player deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting player with id {Id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error occurred while deleting player.",
                    Data = ex.Message
                });
            }
        }
    }
}
