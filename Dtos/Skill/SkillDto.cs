using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillDto(
    int Id,
    string Name,
    string? Description,
    int? ParentId,
    ICollection<ProjectDto>? Projects,
    ICollection<SkillDto>? Children
);