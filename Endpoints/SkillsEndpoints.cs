using SharlmagneHenryAPI.Dtos.Skill;
using SharlmagneHenryAPI.Mapping;
using SharlmagneHenryAPI.Models;
using SharlmagneHenryAPI.Services;

namespace SharlmagneHenryAPI.Endpoints;

public static class SkillsEndpoints
{
    private const string GetSkillsRouteName = "GetSkills";

    public static RouteGroupBuilder MapSkillsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("skills").WithParameterValidation();

        // GET /skills
        group.MapGet(
            "/",
            async (string? include, SkillService skillService) =>
            {
                var skills = await skillService.GetSkillsAsync(include);
                return Results.Ok(skills);
            }
        );

        // GET /skills/{id}
        group
            .MapGet(
                "/{id}",
                async (int id, string? include, SkillService skillService) =>
                {
                    // If the include parameter is provided and the includes dictionary contains the include key

                    if (include != null && include.Contains("children"))
                    {
                        var skillTree = await skillService.GetSkillTree(id, include);
                        return Results.Ok(skillTree);
                    }
                    else
                    {
                        var skill = await skillService.GetSkillAsync(id, include);
                        return skill is null ? Results.NotFound() : Results.Ok(skill);
                    }
                }
            )
            .WithName(GetSkillsRouteName);

        // POST /skills
        group.MapPost(
            "/",
            async (CreateSkillDto newSkill, SkillService skillService) =>
            {
                var skill = await skillService.CreateSkillAsync(newSkill);

                return Results.CreatedAtRoute(
                    GetSkillsRouteName,
                    new { id = skill.Id },
                    skill.ToDto()
                );
            }
        );

        // PUT /skills/{id}
        group.MapPut(
            "/{id}",
            async (int id, UpdateSkillDto updatedSkill, SkillService skillService) =>
            {
                var skill = await skillService.UpdateSkillAsync(id, updatedSkill);
                return skill is null ? Results.NotFound() : Results.Ok(skill.ToDto());
            }
        );

        // DELETE /skills/{id}
        group.MapDelete(
            "/{id}",
            async (int id, SkillService skillService) =>
            {
                await skillService.DeleteSkillAsync(id);
                return Results.Ok("Skill deleted successfully!");
            }
        );

        // POST /skills/{skillId}/projects/{projectId}
        group.MapPost(
            "/{skillId}/project/{projectId}",
            async (int skillId, int projectId, SkillService skillService) =>
            {
                var result = await skillService.AddProjectToSkillAsync(skillId, projectId);

                return result switch
                {
                    200 => Results.Ok("Project added to skill successfully!"),
                    400 => Results.BadRequest("Project already exists in skill!"),
                    404 => Results.NotFound(),
                    _ => Results.Problem("An unexpected error occurred.")
                };
            }
        );

        // DELETE /skills/{skillId}/projects/{projectId}
        group.MapDelete(
            "/{skillId}/project/{projectId}",
            async (int skillId, int projectId, SkillService skillService) =>
            {
                var result = await skillService.RemoveProjectFromSkillAsync(skillId, projectId);

                return result switch
                {
                    200 => Results.Ok("Project removed from skill successfully!"),
                    400 => Results.BadRequest("Project does not exist in skill!"),
                    404 => Results.NotFound(),
                    _ => Results.Problem("An unexpected error occurred.")
                };
            }
        );

        return group;
    }
}