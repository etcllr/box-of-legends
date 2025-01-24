namespace box_of_legends.Models;

public class Cart
{
    public int Id { get; set; }
    public virtual required User User { get; set; }
    public virtual required ICollection<CartLine> CartLines { get; set; }
}