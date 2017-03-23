using System;
using System.Runtime.InteropServices;


namespace IPCWrapper
{
    /// <summary>
    /// C# Wrapper Class for the IPC communicatin implemented in a C++ dll
    /// </summary>
    public class IPCWrapper
    {
        #region ### WRAPPER ###
        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Server_InitPipeConfiguration(IntPtr tmpNetworkHostName, IntPtr tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Server_StartPipeServer();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Server_ResetPipe();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Server_RequestClientConnectionID(IntPtr tmpClientConnectionName);

        [DllImport("IPCComponent.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string Server_RequestClientData(int tmpClientConnectionID);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Client_InitPipeConfiguration(IntPtr tmpNetworkHostName, IntPtr tmpClientPipeName, IntPtr tmpClientName);

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Client_ClientConnectToServerPipe();

        [DllImport("IPCComponent.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Client_ClientSendMessage(IntPtr tmpMessage);
        #endregion

        #region ### INTERFACES ###
        public static int Intf_server_InitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, int tmpServerPipeMaxInstances, int tmpServerOutBufferSize, int tmpServerInBufferSize)
        {
            return Server_InitPipeConfiguration(Helper.StoIPtr(tmpNetworkHostName), Helper.StoIPtr(tmpServerPipeName), tmpServerPipeMaxInstances, tmpServerOutBufferSize, tmpServerInBufferSize);
        }

        public static int Intf_server_StartPipeServer()
        {
            return Server_StartPipeServer();
        }

        public static int Intf_server_ResetPipe()
        {
            return Server_ResetPipe();
        }

        public static int Intf_server_RequestClientConnectionID(string tmpClientConnectionName)
        {
            return Server_RequestClientConnectionID(Helper.StoIPtr(tmpClientConnectionName));
        }

        public static string Intf_server_RequestClientData(int tmpClientConnectionID)
        {
            return Server_RequestClientData(tmpClientConnectionID);
        }

        public static int Intf_client_InitConfiguration(string tmpNetworkHostName, string tmpServerPipeName, string tmpClientName)
        {
            return Client_InitPipeConfiguration(Helper.StoIPtr(tmpNetworkHostName), Helper.StoIPtr(tmpServerPipeName), Helper.StoIPtr(tmpClientName));
        }

        public static int Intf_client_ClientConnectToServerPipe()
        {
            return Client_ClientConnectToServerPipe();
        }

        public static int Intf_client_ClientSendMessage(string tmpMessage)
        {
            return Client_ClientSendMessage(Helper.StoIPtr(tmpMessage));
        }

        #endregion  
    }
}
