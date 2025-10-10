using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using TeTile.Logics;

namespace TeTile.Views;

public partial class SettingsWindow : Window {
    public SettingsWindow() {
        InitializeComponent();
    }
    void DonateButton_Clicked(object? sender,RoutedEventArgs e) {
        System.Diagnostics.Process.Start("explorer.exe", "https://afdian.tv/a/lipolymer");
    }
    void GitHubButton_Clicked(object? sender,RoutedEventArgs e) {
        System.Diagnostics.Process.Start("explorer.exe", "https://github.com/LiPolymer");
    }
    new IStorageProvider? StorageProvider { get => TopLevel.GetTopLevel(this)?.StorageProvider; }
    async void ImportButton_Clicked(object? sender,RoutedEventArgs e) {
        Importer.Import(StorageProvider);
    }
}