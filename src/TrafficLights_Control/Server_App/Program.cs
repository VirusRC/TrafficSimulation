using System;
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
      TcpChannel serverChannel = new TcpChannel(RemoteObject.RemoteObject.GetIpcPort());

      // Register the server channel.
#pragma warning disable 618
      ChannelServices.RegisterChannel(serverChannel);
#pragma warning restore 618

      // Expose an object for remote calls.
      RemotingConfiguration.RegisterWellKnownServiceType(
        typeof(RemoteObject.RemoteObject), RemoteObject.RemoteObject.GetRemoteObjectIdentifier(),
        WellKnownObjectMode.Singleton);

      // Wait for the user prompt.
      Console.WriteLine("Press ENTER to exit the server.");
      Console.ReadLine();
    }
  }
}