using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using DCT.BluetoothChargeCheck.Core.Providers;
using DCT.BluetoothChargeCheck.Core.Services;
using DCT.BluetoothChargeCheck.Models;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using FluentAssertions;

namespace DCT.BluetoothChargeCheck.Tests.Core;

public class BluetoothServiceTests : IDisposable
{
    private const int DeviceDataCount = 5;
    private BluetoothService BluetoothService { get; }
    private IBluetoothDataProvider DataProvider { get; }
    private TestScheduler Scheduler { get; } = new();
    private CancellationTokenSource CancellationTokenSource { get; } = new();
    public BluetoothServiceTests()
    {
        var providerMock = new Mock<IBluetoothDataProvider>();
        providerMock.Setup(x => x.FetchDevicesAsync())
                    .Returns(() => AsyncEnumerable.Repeat(Mock.Of<BluetoothDeviceData>(), DeviceDataCount));

        this.DataProvider = providerMock.Object;
        this.BluetoothService = new BluetoothService(this.DataProvider);
    }

    [Fact]
    public void ServiceFiresOnMethodCall()
    {
        var data = Array.Empty<BluetoothDeviceData>();

        var subscription = this.BluetoothService.GetDevicesAsync(this.Scheduler)
            .ToObservable()
            .ObserveOn(this.Scheduler)
            .Subscribe(x => data = x.ToArray());

        this.Scheduler.Schedule(subscription.Dispose);
        this.Scheduler.Start();
        data.Should().HaveCount(DeviceDataCount);
    }

    [Fact]
    public void ServiceFiresOnSpecifiedInterval()
    {
        int observableCount = 0;
        this.BluetoothService.UpdateInterval = TimeSpan.FromSeconds(2);
 
        var subscription = this.BluetoothService.GetDevicesAsync(this.Scheduler)
            .ToObservable()
            .ObserveOn(this.Scheduler)
            .Subscribe(x => observableCount++);
        this.Scheduler.Schedule(TimeSpan.FromSeconds(2.1), x => subscription.Dispose());
        this.Scheduler.Start();

        observableCount.Should().Be(2);
    }

    public void Dispose() => this.Scheduler.Stop();
}
