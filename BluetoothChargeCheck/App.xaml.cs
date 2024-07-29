// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Windows;

using CommunityToolkit.Mvvm.Messaging;

using DCT.BluetoothChargeCheck.TaskbarIcons;
using DCT.BluetoothChargeCheck.ViewModels;
using DCT.BluetoothChargeCheck.ViewModels.Messages;

using Microsoft.Win32;

using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DCT.BluetoothChargeCheck;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIconManager taskbarIconManager = null!;
    private MainViewModel mainViewModel = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ApplicationThemeManager.ApplySystemTheme();

        this.taskbarIconManager = new TaskbarIconManager();
        this.mainViewModel = new MainViewModel();

        WeakReferenceMessenger.Default.Register(this,
            new MessageHandler<object, ToggleTaskbarIconMessage>(this.OnToggleTaskbarIconMessage));


        this.taskbarIconManager.CreateAppIcon(this.mainViewModel);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.taskbarIconManager.Remove(this.mainViewModel.Id);
        base.OnExit(e);
    }

    void OnToggleTaskbarIconMessage(object recipient, ToggleTaskbarIconMessage message)
    {
        var deviceViewModel = message.Value;

        if (deviceViewModel.IsTrayIconVisible)
        {
            this.taskbarIconManager.CreateDeviceIcon(deviceViewModel);
        }
        else
        {
            this.taskbarIconManager.Remove(deviceViewModel.Id);
        }
    }
}