namespace box_of_legends.Models;

public class CartLine
{
    public int Id { get; set; }
    public virtual required Cart Cart { get; set; }
    public virtual required Product Product { get; set; }
    public int Quantity { get; set; }
}