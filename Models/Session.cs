namespace Hospital.Data.Models;

public class Session
{
    public string IDpatient { get; set; } = string.Empty;
    public string EnvironnementUtilise { get; set; } = string.Empty;
    public string PositionDepart { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }

    public string niveauDifficulte { get; set; } = "moyen"; // facile/moyen/difficile
    public int NiveauAssistance_moyen { get; set; } = 2;
    public string ObjectifsAtteints { get; set; } = string.Empty; // e.g. "80%"
    public string ObjectifsManques { get; set; } = string.Empty;  // e.g. "20%"
    public double duree { get; set; } // seconds

    public int ScoreTotal { get; set; }

    public string IdSuperviseur { get; set; } = string.Empty;
    public double TempsReaction { get; set; }
    public double PrecisionPointage { get; set; }
    public string Commentaire { get; set; } = string.Empty;
}
