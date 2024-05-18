using System.ComponentModel.DataAnnotations;

namespace SharlmagneHenryAPI.Models;

public class Project
{
    public int Id { get; set; } // Primary Key

    [StringLength(50)]
    public required string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(200)]
    public string? Link { get; set; }

    // Navigation property for referencing associated Media objects
    public ICollection<Media>? Media { get; set; } = new List<Media>();  // Initialize empty list
}