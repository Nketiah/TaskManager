
using AutoMapper;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Team;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Teams;

public class TeamsProfile : Profile
{
    public TeamsProfile()
    {
        CreateMap<CreateTeamRequestDto, Team>();
        CreateMap<UpdateTeamRequestDto, Team>();
        CreateMap<Team, TeamDto>();
        CreateMap<TeamDto, Team>();
        CreateMap<Member, MemberDto>();
    }
}
