// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using DCT.BluetoothChargeCheck.Core.Providers;
using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Services;

/// <summary>
/// Service to fetch bluetooth devices with certain interval. FetchDevices property must be set in order to use service <br/>
/// <example>
/// For example:
/// <code><![CDATA[
/// var service = new BluetoothService(dataProvider);
///
/// await foreach (var newDevices in this.deviceService.GetDevicesAsync())
/// { /* do something */ }
/// ]]>
/// </code>
/// </example>
/// </summary>
public class BluetoothService(IBluetoothDataProvider deviceFetcher)
{
    public IBluetoothDataProvider dataProvider { get; } = deviceFetcher;

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(20);

    public IAsyncEnumerable<IEnumerable<BluetoothDeviceData>> GetDevicesAsync() =>
        Observable.Interval(this.UpdateInterval)
            .Prepend(this.UpdateInterval.Ticks) // Prepend value to trigger select immediatly
            .Select(x => this.dataProvider
                .FetchDevicesAsync()
                .ToArrayAsync()
                .AsTask()
                .ToObservable())
            .Concat()
            .ToAsyncEnumerable();
}
