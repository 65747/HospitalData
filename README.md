# HospitalData — Documentation et usage

Ce README décrit la structure de données, les modèles C# et l'utilisation de la petite "base de données" JSON fournie dans ce projet.

Chemins importants
- Fichiers modèle : [Models](Models)
- Stockage JSON d'exemple : [Data/patients.json](Data/patients.json)
- Gestionnaire de stockage : [Storage/PatientDatabase.cs](Storage/PatientDatabase.cs)

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

Format JSON et commentaires
- Le fichier exemple `Data/patients.json` est utilisé comme base de données locale.
- Il contient désormais des commentaires inline (`//`) pour l'explication. Le parseur JSON utilisé dans `PatientDatabase.Load()` a `ReadCommentHandling = JsonCommentHandling.Skip`, donc les commentaires sont ignorés pendant la désérialisation.
- Si vous avez besoin d'un JSON strict sans commentaires, supprimez les `//` et le fichier restera compatible.

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
