using SharlmagneHenryAPI.Dtos.Skill;

namespace SharlmagneHenryAPI.Dtos.Project;

public record ProjectDto(int Id, string Title, string? Description, string? Link, ICollection<SkillIncludeDto>? Skills);