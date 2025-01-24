using box_of_legends.Data;

namespace box_of_legends.Models;

public class Cart
{
    public int Id { get; set; }
    public virtual required ApplicationUser ApplicationUser { get; set; }
    public virtual required ICollection<CartLine> CartLines { get; set; }
}