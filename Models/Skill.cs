using System.ComponentModel.DataAnnotations;

namespace SharlmagneHenryAPI.Models;

public class Skill
{
    public int Id { get; set; } // Primary Key

    [StringLength(50)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; } // Optional description

    // Foreign key referencing the parent Skill in the tree hierarchy
    public int? ParentId { get; set; }
    public Skill? Parent { get; set; }

    // Navigation property for referencing child skills
    public ICollection<Skill> Children { get; set; } = new List<Skill>();

    // Navigation property for linking associated Projects
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}