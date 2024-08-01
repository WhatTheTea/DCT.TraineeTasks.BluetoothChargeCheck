// <copyright file = "CompositeBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Abstractions;
using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Providers;
/// <summary>
/// Class to use several providers simultaneously
/// </summary>
public class CompositeBluetoothDataProvider(IEnumerable<IBluetoothDataProvider> dataProviders) : IBluetoothDataProvider
{
    public IEnumerable<IBluetoothDataProvider> DataProviders { get; set; } = dataProviders;

    public IEnumerable<BluetoothDeviceData> FetchDevices() =>
        this.FetchDevicesAsync().ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    /// <summary>
    /// Returns all data providers concatenated or an empty sequence, if data providers was empty
    /// </summary>
    public IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync() =>
        this.DataProviders.Aggregate(AsyncEnumerable.Empty<BluetoothDeviceData>(),
            (x, y) => x.Concat(y.FetchDevicesAsync()));
}
