// <copyright file = "TrayIconVisibilityChanged.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;

public class TrayIconVisibilityChanged(DeviceViewModel value) : ValueChangedMessage<DeviceViewModel>(value);