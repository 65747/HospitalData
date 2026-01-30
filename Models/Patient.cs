
namespace Hospital.Data.Models;

public class Patient
{
    public int SchemaVersion { get; set; } = 1;
    public string Identifiant { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sexe { get; set; } = string.Empty;
    public string Pathologie { get; set; } = string.Empty;
    public string CoteNeglige { get; set; } = "gauche"; // gauche/droite
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public List<Experience> Experiences { get; set; } = new();
    public List<Session> Sessions { get; set; } = new();
    public List<Observation> Observations { get; set; } = new();
}
