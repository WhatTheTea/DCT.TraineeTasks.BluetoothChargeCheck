// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using H.NotifyIcon;

using Color = System.Windows.Media.Color;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

public partial class DeviceViewModel : ObservableObject, IDisposable
{
    // TrayIcon
    private static string[] ChargeLevelGlyphs =>
    [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
    ];
    private TaskbarIcon? trayIcon;

    private bool isTrayIconVisible = false;
    public bool IsTrayIconVisible
    {
        get => this.isTrayIconVisible;
        set
        {
            this.OnPropertyChanging();

            if (value)
            {
                this.CreateTrayIcon();
            }
            else
            {
                this.RemoveTrayIcon();
            }
        }
    }

    [ObservableProperty] private Guid id = Guid.NewGuid();
    [ObservableProperty] private Color? accent;
    [ObservableProperty] private string glyph;
    // Actual device
    [ObservableProperty] private IBluetoothDevice bluetoothDevice;

    public DeviceViewModel(IBluetoothDevice device) 
    {
        this.BluetoothDevice = device;
        this.Glyph = ChargeLevelGlyphs[0];
        this.Accent = Wpf.Ui.Appearance.ApplicationAccentColorManager.PrimaryAccent;
        // Subscribe to charge updates
        this.BluetoothDevice.PropertyChanged += this.OnChargeChanged;
    }

    [RelayCommand]
    private void CreateTrayIcon()
    {
        var template = Application.Current.Resources["BatteryTrayIcon"] as DataTemplate;
        this.trayIcon = template?.LoadContent() as TaskbarIcon ??
                          throw new InvalidOperationException("Can't load tray icon from template");
        this.trayIcon.DataContext = this;
        this.trayIcon.ForceCreate();

        this.isTrayIconVisible = true;
        this.OnPropertyChanged(nameof(this.IsTrayIconVisible));
    }


    [RelayCommand]
    private void RemoveTrayIcon()
    {
        this.trayIcon!.Dispose();
        this.trayIcon = null;

        this.isTrayIconVisible = false;
        this.OnPropertyChanged(nameof(this.IsTrayIconVisible));
    }

    private void OnChargeChanged(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IBluetoothDevice.Charge) && this.IsTrayIconVisible)
        {
            Debug.Assert(this.trayIcon is not null);
            this.Glyph = ChargeLevelGlyphs[(int)(this.BluetoothDevice.Charge / 10)];
        }
    }

    // IDisposable

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.BluetoothDevice.PropertyChanged -= this.OnChargeChanged;
            this.trayIcon?.Dispose();
        }
    }
}