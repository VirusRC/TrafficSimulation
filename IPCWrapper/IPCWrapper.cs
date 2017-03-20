using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace IPCWrapper
{
    /// <summary>
    /// C# Wrapper Class for the IPC communicatin implemented in a C++ dll
    /// </summary>
    public class IPCWrapper
    {
        #region ### WRAPPER ###
        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int server_InitPipeConfiguration(IntPtr tmpNetworkHostName, IntPtr tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int server_StartPipeServer();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int server_ResetPipe();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int server_RequestClientConnectionID(IntPtr tmpClientConnectionName);

        [DllImport("IPCComponent.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string server_RequestClientData(int tmpClientConnectionID);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int client_InitPipeConfiguration(IntPtr tmpNetworkHostName, IntPtr tmpClientPipeName, IntPtr tmpClientName);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int client_ClientConnectToServerPipe();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int client_ClientSendMessage(IntPtr tmpMessage);
        #endregion

        #region ### INTERFACES ###
        public static int Intf_server_InitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
        {
            return server_InitPipeConfiguration(Helper.StoIPtr(tmpNetworkHostName), Helper.StoIPtr(tmpServerPipeName), tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
        }

        public static int Intf_server_StartPipeServer()
        {
            return server_StartPipeServer();
        }

        public static int Intf_server_ResetPipe()
        {
            return server_ResetPipe();
        }

        public static int Intf_server_RequestClientConnectionID(string tmpClientConnectionName)
        {
            return server_RequestClientConnectionID(Helper.StoIPtr(tmpClientConnectionName));
        }

        public static string Intf_server_RequestClientData(int tmpClientConnectionID)
        {
            return server_RequestClientData(tmpClientConnectionID);
        }

        public static int Intf_client_InitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, string tmpClientName)
        {
            return client_InitPipeConfiguration(Helper.StoIPtr(tmpNetworkHostName), Helper.StoIPtr(tmpServerPipeName), Helper.StoIPtr(tmpClientName));
        }

        public static int Intf_client_ClientConnectToServerPipe()
        {
            return client_ClientConnectToServerPipe();
        }

        public static int Intf_client_ClientSendMessage(string tmpMessage)
        {
            return client_ClientSendMessage(Helper.StoIPtr(tmpMessage));
        }

        #endregion  
    }
}
