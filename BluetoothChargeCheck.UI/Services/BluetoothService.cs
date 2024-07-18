// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;

using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

using InTheHand.Bluetooth;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
public partial class BluetoothService : ObservableObject, IBluetoothService
{
    public ObservableCollection<BluetoothDeviceData> Devices => throw new NotImplementedException();

    private Func<IAsyncEnumerable<BluetoothDeviceData>> FetchDevices;

    public async IAsyncEnumerable<BluetoothDeviceData> GetValuesAsync(int intervalSeconds = 20)
    {
        while (true)
        {
            await foreach (var device in this.FetchDevices())
            {
                yield return device;
            }
            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
        }
    }

    /// <summary>
    /// Updates devices list appending new and removing disconnected
    /// </summary>
    private async Task ScanDevices()
    {
        // TODO: Availability check
        var pairedDevices = await this.FetchDevices().ToArrayAsync() ?? [];

        var currentIds = this.Devices.Select(x => x.Id);
        var newIds = pairedDevices.Select(x => x.Id);

        this.AddNewDevices(pairedDevices, currentIds);
        this.RemoveUnpairedDevices(newIds);
    }

    private void RemoveUnpairedDevices(IEnumerable<string> newIds)
    {
        var unpairedDevices = this.Devices.Where(x => !newIds.Contains(x.Id)).ToArray();
        foreach (var device in unpairedDevices)
        {
            this.Devices.Remove(device);
        }
    }

    private void AddNewDevices(IEnumerable<BluetoothDeviceData> pairedDevices, IEnumerable<string> currentIds)
    {
        var newDevices = pairedDevices.Where(x => !currentIds.Contains(x.Id));

        foreach (var device in newDevices)
        {
            this.Devices.Add(device);
        }
    }
}
