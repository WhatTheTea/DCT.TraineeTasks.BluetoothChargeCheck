// <copyright file = "DeviceViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using DCT.BluetoothChargeCheck.Messages;
using DCT.BluetoothChargeCheck.Models;

//using Wpf.Ui.Appearance;

using Color = System.Windows.Media.Color;

namespace DCT.BluetoothChargeCheck.ViewModels.Device;

/// <summary>
/// ViewModel to contain bluetooth device data and provide data needed for tray icon <br/>
/// This viewmodel also controls tray icon appearance.
/// </summary>
///
/// <remarks>
/// Bluetooth device data meant to be updated externally. 
/// </remarks>
public partial class DeviceViewModel : ObservableObject, IViewModelWithIdentity, IDisposable
{
    [ObservableProperty]
    private Color? accent;

    [ObservableProperty]
    private BluetoothDeviceData bluetoothDevice;

    [ObservableProperty]
    private bool isTrayIconVisible;

    // Unicode battery symbols in Segoe Fluent Icons: 0,10,20,30,40,50,60,70,80,90,100%
    private static string[] ChargeLevelGlyphs =>
    [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
    ];

    public string Glyph
    {
        get
        {
            int index = (int)(this.BluetoothDevice.Charge / 10);
            return ChargeLevelGlyphs[index];
        }
    }
    public DeviceViewModel(BluetoothDeviceData? device = null)
    {
        this.BluetoothDevice = device ?? new()
        {
            Id = "ID",
            Name = "NAME",
            Charge = 50,
            Connected = true,
        };
        this.Accent = Color.FromRgb(128, 128, 128);
    }

    // Guid here is meant for TrayIcon to prevent battery icons replacing eachother
    public Guid Id { get; } = Guid.NewGuid();

    /// <see cref="ToggleTaskbarIconMessage"/>
    partial void OnIsTrayIconVisibleChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new ToggleTaskbarIconMessage(this));

    // Notify about charge update, if it changed after updating device data
    partial void OnBluetoothDeviceChanged(BluetoothDeviceData? oldValue, BluetoothDeviceData newValue)
    {
        var isChargeChanged = oldValue is not null && newValue.Charge != oldValue.Charge;
        if (oldValue is null || isChargeChanged)
        {
            this.OnPropertyChanged(nameof(this.Glyph));
        }
    }

    #region IDisposable
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    public void Dispose()
    {
        this.IsTrayIconVisible = false;
    }

    #endregion
}