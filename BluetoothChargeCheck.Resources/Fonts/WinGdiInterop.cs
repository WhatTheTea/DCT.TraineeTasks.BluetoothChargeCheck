// <copyright file = "WinGdiInterop.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace DCT.BluetoothChargeCheck.Resources.Fonts;

/// <summary>
/// Used to fix missing fonts in Windows 10
/// </summary>
public static partial class WinGdiInterop
{
    /// <summary>
    /// The AddFontResourceW function adds the font resource from the specified file to the system. <br/>
    /// </summary>
    /// <param name="lpFileName">
    /// A pointer to a null-terminated character string that contains a valid font file name.
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value specifies the number of fonts added. <br/>
    /// If the function fails, the return value is zero. No extended error information is available.
    /// </returns>
    [LibraryImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
    public static partial int AddFontResource(
        [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
}
