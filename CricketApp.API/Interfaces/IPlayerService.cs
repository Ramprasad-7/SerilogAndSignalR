using CricketApp.API.DTOs;
using CricketApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketApp.API.Interfaces
{
    public interface IPlayerService
    {
                    
        Task<IEnumerable<Player>> GetAllPlayersAsync(string? search = null); 
        Task<PlayerDto> GetPlayerByIdAsync(int id);
        Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto);
        Task<PlayerDto> UpdatePlayerAsync(int id, PlayerDto playerDto);     
  
        Task<bool> DeletePlayerAsync(int id);
    }
}
