using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Fonts
{
    public static class FluentIcons
    {
        public static FontFamily FontFamily
        {
            get
            {
                return new FontFamily(new Uri("pack://application:,,,/Fonts/SegoeFluentIcons.ttf"), "Segoe Fluent Icons");
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                         string lpFileName);
    }
}
