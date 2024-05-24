using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillTreeQueryDto(
    int Id,
    string Name,
    string Description,
    int? ParentId,
    string Path
)
{
    public List<ProjectIncludeDto>? Projects { get; set; }
    public List<SkillTreeQueryDto>? Children { get; set; }
}