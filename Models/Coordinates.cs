namespace Hospital.Data.Models;

public class Coordinates
{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public Coordinates() { }
    public Coordinates(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }
}
