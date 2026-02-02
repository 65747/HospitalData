namespace Hospital.Data.Models;

public class Patient
{
    public string IDpatient { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int date_de_naissance { get; set; }
    public string Sexe { get; set; } = string.Empty;
    public string Pathologie { get; set; } = string.Empty;
    public string CoteNeglige { get; set; } = "gauche"; // gauche/droite
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public string SuiviPatient { get; set; } = string.Empty;
}
