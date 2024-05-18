using System.ComponentModel.DataAnnotations;

namespace SharlmagneHenryAPI.Models;

public class Media
{
    public int Id { get; set; } // Primary Key
    public int ProjectId { get; set; } // Foreign Key referencing Project

    // Navigation property for referencing the parent Project
    public Project? Project { get; set; }


    [StringLength(50)]
    public required string Title { get; set; }

    [StringLength(200)]
    public required string FileUrl { get; set; } // Relative or absolute path to the stored file
}