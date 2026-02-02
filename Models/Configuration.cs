
namespace Hospital.Data.Models;

public class Configuration
{
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
