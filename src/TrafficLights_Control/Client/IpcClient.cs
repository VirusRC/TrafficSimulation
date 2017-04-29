using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace Client
{
  public class IpcClient
  {
    #region ### PRIVATE PROPERTIES ###

    #endregion

    #region ### PUBLIC PROPERTIES ###
    public RemoteObject.RemoteObject MyRemoteObject;
    #endregion


    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###

    public bool StartRemoteConnection(string hostname)
    {
      if (String.IsNullOrEmpty(hostname))
      {
        return false;
      }
      try
      {
        // Create the channel.
        TcpChannel clientChannel = new TcpChannel();

        // Register the channel.
        ChannelServices.RegisterChannel(clientChannel, false);

        // Register as client for remote object.
        WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
          typeof(RemoteObject.RemoteObject), $"tcp://{hostname}:{RemoteObject.RemoteObject.GetIpcPort()}/{RemoteObject.RemoteObject.GetRemoteObjectIdentifier()}");
        RemotingConfiguration.RegisterWellKnownClientType(remoteType);

        // Create a message sink.
        //string objectUri;
        //System.Runtime.Remoting.Messaging.IMessageSink messageSink =
        //  clientChannel.CreateMessageSink(
        //    $"tcp://{hostname}:{RemoteObject.RemoteObject.GetIpcPort()}/{RemoteObject.RemoteObject.GetRemoteObjectIdentifier()}", null,
        //    out objectUri);

        // Create an instance of the remote object.
        MyRemoteObject = new RemoteObject.RemoteObject();

        // Invoke a method on the remote object.
        MyRemoteObject.GetCount();

        return true;
      }
      catch (Exception ex)
      {
        Debug.Print(ex.Message);
        return false;
      }
    }
    #endregion
  }
}
