using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeTile.WakeUp;

public class CourseSchedule {
    public CourseSchedule(string[] jsons) {
        //todo:null铵醛
        ScheduleItems = JsonSerializer.Deserialize<List<ScheduleItem>>(jsons[4]);
        CourseTable = new CourseTable(jsons[3]);
        TimeTable = new TimeTable(jsons[1]);
        ScheduleMeta = JsonSerializer.Deserialize<ScheduleMeta>(jsons[2]);
    }

    public CourseSchedule() { }

    public CourseMeta? GetCurrentCourseMeta(DateTime? date = null) {
        date ??= DateTime.Now;
        TimeNode? tn = TimeTable!.GetCurrentNode();
        if (tn == null) return null;
        int dayOfWeek = DayOfWeekToInt(date.Value.DayOfWeek);
        int weekDelta = GetWeekDelta(DateTime.Parse(ScheduleMeta!.StartDate),date.Value);
        ScheduleItem si = ScheduleItems!.FirstOrDefault(si => si.DayOfWeek == dayOfWeek 
                                                              && si.StartWeek <= weekDelta && weekDelta <= si.EndWeek
                                                              && si.StartNode <= tn.Id && tn.Id <= si.StartNode + si.NodeStep - 1)!;
        return CourseTable!.GetCourse(si.CourseId);
    }

    public List<(CourseMeta Course,List<TimeNode> Time,ScheduleItem Schedule)> GetTodayCourses(DateTime? date = null) {
        date ??= DateTime.Now;
        int dayOfWeek = DayOfWeekToInt(date.Value.DayOfWeek);
        int weekDelta = GetWeekDelta(DateTime.Parse(ScheduleMeta!.StartDate),date.Value);
        int weekType = (weekDelta % 2 == 0) ? 2 : 1;
        return ScheduleItems!.Where(si => si.DayOfWeek == dayOfWeek 
                                          && si.StartWeek <= weekDelta 
                                          && weekDelta <= si.EndWeek
                                          && (si.Type == 0|si.Type == weekType))
            .Select(si => (CourseTable!.GetCourse(si.CourseId)
                        ,TimeTable!.Nodes!
                            .Where(tn => si.StartNode <= tn.Id
                                                  && tn.Id <= si.StartNode + si.NodeStep - 1).ToList()
                        ,si))
            .ToList();
    }
    
    static int DayOfWeekToInt(DayOfWeek dow) {
        return dow switch {
            DayOfWeek.Sunday => 7,
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    static int GetWeekDelta(DateTime baseWeekStart, DateTime currentDate = default)
    {
        if (currentDate == default)
            currentDate = DateTime.Today;
        if (baseWeekStart.DayOfWeek != DayOfWeek.Monday)
            baseWeekStart = baseWeekStart.AddDays(-(DayOfWeekToInt(baseWeekStart.DayOfWeek) - 1));
        TimeSpan diff = currentDate.Date - baseWeekStart.Date;
        if (diff.TotalDays < 0)
            throw new ArgumentException("当前日期不能早于基准周起始日。");
        int weeks = (int)Math.Floor(diff.TotalDays / 7.0) + 1;
        return weeks;
    }
    
    public ScheduleMeta? ScheduleMeta { get; set; }
    public TimeTable? TimeTable { get; set; }
    public CourseTable? CourseTable { get; set; }
    public List<ScheduleItem>? ScheduleItems { get; set; }
}

public class ScheduleItem {
    [JsonPropertyName("day")]
    public required int DayOfWeek { get; set; }
    [JsonPropertyName("id")]
    public required int CourseId { get; set; }
    [JsonPropertyName("level")]
    public int Level { get; set; } = 0;
    [JsonPropertyName("room")]
    public required string Location { get; set; }
    [JsonPropertyName("startWeek")]
    public required int StartWeek { get; set; }
    [JsonPropertyName("endWeek")]
    public required int EndWeek { get; set; }
    [JsonPropertyName("startNode")]
    public required int StartNode { get; set; }
    [JsonPropertyName("step")]
    public required int NodeStep { get; set; }
    [JsonPropertyName("tableId")]
    public required int TableId { get; set; }
    [JsonPropertyName("teacher")]
    public string Teacher { get; set; } = "???";
    [JsonPropertyName("type")]
    public int Type { get; set; } = 0; 
    [JsonPropertyName("startTime")]
    public string StartTime { get; set; } = "";
    [JsonPropertyName("endTime")]
    public string EndTime { get; set; } = "";
    [JsonPropertyName("ownTime")]
    public bool OwnTime { get; set; } = false;
}

