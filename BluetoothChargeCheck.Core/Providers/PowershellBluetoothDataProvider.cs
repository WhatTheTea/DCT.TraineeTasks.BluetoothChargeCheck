// <copyright file = "PowershellBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;

using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace DCT.BluetoothChargeCheck.Core.Providers;

public enum BluetoothKind
{
    Classic,
    LowEnergy
}

/// <summary>
/// Retrieves data about bluetooth devices from Windows by powershell using PnpDevice module
/// </summary>
public class PowershellBluetoothDataProvider : IBluetoothDataProvider
{
    private const string BluetoothClassicHardwareId = "BTHENUM";
    private const string BluetoothLeHardwareId = "BTHLE";
    /// <summary>
    /// Should be formatted to use <see cref="BluetoothClassicHardwareId"/> or <see cref="BluetoothLeHardwareId"/>
    /// </summary>
    private const string GetDeviceDataScriptTemplate =
        "Get-PnpDevice -Class Bluetooth " +
        "| where HardwareID -like '{0}\\Dev' " + // Bluetooth tech
        "| Get-PnpDeviceProperty -KeyName " +
                "DEVPKEY_Device_InstanceId, " +                  // Id
                "DEVPKEY_Device_FriendlyName, " +                // Name
                "'{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2', " + // Charge
                "'{83DA6326-97A6-4088-9453-A1923F573B29} 15' " + // Connected
        "| group -Property InstanceId";

    
    private InitialSessionState sessionState = InitialSessionState.CreateDefault();
    private string DataScript => this.BluetoothKind switch
    {
        BluetoothKind.Classic => string.Format(GetDeviceDataScriptTemplate, BluetoothClassicHardwareId),
        BluetoothKind.LowEnergy => string.Format(GetDeviceDataScriptTemplate, BluetoothLeHardwareId),
        _ => throw new ArgumentOutOfRangeException(),
    };

    public BluetoothKind BluetoothKind { get; set; }

    public PowershellBluetoothDataProvider()
    {
        this.sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
    }
    /// <summary>
    /// Starts shell script to return data about bluetooth devices <br/>
    /// Method fetches Bluetooth Classic devices by default
    /// </summary>
    public async IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync()
    {
        using var shell = PowerShell.Create(this.sessionState);
        shell.AddScript(this.DataScript);

        foreach (PSObject? obj in await shell.InvokeAsync())
        {
            var group = obj?.Members["Group"]?.Value as Collection<PSObject?> ?? [];
            yield return new BluetoothDeviceData()
            {
                Id = GetPowershellValue(group[0]) as string ?? string.Empty,
                Name = GetPowershellValue(group[1]) as string ?? string.Empty,
                Charge = GetPowershellValue(group[2]) as int? ?? 0,
                Connected = GetPowershellValue(group[3]) as bool? ?? false,
            };
        }
    }

    public IEnumerable<BluetoothDeviceData> FetchDevices() =>
        this.FetchDevicesAsync()
            .ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

    private static object? GetPowershellValue(PSObject? value) =>
        value?.Members["Data"]?.Value;

}
