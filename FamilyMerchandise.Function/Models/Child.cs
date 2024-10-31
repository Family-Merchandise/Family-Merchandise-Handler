using System.Text.Json.Serialization;

namespace FamilyMerchandise.Function.Models;

public record Child
{
    public string Name { get; set; }
    public int? IconCode { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChildGender Gender { get; set; }
    public DateTime DOB { get; set; }
    public int? PointsEarned { get; set; }
}

public enum ChildGender
{
    GIRL,
    BOY
}