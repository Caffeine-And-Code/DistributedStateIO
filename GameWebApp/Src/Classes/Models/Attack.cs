namespace GameWebApp.Classes.Models;

public class Attack
{
    public string Id { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public int Troops { get; set; }
    public double Progress { get; set; }
}