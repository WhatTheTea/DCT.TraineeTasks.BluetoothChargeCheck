// <copyright file = "SampleBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

internal class SampleBluetoothService : ObservableObject, IBluetoothService
{
    private readonly ObservableCollection<BluetoothDeviceData> devices = [];

    public SampleBluetoothService()
    {
        for (int i = 0; i < 5; i++)
        {
            this.devices.Add(new SampleBluetoothDevice());
        }

        this.Devices = this.devices;
        this.Connected = this.Devices;
    }

    public ObservableCollection<BluetoothDeviceData> Connected { get; init; }

    public ObservableCollection<BluetoothDeviceData> Devices { get; init; }
}