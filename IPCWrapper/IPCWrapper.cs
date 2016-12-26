using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IPCWrapper
{
    /// <summary>
    /// C# Wrapper Class for the IPC communicatin implemented in a C++ dll
    /// </summary>
    public class IPCWrapper
    {
        #region ### WRAPPER ###
        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int server_InitPipe(IntPtr tmpNetworkHostName, IntPtr tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);

        #endregion

        #region ### INTERFACES ###
        public static int Intf_server_InitPipe(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
        {
            return server_InitPipe(Helper.StoIPtr(tmpNetworkHostName), Helper.StoIPtr(tmpServerPipeName), tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
        }

        #endregion  

    }
}
