
namespace Hospital.Data.Models;

public class Position
{
    public string IdPosition { get; set; } = string.Empty;
    public double DureeCumulee { get; set; } // s
    public DateTime DerniereUtilisation { get; set; }
    public int EtapeEnCours { get; set; }
    public List<int> EtapesReussies { get; set; } = new();
}
