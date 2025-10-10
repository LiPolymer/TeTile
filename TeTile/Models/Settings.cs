using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TeTile.Models;

public partial class Settings: ObservableObject {
    const string Filename = "./settings.json";

    JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public Settings() {
        PropertyChanged += (_,_) => {
            File.WriteAllText(Filename,JsonSerializer.Serialize(this,_jsonOptions));
            Console.WriteLine("Configuration Saved");
        };
    }

    public static Settings Load() {
        Settings? set = null;
        if (File.Exists(Filename)) {
            set = JsonSerializer.Deserialize<Settings>(File.ReadAllText(Filename));
        }
        return set ?? new Settings();
    }

    [ObservableProperty] 
    int _positionX;
    
    [ObservableProperty] 
    int _positionY;

    [ObservableProperty]
    bool _isTopmost;

    [ObservableProperty] 
    bool _isCoursesCut;
    
    [ObservableProperty] 
    double _materialOpacity = 0.75;
}