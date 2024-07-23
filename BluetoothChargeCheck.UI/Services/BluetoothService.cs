// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Services;

/// <summary>
/// Service to fetch bluetooth devices with certain interval. FetchDevices property must be set in order to use service <br/>
/// <example>
/// For example:
/// <code>
/// var dataProvider = GattBluetoothDataProvider.FetchDevicesAsync;
/// var service = new BluetoothService(dataProvider);
/// </code>
/// </example>
/// </summary>
public partial class BluetoothService
{
    /// <summary>
    /// Function must return fetched list of device data
    /// </summary>
    public Func<IAsyncEnumerable<BluetoothDeviceData>> DeviceFetcher;

    public BluetoothService(Func<IAsyncEnumerable<BluetoothDeviceData>> deviceFetcher)
    {
        this.DeviceFetcher = deviceFetcher;
    }

    public async IAsyncEnumerable<IEnumerable<BluetoothDeviceData>> GetDevicesAsync(int intervalSeconds = 20)
    {
        while (true)
        {
            yield return await this.DeviceFetcher().ToArrayAsync();
            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
        }
    }
}
