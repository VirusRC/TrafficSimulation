using System;
using System.Runtime.InteropServices;

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
