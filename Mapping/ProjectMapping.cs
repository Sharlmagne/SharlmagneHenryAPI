using SharlmagneHenryAPI.Dtos;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Mapping;

public static class ProjectMapping
{
    public static Project ToEntity(this CreateProjectDto project)
    {
        return new Project()
        {
            Title = project.Title,
            Description = project.Description,
            Link = project.Link
        };
    }

    public static Project ToEntity(this UpdateProjectDto project, int id)
    {
        return new Project()
        {
            Id = id,
            Title = project.Title,
            Description = project.Description,
            Link = project.Link
        };
    }

    public static ProjectDetailsDto ToDto(this Project project)
    {
        return new ProjectDetailsDto(project.Id, project.Title, project.Description, project.Link);
    }
}