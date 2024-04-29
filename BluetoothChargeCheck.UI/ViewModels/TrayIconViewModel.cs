// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject, IDisposable
{
    protected readonly string[] chargeStrings =
    [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
    ];

    private readonly TaskbarIcon trayIcon;

    [ObservableProperty] private Color? accent;

    [ObservableProperty] private IBluetoothDevice bluetoothDevice;

    [ObservableProperty] private string glyph;
    [ObservableProperty] private Guid name = Guid.NewGuid();

    public TrayIconViewModel(IBluetoothDevice device, string color = "#BDBDBD") // TODO: Refactor
    {
        // Init VM
        this.Glyph = this.chargeStrings[0];
        this.Accent = Color.FromRgb(255, 0, 0);
        this.BluetoothDevice = device;
        // Init View
        var template = Application.Current.Resources["TaskbarIcon"] as DataTemplate;
        this.trayIcon = template.LoadContent() as TaskbarIcon;
        this.trayIcon.DataContext = this;
        this.trayIcon.ForceCreate();
        // Subscribe to charge updates
        this.BluetoothDevice.PropertyChanged += this.OnChargeChanged;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    [RelayCommand]
    private void ExitApplication() =>
        Application.Current.Shutdown();

    private void OnChargeChanged(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(this.BluetoothDevice.Charge))
        {
            this.Glyph = this.chargeStrings[(int)(this.BluetoothDevice.Charge / 10)];
        }
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