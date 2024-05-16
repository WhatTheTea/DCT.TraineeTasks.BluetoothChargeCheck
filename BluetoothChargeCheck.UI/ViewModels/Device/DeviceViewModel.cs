﻿// <copyright file = "DeviceViewModel.cs" company = "Digital Cloud Technologies">
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
    // Unicode battery symbols in Segoe Fluent Icons: 0,10,20,30,40,50,60,70,80,90,100%
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
        this.UpdateGlyph();
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

    private void OnChargeChanged(object? _, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IBluetoothDevice.Charge))
        {
            this.UpdateGlyph();
        }
    }

    private void UpdateGlyph()
    {
        // Tens of charge used as index for glyphs array
        int index = (int)(this.BluetoothDevice.Charge / 10);
        this.Glyph = ChargeLevelGlyphs[index];
    }
}