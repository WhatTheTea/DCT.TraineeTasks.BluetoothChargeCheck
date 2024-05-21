﻿// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
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

    public BluetoothService() => this.StartDeviceScanning();

    private void StartDeviceScanning() => Task.Factory.StartNew(async () =>
    {
        while (true)
        {
            await this.ScanDevices();
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }, TaskCreationOptions.LongRunning);

    private async Task ScanDevices()
    {
        if (await Bluetooth.GetAvailabilityAsync())
        {
            // ScanForDevicesAsync returns more devices at time cost
            //var pairedDevices = await Bluetooth.ScanForDevicesAsync(); // 10+ seconds 💀
            IReadOnlyCollection<InTheHand.Bluetooth.BluetoothDevice> pairedDevices = await Bluetooth.GetPairedDevicesAsync();

            foreach (IBluetoothDevice device in this.Devices)
            {
                (device as IDisposable)?.Dispose();
            }

            IEnumerable<BluetoothDevice> foundDevices = pairedDevices.Select(x => new BluetoothDevice(x));
            this.Devices = new ObservableCollection<IBluetoothDevice>(foundDevices);
        }
        else
        {
            this.Devices = [];
        }
    }
}