using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeDll
{
    /// <summary>
    /// This project is used to create a DLL for the named and unnamed pipes.
    /// 4 different interfaces should be created for sender and receiver pipes (Client/Server).
    /// Additional interfaces to configure the client/server are necessary.
    /// </summary>
    public class Pipe
    {
        //In the .NET Framework, you implement anonymous pipes by using the AnonymousPipeServerStream and AnonymousPipeClientStream classes.
        //In the .NET Framework, you implement named pipes by using the NamedPipeServerStream and NamedPipeClientStream classes.
        public static byte[] Intf_named_Client()
        {
            return new Byte[255];
        }

        public static byte[] Intf_named_Server()
        {
            return new Byte[255];
        }

        public static byte[] Intf_unnamed_Client()
        {
            return new Byte[255];
        }

        public static byte[] Intf_unnamed_Server()
        {
            return new Byte[255];
        }
    }
}
