
namespace Hospital.Data.Models;

public class Metrique
{
    public double TempsReaction { get; set; } // seconds
    public double PrecisionPointage { get; set; }
    public int NombreIndices { get; set; }
    public List<string> ZonesRegardees { get; set; } = new();
    public string Commentaire { get; set; } = string.Empty;
}
