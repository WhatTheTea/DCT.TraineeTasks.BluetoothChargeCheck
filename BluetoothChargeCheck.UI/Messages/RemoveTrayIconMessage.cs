// <copyright file = "RemoveTrayIconMessage.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
internal class RemoveTrayIconMessage(TrayIconViewModel value) : ValueChangedMessage<TrayIconViewModel>(value);
