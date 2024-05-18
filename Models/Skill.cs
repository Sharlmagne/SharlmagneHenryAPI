namespace SharlmagneHenryAPI.Models;

public class Skill
{
    public int Id { get; set; } // Primary Key
    public required string Name { get; set; }
    public string? Description { get; set; } // Optional description

    // Foreign key referencing the parent Skill in the tree hierarchy
    public int? ParentId { get; set; }
    public Skill? Parent { get; set; }

    // Navigation property for referencing child skills
    public ICollection<Skill> Children { get; set; } = new List<Skill>(); // Initialize empty list

    // Navigation property for linking associated Projects
    public ICollection<Project> Projects { get; set; } = new List<Project>(); // Initialize empty list

    // Property to define the order within its parent's children
    public int Order { get; set; }

    // Method to add a child skill (modified to consider order)
    public void AddChild(Skill child, int order = int.MaxValue)
    {
        child.ParentId = this.Id;
        if (order == int.MaxValue)
        {
            // Add at the end if no specific order is provided
            child.Order = Children.Count;
        }
        else
        {
            child.Order = order;
            // Adjust order of existing children if needed
            foreach (var existingChild in Children.Where(c => c.Order >= order))
            {
                existingChild.Order++;
            }
        }
        Children.Add(child);
    }

    // Method to remove a child skill
    public void RemoveChild(Skill child)
    {
        if (Children.Contains(child))
        {
            child.ParentId = null;
            // Adjust order of remaining children
            int removedOrder = child.Order;
            foreach (var existingChild in Children.Where(c => c.Order > removedOrder))
            {
                existingChild.Order--;
            }
            Children.Remove(child);
        }
    }

    // Method to reposition the skill under a new parent (modified to consider order)
    public void Reposition(Skill newParent, int order = int.MaxValue)
    {
        if (this.Parent != null)
        {
            this.Parent.RemoveChild(this);
        }
        newParent.AddChild(this, order);
    }
}