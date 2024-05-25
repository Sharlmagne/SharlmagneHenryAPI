using System.ComponentModel.DataAnnotations;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record CreateSkillDto(
    [Required] [StringLength(50)] string Name,
    [StringLength(500)] string? Description,
    int? ParentId
);
