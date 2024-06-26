﻿// <copyright file = "SampleDeviceViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

public class SampleDeviceViewModel : DeviceViewModel
{
    private CancellationTokenSource source = new();

    public SampleDeviceViewModel(IBluetoothDevice device) : base(device) =>
        this.PropertyChanging += (_, args) =>
        {
            if (args.PropertyName == nameof(this.IsTrayIconVisible))
            {
                if (this.IsTrayIconVisible)
                {
                    this.source.Cancel();
                }
                else
                {
                    this.source = new CancellationTokenSource();
                    Task.Run(
                        () => (this.BluetoothDevice as SampleBluetoothDevice)!.ChargeCyclingAsync(this.source.Token),
                        this.source.Token);
                }
            }
        };

    public SampleDeviceViewModel() : this(new SampleBluetoothDevice())
    {
    }
}