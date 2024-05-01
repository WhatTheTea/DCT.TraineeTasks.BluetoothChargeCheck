// <copyright file = "IBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public interface IBluetoothService : INotifyPropertyChanged
{
    ObservableCollection<IBluetoothDevice> Devices { get; }
    ObservableCollection<IBluetoothDevice> Connected { get; }
}