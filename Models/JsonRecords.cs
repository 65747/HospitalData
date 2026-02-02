using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hospital.Data.Models;

// Structures alignées sur les fichiers JSON dans Data/
public class PatientJson
{
    [JsonPropertyName("IDpatient")]
    public string IdPatient { get; set; } = string.Empty;

    [JsonPropertyName("Nom")]
    public string Nom { get; set; } = string.Empty;

    [JsonPropertyName("Prenom")]
    public string Prenom { get; set; } = string.Empty;

    [JsonPropertyName("date_de_naissance")]
    public int AnneeNaissance { get; set; }

    [JsonPropertyName("Sexe")]
    public string Sexe { get; set; } = string.Empty;

    [JsonPropertyName("Pathologie")]
    public string Pathologie { get; set; } = string.Empty;

    [JsonPropertyName("CoteNeglige")]
    public string CoteNeglige { get; set; } = string.Empty;

    [JsonPropertyName("DateCreation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("SuiviPatient")]
    public string SuiviPatient { get; set; } = string.Empty;
}

public class SuperviseurJson
{
    [JsonPropertyName("IdSuperviseur")]
    public string IdSuperviseur { get; set; } = string.Empty;

    [JsonPropertyName("Nom")]
    public string Nom { get; set; } = string.Empty;

    [JsonPropertyName("Prenom")]
    public string Prenom { get; set; } = string.Empty;

    [JsonPropertyName("fonction")]
    public string Fonction { get; set; } = string.Empty;
}

public class SessionJson
{
    [JsonPropertyName("IDpatient")]
    public string IdPatient { get; set; } = string.Empty;

    [JsonPropertyName("EnvironnementUtilise")]
    public string EnvironnementUtilise { get; set; } = string.Empty;

    [JsonPropertyName("PositionDepart")]
    public string PositionDepart { get; set; } = string.Empty;

    [JsonPropertyName("DateDebut")]
    public DateTime DateDebut { get; set; }

    [JsonPropertyName("niveauDifficulte")]
    public string NiveauDifficulte { get; set; } = string.Empty;

    [JsonPropertyName("NiveauAssistance_moyen")]
    public int NiveauAssistanceMoyen { get; set; }

    [JsonPropertyName("ObjectifsAtteints")]
    public string ObjectifsAtteints { get; set; } = string.Empty;

    [JsonPropertyName("ObjectifsManques")]
    public string ObjectifsManques { get; set; } = string.Empty;

    [JsonPropertyName("duree")]
    public int DureeSeconds { get; set; }

    [JsonPropertyName("ScoreTotal")]
    public int ScoreTotal { get; set; }

    [JsonPropertyName("IdSuperviseur")]
    public string IdSuperviseur { get; set; } = string.Empty;

    [JsonPropertyName("TempsReaction")]
    public double TempsReaction { get; set; }

    [JsonPropertyName("PrecisionPointage")]
    public double PrecisionPointage { get; set; }

    [JsonPropertyName("Commentaire")]
    public string Commentaire { get; set; } = string.Empty;
}

public class SessionEnvelope
{
    [JsonPropertyName("Sessions")]
    public List<SessionJson> Sessions { get; set; } = new();
}
