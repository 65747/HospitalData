
namespace Hospital.Data.Models;

public class Position
{
    public string IdPosition { get; set; } = string.Empty;
    public double DureeCumulee { get; set; } // seconds
    public DateTime DerniereUtilisation { get; set; }
    public int EtapeEnCours { get; set; }
    public List<int> EtapesReussies { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    // Difficulté de cet objet en fonction de la position de départ (id -> niveau 1..5)
    public Dictionary<string, int> DifficulteParPositionDepart { get; set; } = new();
    // Position 3D dans l'environnement (mètres)
    public Coordinates? Coordinates { get; set; }
}
