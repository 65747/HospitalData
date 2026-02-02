# HospitalData — Documentation et usage

Ce README décrit la structure de données, les modèles C# et l'utilisation des fichiers JSON fournis dans ce projet.

Chemins importants
- Fichiers modèle : [Models](Models)
- Stockage JSON d'exemple : [Data/les_patients.json](Data/les_patients.json), [Data/les_superviseur.json](Data/les_superviseur.json), [Data/sessions.json](Data/sessions.json)
- Gestionnaires JSON simples : [Storage/PatientsManager.cs](Storage/PatientsManager.cs), [Storage/SuperviseursManager.cs](Storage/SuperviseursManager.cs), [Storage/SessionsManager.cs](Storage/SessionsManager.cs)
- Gestionnaire historique (modèle riche `Patient`) : [Storage/PatientDatabase.cs](Storage/PatientDatabase.cs)

Résumé du projet
L'application sert à conserver localement des données patients pour des exercices VR/RA de rééducation (héminégligence, hémiplégie). Les données sont sérialisées en JSON (local) et contiennent : informations patient, expériences/environnements, objets à trouver (positions), sessions et observations cliniques.

Principaux modèles (fichiers)
- `Patient` (`Models/Patient.cs`)
  - `SchemaVersion`, `Identifiant`, `Nom`, `Prenom`, `Age`, `Sexe`, `Pathologie`, `CoteNeglige`, `DateCreation`
  - Collections : `Experiences`, `Sessions`, `Observations`

- `Experience` (`Models/Experience.cs`)
  - `IdEnvironnement`, `Type` (ouvert/ferme)
  - `Positions` : liste des objets à trouver
  - `StartPositions` : identifiants des positions de départ disponibles (ex: `assise-centre`)
  - `Difficultes` : presets (`facile`, `moyen`, `difficile`) de type `DifficultyPreset` (contient `NombreObjets`, `NiveauAssistance`, `TempsLimiteSeconds`)

- `Position` (`Models/Position.cs`)
  - `IdPosition`, `DureeCumulee`, `DerniereUtilisation`, `EtapeEnCours`, `EtapesReussies`, `Description`
  - `Coordinates` (`Models/Coordinates.cs`) : `{ x, y, z }` en mètres
  - `DifficulteParPositionDepart` : mapping `{ startPositionId -> difficulté (1..5) }`

- `Session` (`Models/Session.cs`)
  - `IdSession`, `DateDebut`, `DateFin`, `IdConfiguration`, `IdSuperviseur`, `EnvironnementUtilise`, `PositionDepart`, `NiveauAssistance`
  - `PresetName` : nom du preset utilisé pour générer la session (ex: `moyen`)
  - `NombreObjetsSelected` : nombre d'objets sélectionnés pour la session
  - `TargetPositionIds` : liste d'IDs d'objets sélectionnés pour la session
  - `Resultats` : `Resultats` (score, temps, métriques)

- `Resultats`, `Metrique`, `Observation`, `Configuration` : voir les fichiers sous `Models` pour détails supplémentaires.

## Modèles alignés sur les fichiers JSON actuels
- `PatientJson`, `SuperviseurJson`, `SessionJson` sont dans [Models/JsonRecords.cs](Models/JsonRecords.cs) et correspondent 1:1 aux fichiers du dossier Data (noms de propriétés conservés).
- L'enveloppe `SessionEnvelope` permet de lire le format actuel de [Data/sessions.json](Data/sessions.json) (`[{ "Sessions": [...] }]`).

## Nouveaux manageurs JSON prêts à l'emploi
Ces classes offrent un accès simple lecture/écriture aux trois fichiers JSON (verrouillage thread-safe, chargement lazy, écriture indentée) :
- Patients : [Storage/PatientsManager.cs](Storage/PatientsManager.cs)
- Superviseurs : [Storage/SuperviseursManager.cs](Storage/SuperviseursManager.cs)
- Sessions : [Storage/SessionsManager.cs](Storage/SessionsManager.cs)

### Exemples rapides

Afficher la liste des patients, ajouter un patient, mettre à jour le suivi :

```csharp
using Hospital.Data.Storage;

var patients = new PatientsManager();

// Lire tout
foreach (var p in patients.GetAll())
  Console.WriteLine($"{p.IdPatient} - {p.Prenom} {p.Nom}");

// Ajouter
patients.Add(new PatientJson
{
  Nom = "Doe",
  Prenom = "Jane",
  AnneeNaissance = 1990,
  Sexe = "F",
  Pathologie = "Héminégligence",
  CoteNeglige = "gauche"
});

// Modifier le suivi
patients.UpdateSuivi("patient-001", "Poursuivre les exercices côté gauche");
```

