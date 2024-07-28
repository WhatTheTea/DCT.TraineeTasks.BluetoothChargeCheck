// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using DCT.BluetoothChargeCheck.Core.Providers;
using DCT.BluetoothChargeCheck.Models;
using DCT.BluetoothChargeCheck.Validation;

namespace DCT.BluetoothChargeCheck.Core.Services;

/// <summary>
/// Service to fetch bluetooth devices with certain interval. <see cref="DataProvider"/> property must be set in order to use service <br/>
/// Update interval of 20 seconds is default.
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
    private IBluetoothDataProvider DataProvider { get; } = deviceFetcher;
    private BluetoothDataValidator BluetoothValidator { get; } = new(enforceConnection: true);

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Returns collections of device data on specified interval.<br/>
    /// Runs indefinitely until stopped, can be supplied with scheduler.
    /// </summary>
    public IAsyncEnumerable<IEnumerable<BluetoothDeviceData>> GetDevicesAsync(IScheduler? scheduler = null) =>
        Observable.Interval(this.UpdateInterval, scheduler ?? Scheduler.Default)
            .Prepend(this.UpdateInterval.Ticks) // Prepend value to trigger select immediately
            .Select(_ => this.DataProvider
                .FetchDevicesAsync()
                .ToArrayAsync()
                .AsTask()
                .ToObservable())
            .Concat()
            .Where(x => x.All(device => this.BluetoothValidator.Validate(device).IsValid))
            .ToAsyncEnumerable();
}
