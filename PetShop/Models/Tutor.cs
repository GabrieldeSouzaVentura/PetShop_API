namespace PetShop.Models;

public class Tutor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NumberContect { get; set; }
    public string Address { get; set; }
    public List<Pet> Pets { get; set; } = new();
}