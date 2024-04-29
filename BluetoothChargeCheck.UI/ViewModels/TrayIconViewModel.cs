// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using H.NotifyIcon;
using Color = System.Windows.Media.Color;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject, IDisposable
{
    protected readonly string[] ChargeStrings =
    [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
    ];

    private readonly TaskbarIcon trayIcon;

    [ObservableProperty] private Guid id = Guid.NewGuid();

    [ObservableProperty] private Color? accent;
    [ObservableProperty] private string glyph;

    [ObservableProperty] private IBluetoothDevice bluetoothDevice;

    public TrayIconViewModel(IBluetoothDevice device, Color color = default) 
    {
        this.BluetoothDevice = device;
        this.Glyph = this.ChargeStrings[0];
        this.Accent = color == default ? Wpf.Ui.Appearance.ApplicationAccentColorManager.PrimaryAccent
                                       : color;
        this.trayIcon = this.CreateTrayIcon();
        // Subscribe to charge updates
        this.BluetoothDevice.PropertyChanged += this.OnChargeChanged;
    }

    private TaskbarIcon CreateTrayIcon()
    {
        var template = Application.Current.Resources["TaskbarIcon"] as DataTemplate;
        TaskbarIcon taskbarIcon = template?.LoadContent() as TaskbarIcon ??
                          throw new ResourceReferenceKeyNotFoundException();
        taskbarIcon.DataContext = this;
        taskbarIcon.ForceCreate();
        return taskbarIcon;
    }


    [RelayCommand]
    private void RemoveIcon() =>
        Application.Current.Shutdown();

    private void OnChargeChanged(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(this.BluetoothDevice.Charge))
        {
            this.Glyph = this.ChargeStrings[(int)(this.BluetoothDevice.Charge / 10)];
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
            this.trayIcon.Dispose();
        }
    }
}