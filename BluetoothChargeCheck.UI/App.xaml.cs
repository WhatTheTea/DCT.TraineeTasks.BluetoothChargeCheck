// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using H.NotifyIcon;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon appTrayIcon = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SystemThemeWatcher.Watch(null, WindowBackdropType.Auto);
        this.CreateAppTrayIcon();
    }

    private void CreateAppTrayIcon()
    {
        this.appTrayIcon = this.FindResource("AppTrayIcon") as TaskbarIcon
                           ?? throw new InvalidOperationException("Can't load AppTrayIcon resource");
        this.appTrayIcon.ForceCreate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.appTrayIcon.Dispose();
        base.OnExit(e);
    }
}