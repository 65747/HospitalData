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

    public PatientDatabase(string path = "Data/les_patients.json")
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
        if (string.IsNullOrWhiteSpace(patient.IDpatient)) patient.IDpatient = Guid.NewGuid().ToString();
        _patients.Add(patient);
    }

    public Patient? FindById(string id) => _patients.FirstOrDefault(p => p.IDpatient == id);

    public bool RemoveById(string id)
    {
        var p = FindById(id);
        if (p == null) return false;
        return _patients.Remove(p);
    }
}
