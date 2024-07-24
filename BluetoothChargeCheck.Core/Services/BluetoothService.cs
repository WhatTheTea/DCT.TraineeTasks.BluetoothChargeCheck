// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Reactive.Linq;

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Services;

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
public partial class BluetoothService(Func<IAsyncEnumerable<BluetoothDeviceData>> deviceFetcher)
{
    /// <summary>
    /// Function must return fetched list of device data
    /// </summary>
    public Func<IAsyncEnumerable<BluetoothDeviceData>> DeviceFetcher = deviceFetcher;

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(10);

    public IAsyncEnumerable<IEnumerable<BluetoothDeviceData>> GetDevicesAsync() =>
        Observable.Interval(this.UpdateInterval)
            .Select(x => Observable.FromAsync(
                () => this.DeviceFetcher().ToArrayAsync()
                                          .AsTask()))
            .Concat()
            .ToAsyncEnumerable();
}
