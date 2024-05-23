using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record UpdateSkillDto(
    string Name,
    string? Description,
    int? ParentId
);