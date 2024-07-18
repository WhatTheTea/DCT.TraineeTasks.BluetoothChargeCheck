// <copyright file = "IBluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

public interface IBluetoothDevice
{
    string Name { get; }
    string Id { get; }
    double Charge { get; }
    bool Connected { get; }
}