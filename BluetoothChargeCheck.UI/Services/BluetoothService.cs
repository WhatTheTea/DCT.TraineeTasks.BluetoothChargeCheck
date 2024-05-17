// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using InTheHand.Bluetooth;
using BluetoothDevice = DCT.TraineeTasks.BluetoothChargeCheck.UI.Models.BluetoothDevice;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public partial class BluetoothService : ObservableObject, IBluetoothService
{
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> devices = [];

    public BluetoothService()
    {
        this.StartDeviceScanning();
    }

    private async Task ScanDevices()
    {
        if (await Bluetooth.GetAvailabilityAsync())
        {
            var pairedDevices = await Bluetooth.GetPairedDevicesAsync();
            if (pairedDevices.Count != this.Devices.Count)
            {
                foreach (var device in this.Devices)
                {
                    (device as IDisposable)?.Dispose();
                }
                this.Devices = new ObservableCollection<IBluetoothDevice>(pairedDevices.Select(x =>
                    new BluetoothDevice(x)));
            }
        }
        else
        {
            this.Devices = [];
        }
    }

    private void StartDeviceScanning() => Task.Run(async () =>
    {
        while (true)
        {
            await this.ScanDevices();
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    });
}