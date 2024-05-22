using Microsoft.EntityFrameworkCore;
using SharlmagneHenryAPI.Data;
using SharlmagneHenryAPI.Dtos;
using SharlmagneHenryAPI.Dtos.Project;
using SharlmagneHenryAPI.Mapping;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Endpoints;

public static class ProjectsEndpoints
{
    private const string GetProjectRouteName = "GetProject";

    public static RouteGroupBuilder MapProjectsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("projects").WithParameterValidation();

        // GET /Projects
        group.MapGet(
            "/",
            async (DataContextEf dbContext) =>
                await dbContext
                    .Projects.Select(project => project.ToDto())
                    .AsNoTracking()
                    .ToListAsync()
        );

        // GET /Projects/{id}
        group
            .MapGet(
                "/{id}",
                async (int id, DataContextEf dbContext) =>
                {
                    var project = await dbContext.Projects.FindAsync(id);

                    return project is not null ? Results.Ok(project.ToDto()) : Results.NotFound();
                }
            )
            .WithName(GetProjectRouteName);

        // POST /Projects
        group.MapPost(
            "/",
            async (CreateProjectDto newProject, DataContextEf dbContext) =>
            {
                Project project = newProject.ToEntity();
                dbContext.Projects.Add(project);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(
                    GetProjectRouteName,
                    new { id = project.Id },
                    project.ToDto()
                );
            }
        );

        // PUT /Projects/{id}
        group.MapPut(
            "/{id}",
            async (int id, UpdateProjectDto updatedProject, DataContextEf dbContext) =>
            {
                // Find the existing project
                var existingProject = await dbContext.Projects.FindAsync(id);

                // If the project does not exist, return a 404 Not Found response
                if (existingProject is null)
                {
                    return Results.NotFound();
                }

                // Update the project with the new values
                var project = updatedProject.ToEntity(id);

                // Update the project in the database
                dbContext.Entry(existingProject).CurrentValues.SetValues(project);
                await dbContext.SaveChangesAsync();

                // Return a 200 OK response with the updated project
                return Results.Ok(project.ToDto());
            }
        );

        // DELETE /Projects/{id}
        group.MapDelete(
            "/{id}",
            async (int id, DataContextEf dbContext) =>
            {
                await dbContext.Projects.Where(project => project.Id == id).ExecuteDeleteAsync();
                return Results.Ok("Project deleted successfully.");
            }
        );

        return group;
    }
}