// <copyright file = "FluentIcons.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using System.Windows.Media;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Fonts;

public static class FontManager
{
    [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
    public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                     string lpFileName);
}
