
using AutoMapper;
using TaskManager.Application.DTOs.Member;
using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<CreateTaskRequestDto, TaskItem>();
        CreateMap<UpdateTaskRequestDto, TaskItem>();
        CreateMap<TaskItem, TaskDto>();
        CreateMap<Member, MemberDto>();
    }
}
