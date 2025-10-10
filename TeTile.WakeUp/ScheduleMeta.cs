using System.Text.Json.Serialization;

namespace TeTile.WakeUp;

public class ScheduleMeta {
    [JsonPropertyName("background")]
    public string Background { get; set; } = "";
    [JsonPropertyName("courseTextColor")]
    public int CourseTextColor { get; set; } = -1;
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("itemAlpha")]
    public int ItemAlpha { get; set; } = 50;
    [JsonPropertyName("itemHeight")]
    public int ItemHeight { get; set; } = 64;
    [JsonPropertyName("itemTextSize")]
    public int ItemTextSize { get; set; } = 12;
    [JsonPropertyName("maxWeek")]
    public required int MaxWeek { get; set; }
    [JsonPropertyName("nodes")]
    public required int Nodes { get; set; }
    [JsonPropertyName("showOtherWeekCourse")]
    public bool ShowOtherWeekCourse { get; set; } = true;
    [JsonPropertyName("showSat")]
    public bool ShowSat { get; set; } = true;
    [JsonPropertyName("showSun")]
    public bool ShowSun { get; set; } = true;
    [JsonPropertyName("showTime")]
    public bool ShowTime { get; set; } = false;
    [JsonPropertyName("startDate")]
    public required string StartDate { get; set; }
    [JsonPropertyName("strokeColor")]
    public int StrokeColor { get; set; } = -2130706433;
    [JsonPropertyName("sundayFirst")]
    public bool SundayFirst { get; set; } = false;
    [JsonPropertyName("tableName")]
    public string TableName { get; set; } = "未命名";
    [JsonPropertyName("textColor")]
    public int TextColor { get; set; } = -16777216;
    [JsonPropertyName("timeTable")]
    public required int TimeTable { get; set; }
    [JsonPropertyName("type")]
    public int Type { get; set; } = 0;
    [JsonPropertyName("widgetCourseTextColor")]
    public int WidgetCourseTextColor { get; set; } = -1;
    [JsonPropertyName("widgetItemAlpha")]
    public int WidgetItemAlpha { get; set; } = 50;
    [JsonPropertyName("widgetItemHeight")]
    public int WidgetItemHeight { get; set; } = 64;
    [JsonPropertyName("widgetItemTextSize")]
    public int WidgetItemTextSize { get; set; } = 12;
    [JsonPropertyName("widgetStrokeColor")]
    public int WidgetStrokeColor { get; set; } = -2130706433;
    [JsonPropertyName("widgetTextColor")]
    public int WidgetTextColor { get; set; } = -16777216;
}