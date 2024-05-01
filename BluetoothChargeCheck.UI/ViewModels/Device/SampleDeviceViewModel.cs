// <copyright file = "TestTrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>


// <copyright file = "TestTrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;

public class SampleDeviceViewModel : DeviceViewModel
{
    private CancellationTokenSource source = new();
    public SampleDeviceViewModel(IBluetoothDevice device) : base(device)
    {
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
                    this.source = new();
                    Task.Run(
                        () => (this.BluetoothDevice as TestBluetoothDevice)!.ChargeCyclingAsync(this.source.Token),
                        this.source.Token);
                }
            }
        };

    }

    public SampleDeviceViewModel() : this(new TestBluetoothDevice())
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.source.Cancel();
            this.source.Dispose();
        }
        base.Dispose(disposing);
    }
}