// <copyright file = "Class1.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;

using FluentValidation;

namespace DCT.BluetoothChargeCheck.Validation;

public class BluetoothDataValidator : AbstractValidator<BluetoothDeviceData>
{
    public BluetoothDataValidator(bool enforceConnection = true)
    {
        this.RuleFor(x => x.Id).NotEmpty(); // TODO: InstanceID validation
        this.RuleFor(x => x.Name).NotEmpty();
        this.RuleFor(x => x.Charge).InclusiveBetween(1, 100);
        // must be connected if enforced
        this.RuleFor(x => x.Connected).Must(x => !enforceConnection || x);
    }
}
