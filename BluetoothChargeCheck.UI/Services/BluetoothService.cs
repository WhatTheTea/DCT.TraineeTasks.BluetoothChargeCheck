﻿// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using InTheHand.Bluetooth;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using BluetoothDevice = DCT.TraineeTasks.BluetoothChargeCheck.UI.Models.BluetoothDevice;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public partial class BluetoothService : ObservableObject, IBluetoothService
{
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> devices = [];

    public BluetoothService() => App.Current.Dispatcher.BeginInvoke(this.StartDeviceScanning, System.Windows.Threading.DispatcherPriority.Background);

    private static async IAsyncEnumerable<InTheHand.Bluetooth.BluetoothDevice> GetConnectedDevicesAsync()
    {
        var connectedSelector = BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected);
        var foundDevices = await DeviceInformation.FindAllAsync(connectedSelector);

        foreach (var device in foundDevices)
        {
            var leDevice = await BluetoothLEDevice.FromIdAsync(device.Id);

            if(leDevice is not null)
            {
                yield return leDevice;
            }
        }
    }

    private async Task StartDeviceScanning()
    {
        while (true)
        {
            await this.ScanDevices();
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private async Task ScanDevices()
    {
        if (await Bluetooth.GetAvailabilityAsync())
        {
            var pairedDevices = await GetConnectedDevicesAsync().ToArrayAsync() ?? [];

            var currentIds = this.Devices.Select(x => x.Id);
            var newIds = pairedDevices.Select(x => x.Id);

            this.AddNewDevices(pairedDevices, currentIds);
            this.RemoveUnpairedDevices(newIds);
        }
        else if (this.Devices.Count > 0)
        {
            this.Devices.Clear();
        }
    }

    private void RemoveUnpairedDevices(IEnumerable<string> newIds)
    {
        var unpairedDevices = this.Devices.Where(x => !newIds.Contains(x.Id)).ToArray();
        foreach (var device in unpairedDevices)
        {
            this.Devices.Remove(device);
        }
    }

    private void AddNewDevices(IEnumerable<InTheHand.Bluetooth.BluetoothDevice> pairedDevices, IEnumerable<string> currentIds)
    {
        var newDevices = pairedDevices.Where(x => !currentIds.Contains(x.Id))
                        .Select(x => new BluetoothDevice(x));

        foreach (var device in newDevices)
        {
            this.Devices.Add(device);
        }
    }
}