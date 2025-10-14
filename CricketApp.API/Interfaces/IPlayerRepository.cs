using CricketApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CricketApp.API.Interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player> GetByIdAsync(int id);
        Task<Player> AddAsync(Player player);
        Task<Player> UpdateAsync(Player player);
        Task<bool> DeleteAsync(int id);
    }
}
