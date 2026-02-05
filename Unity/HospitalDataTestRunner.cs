using UnityEngine;
using Hospital.Data.Storage;
using Hospital.Data.Models;

namespace Hospital.Data.Unity
{
    /// <summary>
    /// Привяжи этот скрипт к любому GameObject в сцене (например, пустой объект "DatabaseTester").
    /// При запуске (Play) загрузит данные из менеджеров и выведет всё в консоль Unity.
    /// JSON-файлы должны лежать в StreamingAssets (или в подпапке, указанной в Data Folder).
    /// </summary>
    public class HospitalDataTestRunner : MonoBehaviour
    {
        [Tooltip("Подпапка в StreamingAssets, где лежат les_patients.json, les_superviseur.json, sessions.json. Оставь пустым, если файлы прямо в StreamingAssets.")]
        [SerializeField] string dataFolder = "HospitalData";

        [Tooltip("Писать ли подробный вывод по каждому полю.")]
        [SerializeField] bool verbose = true;

        void Start()
        {
            RunAllTests();
        }

        [ContextMenu("Run database tests")]
        public void RunAllTests()
        {
            string basePath = string.IsNullOrEmpty(dataFolder)
                ? Application.streamingAssetsPath
                : System.IO.Path.Combine(Application.streamingAssetsPath, dataFolder);

            Debug.Log("[HospitalData] ========== TEST START ==========");
            Debug.Log($"[HospitalData] Base path: {basePath}");

            TestPatients(basePath);
            TestSuperviseurs(basePath);
            TestSessions(basePath);

            Debug.Log("[HospitalData] ========== TEST END ==========");
        }

        void TestPatients(string basePath)
        {
            string path = System.IO.Path.Combine(basePath, "les_patients.json");
            var manager = new PatientsManager(path);

            var all = manager.GetAll();
            Debug.Log($"[HospitalData] Patients: loaded {all.Count}");

            foreach (var p in all)
            {
                if (verbose)
                    Debug.Log($"[HospitalData]   Patient: Id={p.IdPatient}, Nom={p.Nom}, Prenom={p.Prenom}, Naissance={p.AnneeNaissance}, Pathologie={p.Pathologie}, Cote={p.CoteNeglige}, Suivi={p.SuiviPatient}");
                else
                    Debug.Log($"[HospitalData]   Patient: {p.IdPatient} - {p.Prenom} {p.Nom}");
            }

            if (all.Count > 0)
            {
                var firstId = all[0].IdPatient;
                var byId = manager.GetById(firstId);
                Debug.Log(byId != null ? $"[HospitalData] GetById('{firstId}') OK" : $"[HospitalData] GetById('{firstId}') FAIL");
            }
        }

        void TestSuperviseurs(string basePath)
        {
            string path = System.IO.Path.Combine(basePath, "les_superviseur.json");
            var manager = new SuperviseursManager(path);

            var all = manager.GetAll();
            Debug.Log($"[HospitalData] Superviseurs: loaded {all.Count}");

            foreach (var s in all)
            {
                if (verbose)
                    Debug.Log($"[HospitalData]   Superviseur: Id={s.IdSuperviseur}, Nom={s.Nom}, Prenom={s.Prenom}, Fonction={s.Fonction}");
                else
                    Debug.Log($"[HospitalData]   Superviseur: {s.IdSuperviseur} - {s.Prenom} {s.Nom} ({s.Fonction})");
            }

            if (all.Count > 0)
            {
                var firstId = all[0].IdSuperviseur;
                var byId = manager.GetById(firstId);
                Debug.Log(byId != null ? $"[HospitalData] GetById('{firstId}') OK" : $"[HospitalData] GetById('{firstId}') FAIL");
            }
        }

        void TestSessions(string basePath)
        {
            string path = System.IO.Path.Combine(basePath, "sessions.json");
            var manager = new SessionsManager(path);

            var all = manager.GetAll();
            Debug.Log($"[HospitalData] Sessions: loaded {all.Count}");

            foreach (var s in all)
            {
                if (verbose)
                    Debug.Log($"[HospitalData]   Session: Patient={s.IdPatient}, Env={s.EnvironnementUtilise}, Pos={s.PositionDepart}, Duree={s.DureeSeconds}s, Score={s.ScoreTotal}, Objectifs={s.ObjectifsAtteints}/{s.ObjectifsManques}, Commentaire={s.Commentaire}");
                else
                    Debug.Log($"[HospitalData]   Session: {s.IdPatient} | Score={s.ScoreTotal} | {s.Commentaire}");
            }

            if (all.Count > 0)
            {
                var firstPatient = all[0].IdPatient;
                var byPatient = manager.GetByPatient(firstPatient);
                Debug.Log($"[HospitalData] GetByPatient('{firstPatient}') => {byPatient.Count} session(s)");
            }
        }
    }
}
