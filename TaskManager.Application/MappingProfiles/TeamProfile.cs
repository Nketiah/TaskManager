
using AutoMapper;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Team;
using TaskManager.Application.Features.Teams.Queries.GetAllTeams;
using TaskManager.Application.Features.Teams.Queries.GetTeamDetails;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.MappingProfiles;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<CreateTeamRequestDto, Team>();
        CreateMap<UpdateTeamRequestDto, Team>();
        CreateMap<Team, TeamDto>();
        CreateMap<Team, TeamDetailsDto>();
        CreateMap<TeamDto, Team>();
        CreateMap<Member, MemberDto>();
    }
}
