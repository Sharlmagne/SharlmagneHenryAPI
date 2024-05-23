using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SharlmagneHenryAPI.Data;
using SharlmagneHenryAPI.Dtos.Skill;
using SharlmagneHenryAPI.Mapping;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Endpoints;

public static class SkillsEndpoints
{
    private const string GetSkillsRouteName = "GetSkills";

    private static async Task<IEnumerable<SkillTreeQueryDto>> GetSkillsAsync(
        IDbConnection connection,
        int id
    )
    {
        var sql =
            @"
            WITH skill_tree AS (
                SELECT
                    s.Id,
                    s.Name,
                    s.Description,
                    s.ParentId,
                    CAST(s.Id AS VARCHAR(255)) AS Path
                FROM
                    PortfolioSchema.Skills s
                WHERE
                    s.ParentId IS NULL

                UNION ALL

                SELECT
                    s.Id,
                    s.Name,
                    s.Description,
                    s.ParentId,
                    CAST(st.Path + '->' + CAST(s.Id AS VARCHAR(255)) AS VARCHAR(255))
                FROM
                    PortfolioSchema.Skills s
                INNER JOIN
                    skill_tree st ON s.ParentId = st.Id
            )
            SELECT * FROM skill_tree WHERE Id = @Id OR Path LIKE @PathPattern;";

        var skills = await connection.QueryAsync<SkillTreeQueryDto>(
            sql,
            new { Id = id, PathPattern = $"{id}->%" }
        );

        return skills;
    }

    public static RouteGroupBuilder MapSkillsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("skills").WithParameterValidation();

        var includes = new Dictionary<string, Func<IQueryable<Skill>, IQueryable<Skill>>>
        {
            ["projects"] = query => query.Include(skill => skill.Projects),
            ["children"] = query => query.Include(skill => skill.Children)
        };

        // GET /skills
        group.MapGet(
            "/",
            async (DataContextEf dbContext, string? include) =>
            {
                var skillsQuery = dbContext.Skills.AsQueryable();

                if (include != null && includes.TryGetValue(include, out var includeFunc))
                {
                    skillsQuery = includeFunc(skillsQuery);
                }

                var skills = await skillsQuery
                    .Select(skill => skill.ToDto())
                    .AsNoTracking()
                    .ToListAsync();

                return Results.Ok(skills);
            }
        );

        // GET /skills/{id}
        group
            .MapGet(
                "/{id}",
                async (int id, DataContextEf dbContext, string? include) =>
                {
                    // Start with a base query that selects from the Skills table
                    var skillQuery = dbContext.Skills.AsQueryable();

                    // If the include parameter is provided and the includes dictionary contains the include key

                    if (include != null && includes.TryGetValue(include, out var includeFunc))
                    {
                        if (include == "children")
                        {
                            var skills = await GetSkillsAsync(
                                dbContext.Database.GetDbConnection(),
                                id
                            );

                            var skillTree = SkillTreeBuilder.BuildTree(skills.ToList());
                            return Results.Ok(skillTree);
                        }
                        else
                        {
                            skillQuery = includeFunc(skillQuery);
                        }
                    }

                    var skill = await skillQuery.FirstOrDefaultAsync(s => s.Id == id);

                    return skill is not null ? Results.Ok(skill.ToDto()) : Results.NotFound();
                }
            )
            .WithName(GetSkillsRouteName);

        // POST /skills
        group.MapPost(
            "/",
            async (CreateSkillDto newSkill, DataContextEf dbContext) =>
            {
                var skill = newSkill.ToEntity();
                dbContext.Skills.Add(skill);
                await dbContext.SaveChangesAsync();

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
            async (int id, UpdateSkillDto updatedSkill, DataContextEf dbContext) =>
            {
                // Find the existing skill
                var existingSkill = await dbContext.Skills.FindAsync(id);

                if (existingSkill is null)
                {
                    return Results.NotFound();
                }

                // Convert the DTO to an entity
                var skill = updatedSkill.ToEntity(id);

                // Update the entity with the new values
                dbContext.Entry(existingSkill).CurrentValues.SetValues(skill);
                await dbContext.SaveChangesAsync();

                return Results.Ok(skill.ToDto());
            }
        );

        // DELETE /skills/{id}
        group.MapDelete(
            "/{id}",
            async (int id, DataContextEf dbContext) =>
            {
                await dbContext.Skills.Where(skill => skill.Id == id).ExecuteDeleteAsync();
                return Results.Ok("Skill deleted successfully!");
            }
        );

        // PUT /skills/{skillId}/project/{projectId}
        group.MapPut(
            "/{skillId}/project/{projectId}",
            async (int skillId, int projectId, DataContextEf dbContext) =>
            {
                var skill = await dbContext
                    .Skills.Include(s => s.Projects)
                    .FirstOrDefaultAsync(s => s.Id == skillId);
                var project = await dbContext
                    .Projects.Include(p => p.Skills)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (skill is null || project is null)
                {
                    return Results.NotFound();
                }

                if (skill.Projects.Any(p => p.Id == projectId))
                {
                    return Results.BadRequest("Project already exists in skill!");
                }

                skill.Projects.Add(project);
                project.Skills.Add(skill);

                await dbContext.SaveChangesAsync();

                return Results.Ok("Project added to skill successfully!");
            }
        );

        // DELETE /skills/{skillId}/project/{projectId}
        group.MapDelete(
            "/{skillId}/project/{projectId}",
            async (int skillId, int projectId, DataContextEf dbContext) =>
            {
                var skill = await dbContext
                    .Skills.Include(s => s.Projects)
                    .FirstOrDefaultAsync(s => s.Id == skillId);
                var project = await dbContext
                    .Projects.Include(p => p.Skills)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (skill is null || project is null)
                {
                    return Results.NotFound();
                }

                skill.Projects.Remove(project);
                project.Skills.Remove(skill);

                await dbContext.SaveChangesAsync();

                return Results.Ok("Project removed from skill successfully!");
            }
        );

        return group;
    }
}
