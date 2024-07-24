// <copyright file = "ToggleTaskbarIconMessage.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;

using DCT.BluetoothChargeCheck.ViewModels.Device;

namespace DCT.BluetoothChargeCheck.Messages;
/// <summary>
/// Message used to indicate main viewmodel about tray icon visibility updates <br/>
/// That way device viewmodels notify main viewmodel to create new taskbar icon or to dispose existing one
/// </summary>
public class ToggleTaskbarIconMessage(DeviceViewModel value) : ValueChangedMessage<DeviceViewModel>(value);