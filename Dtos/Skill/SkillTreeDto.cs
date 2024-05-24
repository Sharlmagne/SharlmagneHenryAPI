using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillTreeDto(
    int Id,
    string Name,
    string Description,
    int? ParentId
)
{
    public List<ProjectIncludeDto>? Projects { get; set; } = new List<ProjectIncludeDto>();
    public List<SkillTreeDto>? Children { get; set; }
}