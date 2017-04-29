using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Enum = RemoteObject.Enum;

namespace Client
{
  public class IpcClient
  {
    #region ### PRIVATE PROPERTIES ###

    #endregion

    #region ### PUBLIC PROPERTIES ###
    private RemoteObject.RemoteObject _myRemoteObject;
    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###
    /// <summary>
    /// Creates the remote object and connects to the server.
    /// </summary>
    /// <param name="hostname">Server´s IP address or "localhost"</param>
    /// <returns></returns>
    public bool StartRemoteConnection(string hostname)
    {
      if (String.IsNullOrEmpty(hostname))
      {
        Console.WriteLine("Invalid hostname.");
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

        // Create an instance of the remote object.
        _myRemoteObject = new RemoteObject.RemoteObject();

        Console.WriteLine("Remoteobject created successfully.");

        return true;
      }
      catch (Exception ex)
      {
        Debug.Print(ex.Message);
        return false;
      }
    }

    /// <summary>
    /// Creates a new intersection in the remote object referece with 3 traffic lights.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    public void CreateIntersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3)
    {
      _myRemoteObject.CreateIntersection(uuid, trafficLights1, trafficLights2, trafficLights3);
    }

    /// <summary>
    /// Creates a new intersection in the remote object referece with 4 traffic lights.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    /// <param name="trafficLights4"></param>
    public void CreateIntersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3, string trafficLights4)
    {
      _myRemoteObject.CreateIntersection(uuid, trafficLights1, trafficLights2, trafficLights3, trafficLights4);
    }

    /// <summary>
    /// Gets the current state of a specific traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLightsId"></param>
    public Enum.TrafficLightsStatus GetTrafficLightsStatus(string uuid, string trafficLightsId)
    {

      return Enum.TrafficLightsStatus.Error;
    }

    /// <summary>
    /// Sets the duration of green and red cycles for a specific intersection.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="greenDuration"></param>
    /// <param name="redDuration"></param>
    public void SetDuration(string uuid, int greenDuration, int redDuration)
    {


    }

    /// <summary>
    /// Sets a specific intersection´s traffic lights to active or inactive.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="onOrOff">True = active (traffic lights work), False = inactive (traffic lights go to blinking yellow state)</param>
    public void SetStatus(string uuid, bool onOrOff)
    {

    }

    /// <summary>
    /// Resets the Remoteobject.
    /// </summary>
    public void Reset()
    {

    }

    #endregion
  }
}
