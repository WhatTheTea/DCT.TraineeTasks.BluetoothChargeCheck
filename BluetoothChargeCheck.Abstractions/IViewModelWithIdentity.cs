// <copyright file = "IViewModelWithIdentity.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

namespace DCT.BluetoothChargeCheck.Abstractions;

/// <summary>
/// This identity is used for taskbar icons to not overlap
/// </summary>
public interface IViewModelWithIdentity
{
    Guid Id { get; }
}
