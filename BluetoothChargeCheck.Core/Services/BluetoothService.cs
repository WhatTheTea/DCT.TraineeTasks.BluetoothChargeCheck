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
/// Service to fetch bluetooth devices with certain interval. <see cref="dataProvider"/> property must be set in order to use service <br/>
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
public class BluetoothService(IBluetoothDataProvider bluetoothProvider)
{

    private readonly IBluetoothDataProvider dataProvider = bluetoothProvider;

    private readonly BluetoothDataValidator bluetoothValidator = new(enforceConnection: true);

    /// <summary>
    /// Returns collections of device data on specified interval.<br/>
    /// Runs indefinitely until stopped, can be supplied with scheduler.
    /// </summary>
    public IObservable<IEnumerable<BluetoothDeviceData>> GetDevicesObservable(TimeSpan updateInterval, IScheduler? scheduler = null) =>
        Observable.Interval(updateInterval, scheduler ?? Scheduler.Default)
            // Prepend tick to fire immediately
            .Prepend(updateInterval.Ticks)
            // On each tick get bluetooth data, validate and gather it into array
            .Select(_ => this.dataProvider.FetchDevicesAsync()
                .ToObservable()
                .Where(x => this.bluetoothValidator.Validate(x).IsValid)
                .Aggregate(Array.Empty<BluetoothDeviceData>() as IEnumerable<BluetoothDeviceData>,
                (data, device) => data.Append(device)))
            .Concat()
            .Publish().RefCount();

    public static async Task<bool> CheckBluetoothAvailability()
    {
        var radios = await Windows.Devices.Radios.Radio.GetRadiosAsync();
        var bluetoothAdapter = radios.FirstOrDefault(x => x.Kind == Windows.Devices.Radios.RadioKind.Bluetooth);
        return bluetoothAdapter is not null && bluetoothAdapter.State == Windows.Devices.Radios.RadioState.On;
    }
}
