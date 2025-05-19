﻿

namespace TaskManager.Application.DTOs.Team;

public class TeamDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int MemberCount { get; set; }
}
