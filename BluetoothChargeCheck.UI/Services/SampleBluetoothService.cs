// <copyright file = "SampleBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

internal class SampleBluetoothService : ObservableObject, IBluetoothService
{
    private readonly ObservableCollection<IBluetoothDevice> devices = [];

    public ObservableCollection<IBluetoothDevice> Devices { get; init; }
    public ObservableCollection<IBluetoothDevice> Connected { get; init; }

    public SampleBluetoothService()
    {
        for (int i = 0; i < 5; i++)
        {
            this.devices.Add(new TestBluetoothDevice());
        }
        this.Devices = this.devices;
        this.Connected = this.Devices;
    }
}