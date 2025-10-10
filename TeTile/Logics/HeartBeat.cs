using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TeTile.Models;
using TeTile.ViewModels;

namespace TeTile.Logics;

public static class HeartBeat {
    static int _beepCounter = 0;
    public static void Beat(MainWindowViewModel mvm) {
        // todo:检查时间(天)
        if (mvm.Settings.IsCoursesCut) {
            CourseItem[] cis = mvm.CourseItems.Where(ci => {
                TimeSpan end = TimeSpan.Parse(ci.EndTime);
                return DateTime.Now.TimeOfDay < end;
            }).ToArray();
            foreach (CourseItem ci in mvm.DisplayedCourseItems.ToArray()) {
                if (!cis.Contains(ci)) {
                    mvm.DisplayedCourseItems.Remove(ci);
                }
            }
        } else {
            if (mvm.CourseItems.Any(ci => !mvm.DisplayedCourseItems.Contains(ci))) {
                foreach (CourseItem dci in mvm.DisplayedCourseItems.ToArray()) {
                    mvm.DisplayedCourseItems.Remove(dci);
                }
                foreach (CourseItem cii in mvm.CourseItems) {
                    mvm.DisplayedCourseItems.Add(cii);
                }
            }
        }
        
        CourseItem? selected = mvm.CourseItems.FirstOrDefault(ci => {
            TimeSpan start = TimeSpan.Parse(ci.StartTime);
            TimeSpan end = TimeSpan.Parse(ci.EndTime);
            return start <= DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay < end;
        });
        if (selected == null) {
            CourseItem? next = mvm.CourseItems.FirstOrDefault(ci => {
                TimeSpan start = TimeSpan.Parse(ci.StartTime);
                return DateTime.Now.TimeOfDay <= start;
            });
            if (next == null) selected = null;
            else {
                selected = new CourseItem {
                    Name = $"下一课: {next.Name}",
                    Location = next.Location,
                    Teacher = next.Teacher,
                    StartTime = next.StartTime,
                    EndTime = next.EndTime,
                    Credit = next.Credit
                };
            }
        }
        selected ??= new CourseItem {
            Name = mvm.Schedule.GetTodayCourses().Count == 0 ? "今天没有课程 ~" : "今日课程已完成!",
            Credit = "Enjoy your time ~"
        };
        if (mvm.SelectedCourse?.StartTime != selected.StartTime) {
            new Thread(() => {
                if (_beepCounter == 0) _beepCounter++;
                //else Console.Beep();
            }).Start();
        }
        mvm.SelectedCourse = selected;
        Thread.Sleep(100);
    }
}