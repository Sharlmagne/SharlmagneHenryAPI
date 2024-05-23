using SharlmagneHenryAPI.Dtos.Project;
using SharlmagneHenryAPI.Dtos.Skill;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Mapping;

public static class SkillMapping
{
    public static Skill ToEntity(this CreateSkillDto dto)
    {
        return new Skill
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentId = dto.ParentId,
        };
    }

    public static Skill ToEntity(this UpdateSkillDto dto, int id)
    {
        return new Skill
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            ParentId = dto.ParentId,
        };
    }

    public static SkillDto ToDto(this Skill skill)
    {
        return new SkillDto(
            skill.Id,
            skill.Name,
            skill.Description,
            skill.ParentId,
            skill
                .Projects.Select(p => new ProjectDto(p.Id, p.Title, p.Description, p.Link))
                .ToList(),
            skill
                .Children.Select(c => c.ToDto()) // Recursively call ToDto for each child skill
                .ToList()
        );
    }
}