using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PiApp
{




    /// <summary>
    /// This project will be deployed on a host running ubuntu 16.04.1 xenial.
    /// It will implement the DLL-interfaces from the IPCWrapper project (Server interface for named pipes).
    /// With the use of mono this C# project is used for performing ROS-commands which control the robots.
    /// </summary>
    class PiApplication
    {
        //delegate for async run of server
        private delegate void IPCServerDelegate();

        /// <summary>
        /// Starts the listening process of the server for incoming client connections
        /// </summary>
        static private void startServerAsync()
        {
			try
			{
				IPCWrapper.IPCWrapper.Intf_server_InitConfiguration(".", "testpipename", 3, 255, 255);
				IPCWrapper.IPCWrapper.Intf_server_StartPipeServer();
			}
			catch(Exception ex)
			{
				Debug.Print($"Error setting up server. {ex.Message}");
			}
        }

        static void Main(string[] args)
        {
			try
			{
				IPCServerDelegate serverDelegate = new IPCServerDelegate(startServerAsync);
				serverDelegate.BeginInvoke(null, null);
			}
			catch(Exception ex)
			{
				Debug.Print($"Error starting server. {ex.Message}");
			}


			try
			{
				while(true)
				{
					Debug.Print(IPCWrapper.IPCWrapper.Intf_server_RequestClientData(IPCWrapper.IPCWrapper.Intf_server_RequestClientConnectionID("Is-Position")));
					Thread.Sleep(25);
				}
			}
			catch(Exception ex)
			{
				Debug.Print($"Error getting client sent data. {ex.Message}");
			}

		}
    }
}
