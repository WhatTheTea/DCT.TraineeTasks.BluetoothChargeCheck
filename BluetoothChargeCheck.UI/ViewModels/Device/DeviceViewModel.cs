// <copyright file = "DeviceViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using Wpf.Ui.Appearance;
using Color = System.Windows.Media.Color;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

public partial class DeviceViewModel : ObservableObject
{
    private static string[] ChargeLevelGlyphs =>
    [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
    ];

    public Guid Id { get; } = Guid.NewGuid();
    [ObservableProperty] private Color? accent;
    [ObservableProperty] private string glyph;

    [ObservableProperty] private IBluetoothDevice bluetoothDevice;

    public DeviceViewModel(IBluetoothDevice device) 
    {
        this.BluetoothDevice = device;
        this.Glyph = ChargeLevelGlyphs[0];
        this.Accent = ApplicationAccentColorManager.PrimaryAccent;
        // Subscribe to charge updates
        this.BluetoothDevice.PropertyChanged += this.OnChargeChanged;
    }

    [ObservableProperty]
    private bool isTrayIconVisible;

    partial void OnIsTrayIconVisibleChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new TrayIconVisibilityChanged(this));
    }

    [RelayCommand]
    private void CreateTrayIcon() => this.IsTrayIconVisible = true;

    [RelayCommand]
    private void RemoveTrayIcon() => this.IsTrayIconVisible = false;

    private void OnChargeChanged(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IBluetoothDevice.Charge) && this.IsTrayIconVisible)
        {
            // Debug.Assert(this.trayIcon is not null);
            this.Glyph = ChargeLevelGlyphs[(int)(this.BluetoothDevice.Charge / 10)];
        }
    }
}