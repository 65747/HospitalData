
namespace Hospital.Data.Models;

public class Configuration
{
    public int SchemaVersion { get; set; } = 1;
    public string IdConfiguration { get; set; } = string.Empty;
    public string NomConfiguration { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public ParametresEnvironnement ParametresEnvironnement { get; set; } = new();
    public ParametresPatient ParametresPatient { get; set; } = new();
    public ParametresInteraction ParametresInteraction { get; set; } = new();
    public ParametresLudification ParametresLudification { get; set; } = new();
}

public class ParametresEnvironnement
{
    public string TypeEnvironnement { get; set; } = "ouvert"; // ouvert/ferme
    public string IdEnvironnement { get; set; } = string.Empty;
    public string PositionDepart { get; set; } = string.Empty;
    public bool CameraStationnaire { get; set; } = true;
    public string ZoneSecurite { get; set; } = "limitée"; // limitee/etendue
}

public class ParametresPatient
{
    public string CoteNeglige { get; set; } = "gauche"; // gauche/droite
    //public string ModeleMain { get; set; } = "auto";
    public string TeintModeleMain { get; set; } = "auto";
    public int ChampVision { get; set; } = 180; // 180/160/140
    public int NiveauDifficulte { get; set; } = 1; // 1..5
    public int NiveauAssistance { get; set; } = 1; // 1..5
}

public class ParametresInteraction
{
    public string ModeObjetsProches { get; set; } = "pointage"; // saisie/pointage
    public string TolerancePointage { get; set; } = "moyenne"; // faible/moyenne/haute
    public bool IndicesVisuels { get; set; } = true;
    public bool IndicesSonores { get; set; } = true;
    public bool VibrationManettes { get; set; } = false;
}

public class ParametresLudification
{
    public bool AfficherScore { get; set; } = true;
    public bool AfficherProgression { get; set; } = true;
    public bool RecompensesActives { get; set; } = true;
    //public int? TempsLimite { get; set; } // s/null
}
