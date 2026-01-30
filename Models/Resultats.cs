
namespace Hospital.Data.Models;

public class Resultats
{
    public int ScoreTotal { get; set; }
    public double TempsTotal { get; set; } // s
    public int ObjectifsAtteints { get; set; }
    public int ObjectifsManques { get; set; }
    public List<Metrique> Metriques { get; set; } = new();
}
