// <copyright file = "BluetoothDataProviderComposite.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Providers;
public class CompositeBluetoothDataProvider(IEnumerable<IBluetoothDataProvider> dataProviders) : IBluetoothDataProvider
{
    public IEnumerable<IBluetoothDataProvider> DataProviders { get; set; } = dataProviders;

    public IEnumerable<BluetoothDeviceData> FetchDevices() =>
        FetchDevicesAsync().ToArrayAsync()
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
