using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Hospital.Data.Models;

namespace Hospital.Data.Storage;

public class SuperviseurDatabase
{
    private readonly string _path;
    private List<Superviseur> _superviseurs = new();

    public SuperviseurDatabase(string path = "Data/les_superviseur.json")
    {
        _path = path;
    }

    public void Load()
    {
        if (!File.Exists(_path))
        {
            _superviseurs = new List<Superviseur>();
            return;
        }

        var json = File.ReadAllText(_path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        _superviseurs = JsonSerializer.Deserialize<List<Superviseur>>(json, options) ?? new List<Superviseur>();
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(_superviseurs, options);
        File.WriteAllText(_path, json);
    }

    public IReadOnlyList<Superviseur> Superviseurs => _superviseurs.AsReadOnly();

    public void Add(Superviseur superviseur)
    {
        if (superviseur == null) throw new ArgumentNullException(nameof(superviseur));
        if (string.IsNullOrWhiteSpace(superviseur.IdSuperviseur)) superviseur.IdSuperviseur = Guid.NewGuid().ToString();
        _superviseurs.Add(superviseur);
    }

    public Superviseur? FindById(string id) => _superviseurs.FirstOrDefault(s => s.IdSuperviseur == id);

    public bool RemoveById(string id)
    {
        var s = FindById(id);
        if (s == null) return false;
        return _superviseurs.Remove(s);
    }
}
