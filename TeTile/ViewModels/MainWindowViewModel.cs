using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using TeTile.Logics;
using TeTile.Models;
using TeTile.WakeUp;

namespace TeTile.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    public MainWindowViewModel() {
        Settings = Settings.Load();
        if (!File.Exists("./.wakeup_schedule")) {
            SelectedCourse = new CourseItem {
                Name = "等待导入课表文件",
                Teacher = "请在弹出窗口中操作"
            };
            IsTableLoaded = false;
            return;
        }
        Schedule = new CourseSchedule(File.ReadAllLines("./.wakeup_schedule"));

        List<CourseItem> tcl = [];
        foreach (CourseItem tci in Schedule.GetTodayCourses()
                     .Select(cm => new CourseItem {
                         Name = cm.Course.Name
                             .Trim()
                             .Replace("（","(")
                             .Replace("）",")"),
                         StartTime = cm.Time.First().StartTime,
                         EndTime = cm.Time.Last().EndTime,
                         Location = cm.Schedule.Location
                             .Trim()
                             .Replace("（","(")
                             .Replace("）",")"),
                         Teacher = cm.Schedule.Teacher,
                         Credit = cm.Course.Credit.ToString("0.0")
                     })) {
            tcl.Add(tci);
        }
        
        tcl.Sort((a,b) 
                     => TimeSpan.Parse(a.StartTime).CompareTo(TimeSpan.Parse(b.EndTime)));
        
        foreach (CourseItem ci in tcl) {
            CourseItems.Add(ci);
            DisplayedCourseItems.Add(ci);
        }
        
        SelectedCourse = CourseItems.FirstOrDefault(ci => {
            TimeSpan start = TimeSpan.Parse(ci.StartTime);
            TimeSpan end = TimeSpan.Parse(ci.EndTime);
            return start <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < end;
        }) ?? new CourseItem {
            Name = "暂无课程 ~ "
        };
        _heartbeatThread = new Thread(() => {
            while (_heartbeatThread!.IsAlive) {
                HeartBeat.Beat(this);
            }
            // ReSharper disable once FunctionNeverReturns
        });
        _heartbeatThread.Start();
    }

    public readonly CourseSchedule? Schedule;

    readonly Thread? _heartbeatThread;

    public readonly bool IsTableLoaded = true;

    public ObservableCollection<CourseItem>  CourseItems { get; set; } = [];
    
    public ObservableCollection<CourseItem>  DisplayedCourseItems { get; set; } = [];
    
    [ObservableProperty] 
    bool _isSettingsClosed = true;
    
    [ObservableProperty] 
    CourseItem? _selectedCourse = new CourseItem();

    [ObservableProperty]
    Settings _settings;
}