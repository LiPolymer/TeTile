using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using TeTile.Logics;
using TeTile.Models;
using TeTile.ViewModels;

namespace TeTile.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        IsVisible = false;
        InitializeComponent();
    }
    
    new IStorageProvider? StorageProvider { get => TopLevel.GetTopLevel(this)?.StorageProvider; }
    void OnLoaded(object? sender,RoutedEventArgs e) {
        Settings set = ((MainWindowViewModel)DataContext!).Settings;
        Position = new PixelPoint(set.PositionX,set.PositionY);
        IsVisible = true;
        if (!((MainWindowViewModel)DataContext!).IsTableLoaded) {
            Importer.Import(StorageProvider);
        }
    }
    
    void OnClosed(object? sender,EventArgs e) {
        Environment.Exit(0);
    }
    
    void OnPointerReleased(object? sender,PointerReleasedEventArgs e) {
        ((MainWindowViewModel)DataContext!).Settings.PositionX = Position.X;
        ((MainWindowViewModel)DataContext!).Settings.PositionY = Position.Y;
    }
    
    void InputElement_OnPointerPressed(object? sender,PointerPressedEventArgs e) {
        BeginMoveDrag(e);
    }
    
    void Button_OnClick(object? sender,RoutedEventArgs e) {
        MainWindowViewModel dc = (MainWindowViewModel)DataContext!;
        dc.SelectedCourse = dc.CourseItems.Last();
    }
    
    
    void SettingsButton_OnClick(object? sender,RoutedEventArgs e) {
        MainWindowViewModel dc = (MainWindowViewModel)DataContext!;
        dc.IsSettingsClosed = false;
        SettingsWindow sw = new SettingsWindow {
            DataContext = dc.Settings
        };
        sw.Closed += (_,_) => {
            dc.IsSettingsClosed = true;
        };
        sw.Show();
    }
    void PowerButton_OnClick(object? sender,RoutedEventArgs e) {
        Close();
    }
    void InverseTopmostSelection(object? sender,RoutedEventArgs e) {
        Settings set = ((MainWindowViewModel)DataContext!).Settings;
        set.IsTopmost = !set.IsTopmost;
    }

    int _clickCounter;
    void BrandLabel_Clicked(object? sender,PointerPressedEventArgs e) {
        if (_clickCounter == 5) {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/LiPolymer");
        } else {
            _clickCounter++;
        }
    }
}