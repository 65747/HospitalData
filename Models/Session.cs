
namespace Hospital.Data.Models;

public class Session
{
    public string IdSession { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }

    // Référence à la configuration utilisée
    public string IdConfiguration { get; set; } = string.Empty;

    // Métadonnées de session
    public string IdSuperviseur { get; set; } = string.Empty;
    public string EnvironnementUtilise { get; set; } = string.Empty;
    public string PositionDepart { get; set; } = string.Empty;
    public double DureeSeconds => (DateFin - DateDebut).TotalSeconds;
    public int NiveauAssistance { get; set; } = 1; // 1..5

    // Résultats et progression
    public Resultats? Resultats { get; set; }

    // Identifiants des positions/objets sélectionnés pour cette session
    public List<string> TargetPositionIds { get; set; } = new();

    // Preset utilisé pour générer la session et nombre d'objets sélectionnés
    public string? PresetName { get; set; }
    public int NombreObjetsSelected { get; set; }
}
