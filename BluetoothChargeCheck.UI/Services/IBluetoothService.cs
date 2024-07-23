// <copyright file = "IBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;

using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.Services;

public interface IBluetoothService : INotifyPropertyChanged
{
    ObservableCollection<BluetoothDeviceData> Devices { get; }
}