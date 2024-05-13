// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

public partial class BluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private double charge;
    [ObservableProperty]
    private bool connected;

    public BluetoothDevice(string name, double charge, bool connected)
    {
        this.name = name;
        this.charge = charge;
        this.connected = connected;
    }
}