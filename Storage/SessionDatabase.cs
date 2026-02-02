using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Hospital.Data.Models;

namespace Hospital.Data.Storage;

public class SessionsWrapper
{
    public List<Session> Sessions { get; set; } = new();
}

public class SessionDatabase
{
    private readonly string _path;
    private List<SessionsWrapper> _data = new();

    public SessionDatabase(string path = "Data/sessions.json")
    {
        _path = path;
    }

    public void Load()
    {
        if (!File.Exists(_path))
        {
            _data = new List<SessionsWrapper>();
            return;
        }

        var json = File.ReadAllText(_path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        _data = JsonSerializer.Deserialize<List<SessionsWrapper>>(json, options) ?? new List<SessionsWrapper>();
    }

    public void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(_data, options);
        File.WriteAllText(_path, json);
    }

    public List<Session> GetAllSessions()
    {
        return _data.SelectMany(w => w.Sessions ?? new List<Session>()).ToList();
    }

    public List<Session> GetSessionsByPatient(string patientId)
    {
        return GetAllSessions().Where(s => s.IDpatient == patientId).ToList();
    }

    public void AddSession(Session session)
    {
        if (session == null) throw new ArgumentNullException(nameof(session));

        if (_data.Count == 0)
        {
            _data.Add(new SessionsWrapper());
        }

        _data[0].Sessions ??= new List<Session>();
        _data[0].Sessions.Add(session);
    }
}
