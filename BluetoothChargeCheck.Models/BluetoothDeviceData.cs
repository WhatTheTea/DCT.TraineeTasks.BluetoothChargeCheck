// <copyright file = "BluetoothDeviceData.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

namespace DCT.BluetoothChargeCheck.Models;

/// <summary>
/// Data that is needed for an application.
/// </summary>
public record BluetoothDeviceData
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required double Charge { get; set; }

    public required bool Connected { get; set; }

}
