using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeTile.WakeUp;

public class CourseTable {
    public CourseTable(string json) {
        //todo:null铵醛
        Courses = JsonSerializer.Deserialize<List<CourseMeta>>(json);
    }
    
    public CourseTable() {}

    public CourseMeta GetCourse(int id) {
        CourseMeta? selected =  Courses!.FirstOrDefault(meta => meta.Id == id);
        return selected ?? throw new ArgumentOutOfRangeException(nameof(id),"未能找到对应课程");
    }
    
    public List<CourseMeta>? Courses { get; set; }
}

public class CourseMeta {
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("courseName")]
    public required string Name { get; set; }
    [JsonPropertyName("color")]
    public required string Color { get; set; }
    [JsonPropertyName("credit")]
    public required double Credit { get; set; }
    [JsonPropertyName("tableId")]
    public required int TableId { get; set; }
    [JsonPropertyName("note")]
    public string Note { get; set; } = "";
}