using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillTreeQueryDto(int Id, string Name, string Description, int? ParentId, string Path)
{
    public List<ProjectDto>? Projects { get; set; } = new List<ProjectDto>();

    public List<SkillTreeQueryDto>? Children { get; set; }
}