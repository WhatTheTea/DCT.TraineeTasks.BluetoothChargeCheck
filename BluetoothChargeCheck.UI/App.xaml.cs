// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Views;
using H.NotifyIcon;
using Wpf.Ui.Controls;


namespace DCT.TraineeTasks.BluetoothChargeCheck.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon appTrayIcon;
    private MainViewModel mainViewModel = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.mainViewModel.ShowCommand.Execute(null);
        this.CreateAppTrayIcon();
    }

    private void CreateAppTrayIcon()
    {
        this.appTrayIcon = (this.FindResource("AppTrayIcon") as TaskbarIcon)!;
        this.appTrayIcon.DataContext = this.mainViewModel;
        this.appTrayIcon.ForceCreate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.appTrayIcon.Dispose();
        base.OnExit(e);
    }
}

