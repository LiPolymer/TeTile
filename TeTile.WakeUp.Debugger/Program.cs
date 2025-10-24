namespace TeTile.WakeUp.Debugger;

class Program {
    static void Main(string[] args) {
        CourseSchedule cs = new CourseSchedule(File.ReadAllLines("./.wakeup_schedule"));
        List<(CourseMeta Course,List<TimeNode> Time,ScheduleItem Schedule)> tcl = cs.GetTodayCourses();
        foreach ((CourseMeta Course,List<TimeNode> Time,ScheduleItem Schedule) tci in tcl) {
            Console.WriteLine($"{tci.Course.Name}@{tci.Schedule.Location}|{tci.Time.First().StartTime}-{tci.Time.Last().EndTime}|{tci.Schedule.Type}");
        }
        Console.WriteLine("Current: {0}",cs.GetCurrentCourseMeta()?.Name);
    }
}