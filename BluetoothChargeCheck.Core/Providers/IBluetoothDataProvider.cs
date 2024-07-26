// <copyright file = "IBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Providers;
public interface IBluetoothDataProvider
{
    IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync();
    IEnumerable<BluetoothDeviceData> FetchDevices();
}
