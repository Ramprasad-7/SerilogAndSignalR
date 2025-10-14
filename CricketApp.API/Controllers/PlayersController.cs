
using CricketApp.API.DTOs;
using CricketApp.API.Interfaces;
using CricketApp.API.Models;
using CricketApp.API.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging; // add this

namespace CricketApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;
        private readonly IHubContext<AppHub> _hub;
        private readonly ILogger<PlayerController> _logger; // inject ILogger

        public PlayerController(
            IPlayerService service,
            IHubContext<AppHub> hub,
            ILogger<PlayerController> logger) // inject here
        {
            _service = service;
            _hub = hub;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll endpoint called"); // log
            var players = await _service.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GetById called with id: {Id}", id); // log
            var player = await _service.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning("Player with id {Id} not found", id); // warning log
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerDto dto)
        {
            _logger.LogInformation("Create endpoint called for player: {Name}", dto.Name);
            var player = await _service.CreatePlayerAsync(dto);
            await _hub.Clients.All.SendAsync("PlayerCreated", player);
            _logger.LogInformation("Player created with id: {Id}", player.PlayerId);
            return Ok(player);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlayerDto dto)
        {
            _logger.LogInformation("Update endpoint called for id: {Id}", id);
            var player = await _service.UpdatePlayerAsync(id, dto);
            if (player == null)
            {
                _logger.LogWarning("Player with id {Id} not found for update", id);
                return NotFound();
            }
            await _hub.Clients.All.SendAsync("PlayerUpdated", player);
            _logger.LogInformation("Player with id {Id} updated", id);
            return Ok(player);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete endpoint called for id: {Id}", id);
            var result = await _service.DeletePlayerAsync(id);
            if (!result)
            {
                _logger.LogWarning("Player with id {Id} not found for delete", id);
                return NotFound();
            }
            await _hub.Clients.All.SendAsync("PlayerDeleted", id);
            _logger.LogInformation("Player with id {Id} deleted", id);
            return Ok();
        }
    }
}
