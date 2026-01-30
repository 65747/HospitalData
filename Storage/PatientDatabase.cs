using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Hospital.Data.Models;

namespace Hospital.Data.Storage;

public class PatientDatabase
{
    private readonly string _path;
    private List<Patient> _patients = new();

    public PatientDatabase(string path = "Data/patients.json")
    {
        _path = path;
    }

    public void Load()
    {
        if (!File.Exists(_path))
        {
            _patients = new List<Patient>();
            return;
        }

        var json = File.ReadAllText(_path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        _patients = JsonSerializer.Deserialize<List<Patient>>(json, options) ?? new List<Patient>();
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(_patients, options);
        File.WriteAllText(_path, json);
    }

    public IReadOnlyList<Patient> Patients => _patients.AsReadOnly();

    public void Add(Patient patient)
    {
        if (patient == null) throw new ArgumentNullException(nameof(patient));
        if (string.IsNullOrWhiteSpace(patient.Identifiant)) patient.Identifiant = Guid.NewGuid().ToString();
        _patients.Add(patient);
    }

    public Patient? FindByIdentifiant(string id) => _patients.FirstOrDefault(p => p.Identifiant == id);

    public bool RemoveByIdentifiant(string id)
    {
        var p = FindByIdentifiant(id);
        if (p == null) return false;
        return _patients.Remove(p);
    }

    // Retourne la difficulté d'une position pour une position de départ donnée (1..5)
    private int GetDifficultyForPosition(Position pos, string startPositionId)
    {
        if (pos == null) return 3;
        if (pos.DifficulteParPositionDepart != null && pos.DifficulteParPositionDepart.TryGetValue(startPositionId, out var d))
            return Math.Clamp(d, 1, 5);
        return 3; // défaut moyen
    }

    // Crée une session pour un patient en appliquant un preset de difficulty depuis l'expérience.
    // Sélectionne `NombreObjets` positions en fonction des difficultés par startPosition.
    public Session CreateSessionFromPreset(string patientId, string environmentId, string startPositionId, string presetName)
    {
        var patient = FindByIdentifiant(patientId) ?? throw new ArgumentException("Patient not found", nameof(patientId));

        var experience = patient.Experiences?.FirstOrDefault(e => string.Equals(e.IdEnvironnement, environmentId, StringComparison.OrdinalIgnoreCase));
        if (experience == null) throw new ArgumentException("Experience/environment not found on patient", nameof(environmentId));

        if (experience.Difficultes == null || !experience.Difficultes.TryGetValue(presetName, out var preset))
            throw new ArgumentException($"Preset '{presetName}' not found in experience Difficultes", nameof(presetName));

        var allPositions = experience.Positions ?? new List<Position>();

        // Build list of (position, difficulty)
        var posWithDifficulty = allPositions.Select(p => new { Pos = p, Difficulty = GetDifficultyForPosition(p, startPositionId) }).ToList();

        IEnumerable<Position> ordered;
        // Choose ordering strategy by preset name
        switch (presetName.ToLowerInvariant())
        {
            case "facile":
                // choose easiest positions first
                ordered = posWithDifficulty.OrderBy(x => x.Difficulty).Select(x => x.Pos);
                break;
            case "difficile":
                // choose hardest positions first
                ordered = posWithDifficulty.OrderByDescending(x => x.Difficulty).Select(x => x.Pos);
                break;
            default:
                // moyen or unknown -> pick random but biased to medium difficulties
                var rnd = new Random();
                ordered = posWithDifficulty.OrderBy(x => Math.Abs(x.Difficulty - 3) + rnd.NextDouble()).Select(x => x.Pos);
                break;
        }

        var numberToSelect = Math.Max(0, preset.NombreObjets);
        var selected = ordered.Take(numberToSelect).Select(p => p.IdPosition).ToList();

        var sess = new Session
        {
            IdSession = Guid.NewGuid().ToString(),
            DateDebut = DateTime.UtcNow,
            DateFin = DateTime.UtcNow, // fin à mettre à jour lors de l'enregistrement réel
            IdConfiguration = string.Empty,
            IdSuperviseur = string.Empty,
            EnvironnementUtilise = environmentId,
            PositionDepart = startPositionId,
            NiveauAssistance = preset.NiveauAssistance,
            TargetPositionIds = selected,
            PresetName = presetName,
            NombreObjetsSelected = selected.Count,
            Resultats = new Resultats()
        };

        // add to patient and persist
        patient.Sessions ??= new List<Session>();
        patient.Sessions.Add(sess);
        Save();

        return sess;
    }

    // Retourne les positions (objets) complètes sélectionnées pour une session donnée
    public List<Position> GetTargetPositions(string patientId, string sessionId)
    {
        var patient = FindByIdentifiant(patientId) ?? throw new ArgumentException("Patient not found", nameof(patientId));
        var session = patient.Sessions?.FirstOrDefault(s => s.IdSession == sessionId) ?? throw new ArgumentException("Session not found", nameof(sessionId));

        var experience = patient.Experiences?.FirstOrDefault(e => string.Equals(e.IdEnvironnement, session.EnvironnementUtilise, StringComparison.OrdinalIgnoreCase));
        if (experience == null) return new List<Position>();

        var positions = new List<Position>();
        if (session.TargetPositionIds == null) return positions;

        foreach (var id in session.TargetPositionIds)
        {
            var pos = experience.Positions?.FirstOrDefault(p => string.Equals(p.IdPosition, id, StringComparison.OrdinalIgnoreCase));
            if (pos != null) positions.Add(pos);
        }

        return positions;
    }
}
