//using CricketApp.API.DTOs;
//using CricketApp.API.Interfaces;
//using CricketApp.API.Models;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace CricketApp.API.Services
//{
//    public class PlayerService : IPlayerService
//    {
//        private readonly IPlayerRepository _repository;

//        public PlayerService(IPlayerRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
//        {
//            return await _repository.GetAllAsync();
//        }
//        //add extra
//        public async Task<IEnumerable<Player>> GetAllPlayersAsync(string? search = null)
//        {
//            var players = await _repository.GetAllAsync();

//            if (!string.IsNullOrEmpty(search))
//                players = players.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

//            return players;
//        }


//        public async Task<Player> GetPlayerByIdAsync(int id)
//        {
//            return await _repository.GetByIdAsync(id);
//        }

//        public async Task<Player> CreatePlayerAsync(PlayerDto playerDto)
//        {
//            var player = new Player
//            {
//                Name = playerDto.Name,
//                Team = playerDto.Team,
//                Role = playerDto.Role,
//                Age = playerDto.Age
//            };

//            return await _repository.AddAsync(player);
//        }

//        public async Task<Player> UpdatePlayerAsync(int id, PlayerDto playerDto)
//        {
//            var player = await _repository.GetByIdAsync(id);
//            if (player == null) return null;

//            player.Name = playerDto.Name;
//            player.Team = playerDto.Team;
//            player.Role = playerDto.Role;
//            player.Age = playerDto.Age;

//            return await _repository.UpdateAsync(player);
//        }

//        public async Task<bool> DeletePlayerAsync(int id)
//        {
//            return await _repository.DeleteAsync(id);
//        }
//    }
//}
using AutoMapper;
using CricketApp.API.DTOs;
using CricketApp.API.Interfaces;
using CricketApp.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CricketApp.API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync(string? search = null)
        //{
        //    var players = await _repository.GetAllAsync();

        //    if (!string.IsNullOrEmpty(search))
        //        players = players.Where(p => p.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase));

        //    return _mapper.Map<IEnumerable<PlayerDto>>(players);

        //}
        public async Task<IEnumerable<Player>> GetAllPlayersAsync(string? search = null)
        {
            var players = await _repository.GetAllAsync();
            if (!string.IsNullOrEmpty(search))
                players = players.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            return players;
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(int id)
        {
            var player = await _repository.GetByIdAsync(id);
            return _mapper.Map<PlayerDto>(player); // AutoMapper converts automatically
        }

        public async Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto)
        {
            var player = _mapper.Map<Player>(playerDto);
            var created = await _repository.AddAsync(player);
            return _mapper.Map<PlayerDto>(created);
        }

        public async Task<PlayerDto> UpdatePlayerAsync(int id, PlayerDto playerDto)
        {
            var player = await _repository.GetByIdAsync(id);
            if (player == null) return null;

            _mapper.Map(playerDto, player); // Update existing player from DTO
            var updated = await _repository.UpdateAsync(player);
            return _mapper.Map<PlayerDto>(updated);
        }



        public async Task<bool> DeletePlayerAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
