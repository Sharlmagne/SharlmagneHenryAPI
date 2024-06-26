using System.ComponentModel.DataAnnotations;

namespace SharlmagneHenryAPI.Dtos.Project;

public record CreateProjectDto(
    [Required] [StringLength(50)] string Title,
    [StringLength(500)] string? Description,
    [StringLength(200)] string? Link
);
