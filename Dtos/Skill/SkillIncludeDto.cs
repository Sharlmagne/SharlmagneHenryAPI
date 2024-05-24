namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillIncludeDto(int Id,
    string Name,
    string? Description,
    int? ParentId);