namespace box_of_legends.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double Price { get; set; }
    public required string SmallImage { get; set; }
    public required string LargeImage { get; set; }

    public virtual required Champion Champion { get; set; }
}