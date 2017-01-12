using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace Srvtools
{
    public class CliSysMegLag
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();

        public static SYS_LANGUAGE GetClientLanguage()
        {
            uint dwlang = GetThreadLocale();
            ushort wlang = (ushort)dwlang;
            ushort wprilangid = (ushort)(wlang & 0x3FF);
            ushort wsublangid = (ushort)(wlang >> 10);

            if (0x09 == wprilangid)
                return SYS_LANGUAGE.ENG;
            else if (0x04 == wprilangid)
            {
                if (0x01 == wsublangid)
                    return SYS_LANGUAGE.TRA;
                else if (0x02 == wsublangid)
                    return SYS_LANGUAGE.SIM;
                else if (0x03 == wsublangid)
                    return SYS_LANGUAGE.HKG;
                else
                    return SYS_LANGUAGE.TRA;
            }
            else if (0x11 == wprilangid)
                return SYS_LANGUAGE.JPN;
            else
                return SYS_LANGUAGE.ENG;
        }
    }
}
