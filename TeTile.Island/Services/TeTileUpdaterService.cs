using System.IO;
using System.Reflection.PortableExecutable;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Converters;
using ClassIsland.Shared.Models.Profile;
using Google.Protobuf.Compiler;
using Microsoft.Extensions.Hosting;
using TeTile.Island.Configs;
using TeTile.WakeUp;

namespace TeTile.Island.Services;

public class TeTileUpdaterService(IProfileService profileService, IExactTimeService exactTimeService, ILessonsService lessonsService) : IHostedService {
    public Task StartAsync(CancellationToken cancellationToken) {
        _updatedDate = DateTime.Today;
        Task myInitTask = new Task(Init);
        myInitTask.Start();
        lessonsService.PreMainTimerTicked += (_,_) => {
            if (_updatedDate == DateTime.Today) return;
            _updatedDate = DateTime.Today;
            new Task(Init).Start();
        };
        OnInjectCalled += ReInject;
        return myInitTask;
    }

    public static void Inject() {
        OnInjectCalled?.Invoke();
    }

    public static event Action? OnInjectCalled;

    public static bool IsScheduleMissing { get; set; } = true;

    void ReInject() {
        if (Settings.CourseMap != null) {
            foreach (Guid cmv in Settings.CourseMap.Values) {
                profileService.Profile.Subjects.Remove(cmv);
            }
        }
        if (Settings.TimeTableGuid != null) {
            profileService.Profile.TimeLayouts.Remove(Settings.TimeTableGuid.Value);
        }
        if (Settings.ScheduleGuid != null) {
            profileService.Profile.ClassPlans.Remove(Settings.ScheduleGuid.Value);
        }
        Settings.CourseMap = null;
        Settings.TimeTableGuid = null;
        Settings.ScheduleGuid = null;
        Init();
    }
    
    void Init() {
        // Setup
        if (!File.Exists(Path.Combine(ConfigDir,".wakeup_schedule"))) return;
        IsScheduleMissing = false;
        CourseSchedule cs = new CourseSchedule(File.ReadAllLines(Path.Combine(ConfigDir,".wakeup_schedule")));
        if (Settings.TimeTableGuid == null || Settings.TimeTableGuid != null && !profileService.Profile.TimeLayouts.TryGetValue(Settings.TimeTableGuid.Value,out _)) {
            Settings.TimeTableGuid = InjectTimeLayout(cs.TimeTable!);
        }
        if (Settings.CourseMap == null || Settings.CourseMap
                .Any(cmi => !profileService.Profile.Subjects.ContainsKey(cmi.Value))) {
            //todo: 就情况单独重导入
            Settings.CourseMap = InjectCourses(cs);
        }
        InsertTempLayer(cs,Settings.CourseMap);
    }
    
    public void InsertTempLayer(CourseSchedule cs,Dictionary<int,Guid> cm) {
        profileService.ClearTempClassPlan();
        if (Settings.ScheduleGuid != null) {
            profileService.Profile.ClassPlans.Remove(Settings.ScheduleGuid.Value);
        }
        ClassPlan cp = new ClassPlan {
            TimeLayoutId = Settings.TimeTableGuid!.Value,
            Name = "TeTile Generated Schedule"
        };
        List<(CourseMeta Course,List<TimeNode> Time,ScheduleItem Schedule)> ccl = cs.GetTodayCourses();
        int currentId = 0;
        //int currentLayoutId = 0;
        foreach (TimeLayoutItem tli in profileService.Profile.TimeLayouts[Settings.TimeTableGuid.Value].Layouts) {
            // start at 1
            //currentLayoutId++;
            if (tli.TimeType != 0) continue;
            currentId++;
            (CourseMeta Course, List<TimeNode> Time, ScheduleItem Schedule)
                cci = ccl.FirstOrDefault(i => i.Schedule.StartNode <= currentId
                                              && currentId < i.Schedule.StartNode + i.Schedule.NodeStep);
            if (cci != default) {
                Dictionary<Guid,object?> aod = new Dictionary<Guid,object?> {
                    { CourseMetaAttributeGuid(),cci.Course },
                    { ScheduleItemAttributeGuid(), cci.Schedule }
                };

                ClassInfo ci = new ClassInfo {
                    Index = currentId - 1,
                    SubjectId = cm[cci.Course.Id],
                    AttachedObjects = aod
                };
                cp.Classes.Add(ci); 
            } else {
                cp.Classes.Add(new ClassInfo {
                    Index = currentId - 1,
                    IsEnabled = false
                }); 
            }
        }
        Settings.ScheduleGuid ??= Guid.NewGuid();
        profileService.Profile.ClassPlans.Add(Settings.ScheduleGuid.Value,cp);
        Guid? ti = profileService.CreateTempClassPlan(Settings.ScheduleGuid.Value,null,DateTime.Today);
        if (ti != null) {
            profileService.Profile.ClassPlans[ti.Value].Name = "TeTile Managed Layer";
        }
    }

    DateTime _updatedDate;
    public readonly Configuration Settings = Configuration.Load(ConfigDir);
    Guid CourseMetaAttributeGuid() => Guid.Parse("57efd437-00ac-42d5-afd4-3700ac42d561");
    Guid ScheduleItemAttributeGuid() => Guid.Parse("5aefbb60-ef71-4b04-afbb-60ef716b0445");
    public static string ConfigDir = "";
    
    public Dictionary<int,Guid> InjectCourses(CourseSchedule cs) {
        CourseTable ct = cs.CourseTable!;
        Dictionary<int,Guid> ccd = [];
        foreach (CourseMeta cm in ct.Courses!) {
            Guid guid = Guid.NewGuid();
            Subject sbj = new Subject {
                Name = cm.Name,
                TeacherName = cs.ScheduleItems?.FirstOrDefault(si => si.CourseId == cm.Id)?.Teacher ?? "???"
            };
            profileService.Profile.Subjects.Add(guid,sbj);
            ccd.Add(cm.Id,guid);
        }
        return ccd;
    }
    
    public Guid InjectTimeLayout(TimeTable tt) {
        Guid guid = Guid.NewGuid();
        TimeLayout tl = new TimeLayout {
            Name = "TeTile Imported"
        };
        // todo:null安全
        foreach (TimeNode tn in tt.Nodes!) {
            if (tn.StartTime == tn.EndTime) continue;
            TimeLayoutItem tli = new TimeLayoutItem {
                TimeType = 0,
                StartTime = tn.GetStartTime(),
                EndTime = tn.GetEndTime(),
            };
            tl.Layouts.Add(tli);
        }
        profileService.Profile.TimeLayouts.Add(guid,tl);
        return guid;
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}