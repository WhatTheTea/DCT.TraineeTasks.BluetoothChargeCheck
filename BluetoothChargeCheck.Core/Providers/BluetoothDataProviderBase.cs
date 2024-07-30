// <copyright file = "BluetoothDataProviderBase.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Providers;
public abstract class BluetoothDataProviderBase : IBluetoothDataProvider
{
    public virtual IEnumerable<BluetoothDeviceData> FetchDevices() =>
        this.FetchDevicesAsync()
            .ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

    public abstract IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync();

    protected static async Task<bool> CheckBluetoothAvailability()
    {
        var radios = await Windows.Devices.Radios.Radio.GetRadiosAsync();
        var bluetoothAdapter = radios.FirstOrDefault(x => x.Kind == Windows.Devices.Radios.RadioKind.Bluetooth);
        return bluetoothAdapter is not null && bluetoothAdapter.State == Windows.Devices.Radios.RadioState.On;
    }
}
