using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeTile.WakeUp;

public class TimeTable {
    public TimeTable(string json) {
        //todo:null铵醛
        Nodes = JsonSerializer.Deserialize<List<TimeNode>>(json);
    }
    
    public TimeTable() {}

    public TimeNode? GetCurrentNode() {
        return Nodes?.FirstOrDefault(node => {
            TimeSpan start = TimeSpan.Parse(node.StartTime);
            TimeSpan end = TimeSpan.Parse(node.EndTime);
            return start <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < end;
        });
    }
    
    public List<TimeNode>? Nodes { get; set; }
}

public class TimeNode {
    [JsonPropertyName("startTime")]
    public required string StartTime { get; set; }
    [JsonPropertyName("endTime")]
    public required string EndTime { get; set; }
    [JsonPropertyName("node")]
    public required int Id { get; set; }
    [JsonPropertyName("timeTable")]
    public required int TableId { get; set; }

    public TimeSpan GetStartTime() {
        return TimeSpan.Parse(StartTime);
    }
    
    public TimeSpan GetEndTime() {
        return TimeSpan.Parse(EndTime);
    }
}