using SharlmagneHenryAPI.Dtos.Project;
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

    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto(project.Id, project.Title, project.Description, project.Link);
    }
}
