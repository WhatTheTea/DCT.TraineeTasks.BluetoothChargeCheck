// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

namespace DCT.BluetoothChargeCheck.Models;

/// <summary>
/// Data that is needed for an application. 
/// </summary>
///
/// <remarks>
/// It is meant to be replaced every device info fetch. <br/>
/// The mechanism is a long polling-like, because even if GATT can provide events to update data - RFCOMM doest not. <br/>
/// Thus, for code simplicity, decision was made to pass data, not GATT services and RFCOMM streams.
/// </remarks>
public record BluetoothDeviceData
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required double Charge { get; set; }

    public required bool Connected { get; set; }

}
