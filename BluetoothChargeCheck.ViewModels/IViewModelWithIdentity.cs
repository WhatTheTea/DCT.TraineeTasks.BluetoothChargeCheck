// <copyright file = "IViewModelWithIdentity.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCT.BluetoothChargeCheck.ViewModels;
public interface IViewModelWithIdentity
{
    Guid Id { get; }
}
