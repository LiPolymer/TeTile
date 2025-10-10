using System.IO;
using ClassIsland.Shared.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TeTile.Island.Configs;

public partial class Configuration: ObservableObject {
    public Configuration() {
        PropertyChanged += (_,_) => Save();
    }

    public void Save() {
        ConfigureFileHelper.SaveConfig(Path.Combine(_mainPath,SubPath),this);
    }

    const string SubPath = "Config.json";
    string _mainPath = "";
    
    public static Configuration Load(string path) {
        string fPath = Path.Combine(path,SubPath);
        if (!File.Exists(fPath)) {
            ConfigureFileHelper.SaveConfig(fPath,new Configuration());
        }
        Configuration c = ConfigureFileHelper.LoadConfig<Configuration>(fPath);
        c._mainPath = path;
        return c;
    }
    
    [ObservableProperty]
    Dictionary<int,Guid>? _courseMap;

    [ObservableProperty] 
    Guid? _timeTableGuid;
    
    [ObservableProperty] 
    Guid? _scheduleGuid;
}