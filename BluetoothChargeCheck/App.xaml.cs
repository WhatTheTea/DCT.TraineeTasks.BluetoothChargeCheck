// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;

using CommunityToolkit.Mvvm.Messaging;

using DCT.BluetoothChargeCheck.Abstractions;
using DCT.BluetoothChargeCheck.Core;
using DCT.BluetoothChargeCheck.Core.Providers;
using DCT.BluetoothChargeCheck.Resources.Fonts;
using DCT.BluetoothChargeCheck.TaskbarIcons;
using DCT.BluetoothChargeCheck.ViewModels;
using DCT.BluetoothChargeCheck.ViewModels.Messages;

using Wpf.Ui.Appearance;

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
        TryAddIconsFont();

        this.taskbarIconManager = new TaskbarIconManager();

        IBluetoothDataProvider[] providers = [
            new HfpBluetoothDataProvider(),
            new PowershellBluetoothDataProvider(BluetoothKind.Classic),
            new PowershellBluetoothDataProvider(BluetoothKind.LowEnergy)
        ];
        var composite = new CompositeBluetoothDataProvider(providers);
        var service = new BluetoothService(composite);
        this.mainViewModel = new MainViewModel(service);

        WeakReferenceMessenger.Default.Register(this,
            new MessageHandler<object, ToggleTaskbarIconMessage>(this.OnToggleTaskbarIconMessage));


        this.taskbarIconManager.CreateAppIcon(this.mainViewModel);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.taskbarIconManager.Remove(this.mainViewModel.Id);
        base.OnExit(e);
    }

    /// H.NotifyIcons tries to convert <see cref="System.Windows.Media.FontFamily"/> to <see cref="System.Drawing.FontFamily"/>
    /// by searching for fonts in system. Thus I forced to install font for Win10.
    private static void TryAddIconsFont()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT
            && Environment.OSVersion.Version.Build < 22000) // System is older than Windows 11
        {
            int result = WinGdiInterop.AddFontResource("Fonts\\SegoeFluentIcons.ttf");
            if (result == 0)
            {
                throw new InvalidOperationException("Failed to install Segoe Fluent Icons");
            }
        }
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