using SharlmagneHenryAPI.Dtos.Project;

namespace SharlmagneHenryAPI.Dtos.Skill;

public record SkillTreeDto(int Id, string Name, string Description, int? ParentId)
{
    public List<ProjectDto>? Projects { get; set; } = new List<ProjectDto>();
    public List<SkillTreeDto>? Children { get; set; }
}

public static class SkillTreeBuilder
{
    public static SkillTreeDto? BuildTree(List<SkillTreeQueryDto> nodes)
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
                node.Children.Add(childDto);
                Build(childDto);
            }
        }

        // Find the root node (ParentId is null)
        var rootQuery = lookup[null].ToList()[0];

        // Build the tree starting from the root node
        var root = new SkillTreeDto(
            rootQuery.Id,
            rootQuery.Name,
            rootQuery.Description,
            rootQuery.ParentId
        );
        Build(root);

        return root;
    }
}