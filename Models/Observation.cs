
namespace Hospital.Data.Models;

public class Observation
{
    public DateTime DateObservation { get; set; }
    public string IdentifiantProfessionnel { get; set; } = string.Empty;
    public string RoleProfessionnel { get; set; } = string.Empty;
    public string Commentaire { get; set; } = string.Empty;
    public string SuiviPatient { get; set; } = string.Empty; // notes qualitatives
}
