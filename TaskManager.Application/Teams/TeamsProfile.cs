
using AutoMapper;
using TaskManager.Application.DTOs.Team;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Teams;

public class TeamsProfile : Profile
{
    public TeamsProfile()
    {
        CreateMap<CreateTeamRequestDto, Team>();
        CreateMap<Team, TeamDto>();
    }
}
