using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiApp
{
    /// <summary>
    /// This project will be deployed on a host running ubuntu 16.04.1 xenial.
    /// It will implement the DLL-interfaces from the IPCWrapper project (Server interface for named pipes).
    /// With the use of mono this C# project is used for performing ROS-commands which control the robots.
    /// </summary>
    class PiApplication
    {
        static void Main(string[] args)
        {

            Debug.Print(IPCWrapper.IPCWrapper.Intf_server_InitConfiguration(".", "testpipename", 3 , 255, 255).ToString());
            Debug.Print(IPCWrapper.IPCWrapper.Intf_server_StartPipeServer().ToString());
            //Debug.Print(IPCWrapper.IPCWrapper.Intf_client_InitConfiguration(".", "TestPipeName").ToString());
            //Debug.Print(IPCWrapper.IPCWrapper.Intf_client_ClientConnectToServerPipe().ToString());
            

            Thread.Sleep(5000);
        }
    }
}
