// <copyright file = "FontManager.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace DCT.BluetoothChargeCheck.Fonts;

/// <summary>
/// Used to fix missing fonts in OS
/// </summary>
public static class FontManager
{
    [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
    public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                     string lpFileName);
}
