
namespace Hospital.Data.Models;

public class Session
{
    public string IdSession { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string IdConfiguration { get; set; } = string.Empty;
    public string IdSuperviseur { get; set; } = string.Empty;
    public Resultats? Resultats { get; set; }
}
