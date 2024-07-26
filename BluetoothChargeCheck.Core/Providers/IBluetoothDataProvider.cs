// <copyright file = "IBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Core.Providers;
/// <summary>
/// Fetches devices and returns <see cref="BluetoothDeviceData"/> one by one asyncronously or in blocking manner
/// </summary>
public interface IBluetoothDataProvider
{
    IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync();
    IEnumerable<BluetoothDeviceData> FetchDevices();
}
