
namespace Hospital.Data.Models;

public class Experience
{
    public string IdEnvironnement { get; set; } = string.Empty;
    public string Type { get; set; } = "ouvert"; // ouvert/ferme
    // Positions correspondent aux objets à trouver dans l'expérience
    public List<Position> Positions { get; set; } = new();

    // Positions de départ disponibles dans cet environnement (identifiants)
    public List<string> StartPositions { get; set; } = new();

    // Presets de difficulté par environnement : "facile", "moyen", "difficile"
    public Dictionary<string, DifficultyPreset> Difficultes { get; set; } = new();

    public int NombrePositions => Positions?.Count ?? 0;
}

public class DifficultyPreset
{
    public int NombreObjets { get; set; } = 5;
    public int NiveauAssistance { get; set; } = 1;
    public int TempsLimiteSeconds { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
}
