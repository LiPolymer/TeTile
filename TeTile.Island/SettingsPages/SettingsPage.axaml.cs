using System.IO;
using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using TeTile.Island.Services;

namespace TeTile.Island.SettingsPages;

[HidePageTitle]
[SettingsPageInfo("tetile.master","TeTile","\uEA37","\uEA36")]
public partial class SettingsPage : SettingsPageBase {
    public SettingsPage() {
        InitializeComponent();
    }

    IStorageProvider? StorageProvider { get => TopLevel.GetTopLevel(this)?.StorageProvider; }
    async void ImportSchedule(object? sender,RoutedEventArgs e) {
        try {
            if (StorageProvider == null) return;
            IReadOnlyList<IStorageFile> fileSel = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
                Title = "选择导出文件",
                FileTypeFilter = [
                    new FilePickerFileType("WakeUp 备份文件") {
                        Patterns = ["*.wakeup_schedule"],
                        MimeTypes = ["text/plain"]
                    }
                ],
                AllowMultiple = false
            });
            IStorageFile file = fileSel[0];
            string? path = file.TryGetLocalPath();
            if (path == null) return;
            if (!File.Exists(path)) return;
            File.Copy(path,Path.Combine(TeTileUpdaterService.ConfigDir,".wakeup_schedule"),true);
            TeTileUpdaterService.Inject();
        }
        catch (Exception _) {
            //ignored(neutralized)
        }
    }
    void ReloadSchedule(object? sender,RoutedEventArgs e) {
        TeTileUpdaterService.Inject();
    }
}