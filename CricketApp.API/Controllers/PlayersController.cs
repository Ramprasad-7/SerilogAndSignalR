
//using CricketApp.API.DTOs;
//using CricketApp.API.Interfaces;
//using CricketApp.API.Models;
//using CricketApp.API.Hubs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.Logging; // add this

//namespace CricketApp.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PlayerController : ControllerBase
//    {
//        private readonly IPlayerService _service;
//        private readonly IHubContext<AppHub> _hub;
//        private readonly ILogger<PlayerController> _logger; // inject ILogger

//        public PlayerController(
//            IPlayerService service,
//            IHubContext<AppHub> hub,
//            ILogger<PlayerController> logger) // inject here
//        {
//            _service = service;
//            _hub = hub;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            _logger.LogInformation("GetAll endpoint called"); // log
//            var players = await _service.GetAllPlayersAsync();
//            return Ok(players);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            _logger.LogInformation("GetById called with id: {Id}", id); // log
//            var player = await _service.GetPlayerByIdAsync(id);
//            if (player == null)
//            {
//                _logger.LogWarning("Player with id {Id} not found", id); // warning log
//                return NotFound();
//            }
//            return Ok(player);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(PlayerDto dto)
//        {
//            _logger.LogInformation("Create endpoint called for player: {Name}", dto.Name);
//            var player = await _service.CreatePlayerAsync(dto);
//            await _hub.Clients.All.SendAsync("PlayerCreated", player);
//            _logger.LogInformation("Player created with id: {Id}", player.PlayerId);
//            return Ok(player);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, PlayerDto dto)
//        {
//            _logger.LogInformation("Update endpoint called for id: {Id}", id);
//            var player = await _service.UpdatePlayerAsync(id, dto);
//            if (player == null)
//            {
//                _logger.LogWarning("Player with id {Id} not found for update", id);
//                return NotFound();
//            }
//            await _hub.Clients.All.SendAsync("PlayerUpdated", player);
//            _logger.LogInformation("Player with id {Id} updated", id);
//            return Ok(player);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            _logger.LogInformation("Delete endpoint called for id: {Id}", id);
//            var result = await _service.DeletePlayerAsync(id);
//            if (!result)
//            {
//                _logger.LogWarning("Player with id {Id} not found for delete", id);
//                return NotFound();
//            }
//            await _hub.Clients.All.SendAsync("PlayerDeleted", id);
//            _logger.LogInformation("Player with id {Id} deleted", id);
//            return Ok();
//        }
//    }
//}

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
