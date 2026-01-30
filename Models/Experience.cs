
namespace Hospital.Data.Models;

public class Experience
{
    public string IdEnvironnement { get; set; } = string.Empty;
    public string Type { get; set; } = "ouvert";// ouvert/ferme
    public List<Position> Positions { get; set; } = new();
}
