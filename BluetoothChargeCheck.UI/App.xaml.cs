﻿// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using H.NotifyIcon;
using Wpf.Ui.Controls;


namespace DCT.TraineeTasks.BluetoothChargeCheck.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon appTrayIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this.MainWindow,
            WindowBackdropType.Auto);

        this.appTrayIcon = (this.FindResource("AppTrayIcon") as TaskbarIcon)!;
        this.appTrayIcon.ForceCreate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.appTrayIcon.Dispose();
        base.OnExit(e);
    }
}

