// <copyright file = "TestBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

internal class TestBluetoothService : ObservableObject, IBluetoothService
{
    private readonly ObservableCollection<IBluetoothDevice> devices = [];

    public ReadOnlyObservableCollection<IBluetoothDevice> Devices { get; init; }
    public ReadOnlyObservableCollection<IBluetoothDevice> Connected { get; init; }

    public TestBluetoothService()
    {
        for (int i = 0; i < 5; i++)
        {
            this.devices.Add(new TestBluetoothDevice());
        }
        this.Devices = new(this.devices);
    }
}