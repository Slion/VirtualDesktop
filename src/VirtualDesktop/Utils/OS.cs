using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDesktop.Utils
{
    internal class OS
    {
        /// <summary>
        /// Return the OS Build number such as: 22621.2215
        /// </summary>
        /// <returns></returns>
        public static double Build()
        {
            int _osBuild = Environment.OSVersion.Version.Build;
            int _osRevision = int.Parse(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("UBR").ToString());
            double _osId = _osBuild + (_osRevision / 10000d);
            return _osId;
        }
    }
}
