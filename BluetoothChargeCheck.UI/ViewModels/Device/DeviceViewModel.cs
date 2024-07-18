// <copyright file = "DeviceViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using Wpf.Ui.Appearance;
using Color = System.Windows.Media.Color;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

/// <summary>
/// ViewModel to contain bluetooth device data and provide data needed for tray icon <br/>
/// </summary>
///
/// <remarks>
/// lol
/// </remarks>
public partial class DeviceViewModel : ObservableObject, IDisposable
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

    public DeviceViewModel(BluetoothDeviceData device)
    {
        this.BluetoothDevice = device;
        this.Accent = ApplicationAccentColorManager.PrimaryAccent;
    }

    public Guid Id { get; } = Guid.NewGuid();

    partial void OnIsTrayIconVisibleChanged(bool value) =>
        WeakReferenceMessenger.Default.Send(new TrayIconVisibilityChanged(this));

    // Notify about charge update, if it changed after updating device data
    partial void OnBluetoothDeviceChanged(BluetoothDeviceData? oldValue, BluetoothDeviceData newValue)
    {
        var isChargeChanged = (oldValue is not null) && newValue.Charge != oldValue.Charge;
        if (oldValue is null || isChargeChanged)
        {
            this.OnPropertyChanged(nameof(this.Glyph));
        }
    }

    #region IDisposable
    public void Dispose()
    {
        this.IsTrayIconVisible = false;
    }

    #endregion
}