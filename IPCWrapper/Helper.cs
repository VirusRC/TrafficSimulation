using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IPCWrapper
{
    class Helper
    {
        /// <summary>
        /// Returns given string as IntPtr
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static IntPtr StoIPtr(string tmpString)
        {
            return Marshal.StringToHGlobalAnsi(tmpString);
        }
    }
}
