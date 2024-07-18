// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
internal record BluetoothDevice1 : IBluetoothDevice
{
    public required string Name { get; set; }

    public required string Id { get; set; }

    public required double Charge { get; set; }

    public required bool Connected { get; set; }
}
