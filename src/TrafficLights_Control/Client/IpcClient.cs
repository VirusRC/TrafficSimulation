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
    /// <param name="horTrafficLights1"></param>
    /// <param name="horTrafficLights2"></param>
    /// <param name="verTrafficLights1"></param>
    /// <param name="greenDurationHorizontal"></param>
    /// <param name="greenDurationVertical"></param>
    public void CreateIntersection(string uuid, string horTrafficLights1, string horTrafficLights2, string verTrafficLights1, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      _myRemoteObject.CreateIntersection(uuid, horTrafficLights1, horTrafficLights2, verTrafficLights1, greenDurationHorizontal, greenDurationVertical);
    }

    /// <summary>
    /// Creates a new intersection in the remote object referece with 4 traffic lights.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="horTrafficLights1"></param>
    /// <param name="horTrafficLights2"></param>
    /// <param name="verTrafficLights1"></param>
    /// <param name="verTrafficLights2"></param>
    /// <param name="greenDurationHorizontal"></param>
    /// <param name="greenDurationVertical"></param>
    public void CreateIntersection(string uuid, string horTrafficLights1, string horTrafficLights2, string verTrafficLights1, string verTrafficLights2, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      _myRemoteObject.CreateIntersection(uuid, horTrafficLights1, horTrafficLights2, verTrafficLights1, verTrafficLights2, greenDurationHorizontal, greenDurationVertical);
    }

    /// <summary>
    /// Gets the current state of a specific traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLightsId"></param>
    public Enum.TrafficLightsStatus GetTrafficLightsStatus(string uuid, string trafficLightsId)
    {
      return _myRemoteObject.GetStatus(uuid, trafficLightsId);
    }

    /// <summary>
    /// Sets the duration of green and red cycles for a specific intersection.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="greenDuration"></param>
    /// <param name="redDuration"></param>
    public void SetDuration(string uuid, int greenDuration, int redDuration)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Sets a specific intersection´s traffic lights to active or inactive.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="onOrOff">True = active (traffic lights work), False = inactive (traffic lights go to blinking yellow state)</param>
    public void SetStatus(string uuid, bool onOrOff)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Resets the Remoteobject.
    /// </summary>
    public void Reset()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