Afficher les superviseurs et en ajouter :

```csharp
var superviseurs = new SuperviseursManager();
foreach (var s in superviseurs.GetAll())
  Console.WriteLine($"{s.IdSuperviseur} - {s.Prenom} {s.Nom} ({s.Fonction})");

superviseurs.Add(new SuperviseurJson { Nom = "Martin", Prenom = "Lou", Fonction = "docteur" });
```

Afficher les sessions d'un patient donné :

```csharp
var sessions = new SessionsManager(patients, superviseurs);
var sessionsPatient = sessions.GetByPatient("patient-001");
foreach (var s in sessionsPatient)
  Console.WriteLine($"{s.DateDebut:yyyy-MM-dd HH:mm} - {s.EnvironnementUtilise} - Score {s.ScoreTotal}");
```

Ajouter une session (avec validation des IDs patient/superviseur si les manageurs sont fournis au constructeur) :

```csharp
sessions.Add(new SessionJson
{
  IdPatient = "patient-001",
  IdSuperviseur = "pro-01",
  EnvironnementUtilise = "env-forest",
  PositionDepart = "assise-centre",
  DateDebut = DateTime.UtcNow,
  NiveauDifficulte = "moyen",
  NiveauAssistanceMoyen = 2,
  ObjectifsAtteints = 80,
  ObjectifsManques = 20,
  DureeSeconds = 630,
  ScoreTotal = 120,
  TempsReaction = 1.2,
  PrecisionPointage = 0.85,
  Commentaire = "Patient a bien répondu aux indices sonores."
});
```

Format JSON et commentaires
- Les fichiers de démonstration du dossier Data sont lus en tolérant les commentaires et les virgules traînantes.
- `SessionsManager` lit le format enveloppé actuel (`[{ "Sessions": [...] }]`) et écrit au même format.

Usage basique (C#)
1) Charger la base et lister les patients :

```csharp
using Hospital.Data.Storage;

var db = new PatientDatabase("Data/patients.json");
db.Load();
foreach (var p in db.Patients) Console.WriteLine(p.Identifiant + " - " + p.Prenom + " " + p.Nom);
```

2) Générer une `Session` à partir d'un preset (sélection automatique des objets)

```csharp
// crée et sauvegarde automatiquement la session dans le JSON
var session = db.CreateSessionFromPreset(
    patientId: "patient-001",
    environmentId: "env-forest",
    startPositionId: "assise-centre",
    presetName: "moyen"
);
Console.WriteLine("Session créée : " + session.IdSession);
```

La méthode `CreateSessionFromPreset` :
- récupère l'expérience (`IdEnvironnement`) du patient
- lit le `DifficultyPreset` choisi (nombre d'objets, niveau d'assistance...)
- calcule une difficulté pour chaque `Position` selon `Position.DifficulteParPositionDepart[startPositionId]`
- sélectionne `NombreObjets` positions en tenant compte du preset (`facile` -> objets plus faciles, `difficile` -> plus difficiles, `moyen` -> biais vers moyen)
- ajoute la session au patient et sauvegarde le fichier JSON

3) Récupérer les positions complètes sélectionnées pour une session

```csharp
var targets = db.GetTargetPositions("patient-001", session.IdSession);
foreach (var t in targets) Console.WriteLine(t.IdPosition + " @ " + (t.Coordinates != null ? $"({t.Coordinates.x},{t.Coordinates.y},{t.Coordinates.z})" : "no-coords"));
```

Notes importantes
- Les difficultés des objets sont stockées par position de départ dans `Position.DifficulteParPositionDepart` afin de refléter qu'un même objet peut être plus ou moins difficile selon d'où part le patient.
- Le preset (`DifficultyPreset`) contient `NombreObjets` — c'est ce nombre qui est utilisé pour sélectionner combien d'objets seront présentés dans la session.
- `PatientDatabase.Save()` écrit le contenu en `Data/patients.json` avec indentation.

Prochaines améliorations possibles
- Ajouter une petite interface console pour créer/tester des sessions sans écrire de code
- Ajouter des tests unitaires pour `CreateSessionFromPreset` et `GetTargetPositions`
- Ajouter un export CSV des résultats

Support
Si vous voulez que j'ajoute l'interface console, les tests unitaires, ou que je supprime les commentaires du JSON pour un format strict, dites-moi laquelle de ces tâches vous préférez.
