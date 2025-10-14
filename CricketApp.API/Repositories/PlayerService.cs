using CricketApp.API.DTOs;
using CricketApp.API.Interfaces;
using CricketApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketApp.API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Player> CreatePlayerAsync(PlayerDto playerDto)
        {
            var player = new Player
            {
                Name = playerDto.Name,
                Team = playerDto.Team,
                Role = playerDto.Role,
                Age = playerDto.Age
            };

            return await _repository.AddAsync(player);
        }

        public async Task<Player> UpdatePlayerAsync(int id, PlayerDto playerDto)
        {
            var player = await _repository.GetByIdAsync(id);
            if (player == null) return null;

            player.Name = playerDto.Name;
            player.Team = playerDto.Team;
            player.Role = playerDto.Role;
            player.Age = playerDto.Age;

            return await _repository.UpdateAsync(player);
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
