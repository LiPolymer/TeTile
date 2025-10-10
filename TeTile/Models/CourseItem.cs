using CommunityToolkit.Mvvm.ComponentModel;

namespace TeTile.Models;

public partial class CourseItem: ObservableObject {
    [ObservableProperty]
    string _teacher = "???";
    
    [ObservableProperty]
    string _name = "???";

    [ObservableProperty] 
    string _location = "???";

    [ObservableProperty]
    string _startTime = "???";

    [ObservableProperty]
    string _endTime = "???";

    [ObservableProperty] 
    string _credit = "???";
}