using CricketApp.API.DTOs;
using CricketApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketApp.API.Interfaces
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int id);
        Task<Player> CreatePlayerAsync(PlayerDto playerDto);
        Task<Player> UpdatePlayerAsync(int id, PlayerDto playerDto);
        Task<bool> DeletePlayerAsync(int id);
    }
}
