using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SharlmagneHenryAPI.Data;
using SharlmagneHenryAPI.Dtos.Skill;
using SharlmagneHenryAPI.Mapping;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Services;

public class SkillService
{
    private readonly DataContextEf _dbContext;
    private readonly IDbConnection _connection;

    public SkillService(DataContextEf dbContext, IDbConnection connection)
    {
        _dbContext = dbContext;
        _connection = connection;
    }

    private static SkillTreeDto? BuildTree(List<SkillTreeQueryDto> nodes)
    {
        // Group nodes by ParentId
        var lookup = nodes.ToLookup(node => node.ParentId);

        // Recursive function to build the tree
        void Build(SkillTreeDto node)
        {
            // Initialize Children list if it's null
            node.Children ??= new List<SkillTreeDto>();

            // Recursively build the tree for each child node
            foreach (var child in lookup[node.Id])
            {
                var childDto = new SkillTreeDto(
                    child.Id,
                    child.Name,
                    child.Description,
                    child.ParentId
                );
                childDto.Projects = child.Projects;
                node.Children.Add(childDto);
                Build(childDto);
            }
        }

        // Find the root node (first entry in the lookup)
        var rootQuery = lookup.SelectMany(x => x).First();

        // Build the tree starting from the root node
        var root = new SkillTreeDto(
            rootQuery.Id,
            rootQuery.Name,
            rootQuery.Description,
            rootQuery.ParentId
        );
        root.Projects = rootQuery.Projects;
        Build(root);

        return root;
    }

    public async Task<SkillTreeDto?> GetSkillTree(int id, string? include)
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
                s.Id = @Id

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

        SELECT * FROM skill_tree;";

        var skills = (
            await _connection.QueryAsync<SkillTreeQueryDto>(sql, new { Id = id })
        ).ToList();

        if (include == null || !include.Contains("projects"))
        {
            return BuildTree(skills);
        }

        var skillIds = skills.Select(s => s.Id).ToList();

        var projects = await _dbContext
            .Projects.Include(p => p.Skills)
            .Where(p => p.Skills.Any(s => skillIds.Contains(s.Id)))
            .Select(p => p.ToDto())
            .ToListAsync();

        foreach (var skill in skills)
        {
            var projectList = projects
                .Where(p => p.Skills != null && p.Skills.Any(s => s.Id == skill.Id))
                .ToList();
            skill.Projects = projectList.Select(p => p.ToIncludeDto()).ToList();
        }

        var skillTree = BuildTree(skills.ToList());

        return skillTree;
    }

    public async Task<SkillDto?> GetSkillAsync(int id, string? include)
    {
        var skillQuery = _dbContext.Skills.AsQueryable();

        if (include != null && include.Contains("projects"))
        {
            skillQuery = skillQuery.Include(skill => skill.Projects);
        }

        var skill = await skillQuery.FirstOrDefaultAsync(skill => skill.Id == id);

        return skill?.ToDto();
    }

    public async Task<List<SkillDto>> GetSkillsAsync(string? include)
    {
        var skillQuery = _dbContext.Skills.AsQueryable();

        if (include != null)
        {
            if (include.Contains("projects"))
            {
                skillQuery = skillQuery.Include(skill => skill.Projects);
            }
            if (include.Contains("children"))
            {
                skillQuery = skillQuery.Include(skill => skill.Children);
            }
        }

        var skills = await skillQuery.Select(skill => skill.ToDto()).AsNoTracking().ToListAsync();

        return skills;
    }

    public async Task<Skill> CreateSkillAsync(CreateSkillDto dto)
    {
        var skill = dto.ToEntity();
        _dbContext.Skills.Add(skill);
        await _dbContext.SaveChangesAsync();
        return skill;
    }

    public async Task<Skill?> UpdateSkillAsync(int id, UpdateSkillDto dto)
    {
        var existingSkill = await _dbContext.Skills.FindAsync(id);

        if (existingSkill is null)
        {
            return null;
        }

        var skill = dto.ToEntity(id);

        _dbContext.Entry(existingSkill).CurrentValues.SetValues(skill);

        await _dbContext.SaveChangesAsync();

        return skill;
    }

    public async Task DeleteSkillAsync(int id)
    {
        await _dbContext.Skills.Where(skill => skill.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> AddProjectToSkillAsync(int skillId, int projectId)
    {
        var skill = await _dbContext
            .Skills.Include(s => s.Projects)
            .FirstOrDefaultAsync(s => s.Id == skillId);
        var project = await _dbContext
            .Projects.Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (skill is null || project is null)
        {
            return 404;
        }

        if (skill.Projects.Any(p => p.Id == projectId) || project.Skills.Any(s => s.Id == skillId))
        {
            return 400;
        }

        skill.Projects.Add(project);
        project.Skills.Add(skill);

        await _dbContext.SaveChangesAsync();

        return 200;
    }

    public async Task<int> RemoveProjectFromSkillAsync(int skillId, int projectId)
    {
        var skill = await _dbContext
            .Skills.Include(s => s.Projects)
            .FirstOrDefaultAsync(s => s.Id == skillId);
        var project = await _dbContext
            .Projects.Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        // Check if the skill or project exists
        if (skill is null || project is null)
        {
            return 404; // Not Found
        }

        // Check if the project exists in the skill
        if (skill.Projects.All(p => p.Id != projectId))
        {
            return 400; // Bad Request
        }

        skill.Projects.Remove(project);
        project.Skills.Remove(skill);

        await _dbContext.SaveChangesAsync();

        return 200; // OK
    }
}
