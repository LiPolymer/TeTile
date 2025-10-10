using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace TeTile.Logics;

public static class Importer {
    public static async void Import(IStorageProvider? storageProvider) {
        try {
            if (storageProvider == null) return;
            IReadOnlyList<IStorageFile> fileSel = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
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
            File.Copy(path,"./.wakeup_schedule",true);
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (exePath.EndsWith("dll")) {
                System.Diagnostics.Process.Start("dotnet",exePath);
            } else {
                System.Diagnostics.Process.Start(exePath);
            }
            Environment.Exit(0);
        }
        catch (Exception _) {
            //ignored(neutralized)
        }
    }
}