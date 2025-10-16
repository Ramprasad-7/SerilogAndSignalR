using AutoMapper;
using CricketApp.API.DTOs;
using CricketApp.API.Models;

namespace CricketApp.API.Common.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            // Player <-> PlayerDto
            CreateMap<Player, PlayerDto>().ReverseMap();
        }
    }
}
