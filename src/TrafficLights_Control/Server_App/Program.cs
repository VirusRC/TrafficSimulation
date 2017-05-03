using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;

namespace Server_App
{
    public class Server
    {
        [SecurityPermission(SecurityAction.Demand)]
        public static void Main(string[] args)
        {
            // Create the server channel.
            BinaryServerFormatterSinkProvider bp = new BinaryServerFormatterSinkProvider();
            ClientIPServerSinkProvider csp = new ClientIPServerSinkProvider();
            csp.Next = bp;
            Hashtable ht = new Hashtable();
            ht.Add("port", RemoteObject.RemoteObject.GetIpcPort());
            TcpChannel serverChannel = new TcpChannel(ht, null, csp);

            // Register the server channel.
#pragma warning disable 618
            ChannelServices.RegisterChannel(serverChannel, false);
#pragma warning restore 618

            // Expose an object for remote calls.
            RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(RemoteObject.RemoteObject), RemoteObject.RemoteObject.GetRemoteObjectIdentifier(),
        WellKnownObjectMode.Singleton);

            // Wait for the user prompt.
            Console.WriteLine("Press ENTER to exit the server.");

            while (true)
            {
                var a = Console.ReadLine();
                if (a.Contains("exit"))
                {
                    try
                    {
                        serverChannel.StopListening(null);
                        ChannelServices.UnregisterChannel(serverChannel);
                        serverChannel = null;

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch (Exception)
                    {
                        Debug.Print("Termination error, no handling required.");
                    }
                    System.Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Enter 'exit' to stop the server.");
                }
            }

        }
    }
}


