// <copyright file = "RemoveTrayIconMessage.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;
using DeviceViewModel = DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device.DeviceViewModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
internal class RemoveTrayIconMessage(DeviceViewModel value) : ValueChangedMessage<DeviceViewModel>(value);
