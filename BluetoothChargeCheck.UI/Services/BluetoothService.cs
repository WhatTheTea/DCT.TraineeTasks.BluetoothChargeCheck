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
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> connected = [];

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
                this.Devices = new ObservableCollection<IBluetoothDevice>(pairedDevices.Select(x =>
                    new BluetoothDevice(x.Name, 0, x.IsPaired)));
                this.Connected = this.Devices;
            }
        }
        else
        {
            this.Devices = [];
            this.Connected = this.Devices;
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