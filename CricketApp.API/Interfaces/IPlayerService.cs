//using CricketApp.API.DTOs;
//using CricketApp.API.Models;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace CricketApp.API.Interfaces
//{
//    public interface IPlayerService
//    {
//        Task<IEnumerable<Player>> GetAllPlayersAsync();
//        Task<Player> GetPlayerByIdAsync(int id);
//        Task<Player> CreatePlayerAsync(PlayerDto playerDto);
//        Task<Player> UpdatePlayerAsync(int id, PlayerDto playerDto);
//        Task<bool> DeletePlayerAsync(int id);
//        //add extra code
//        Task<IEnumerable<Player>> GetAllPlayersAsync(string? search = null);

//    }
//}
using CricketApp.API.DTOs;
using CricketApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketApp.API.Interfaces
{
    public interface IPlayerService
    {
       // Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();                  // return DTO
        Task<IEnumerable<Player>> GetAllPlayersAsync(string? search = null); // optional search
        Task<PlayerDto> GetPlayerByIdAsync(int id);                         // return DTO
        Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto);             // return DTO
        Task<PlayerDto> UpdatePlayerAsync(int id, PlayerDto playerDto);     // return DTO
        Task<bool> DeletePlayerAsync(int id);
    }
}
