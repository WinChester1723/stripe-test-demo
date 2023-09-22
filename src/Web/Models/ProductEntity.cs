
namespace Web.Models;

public class ProductEntity
{
    public int id { get; set; }
    public string Product { get; set; }
    public long Rate { get; set; }
    public long Quantity { get; set; }
    public string ImagePath { get; set; }
}