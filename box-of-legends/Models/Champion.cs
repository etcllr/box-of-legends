namespace box_of_legends.Models;

public class Champion
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual required Category Category { get; set; }
}