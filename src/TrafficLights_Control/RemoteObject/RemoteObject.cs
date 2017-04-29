using System;
using System.Collections.Generic;

namespace RemoteObject
{
  public class RemoteObject : MarshalByRefObject
  {
    #region ### PRIVATE PROPERTIES ###
    private List<Intersection> _lstIntersection = new List<Intersection>();
    #endregion

    #region ### PUBLIC PROPERTIES ###

    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###
    /// <summary>
    /// Creates a new intersection in the remote object referece with 3 traffic lights.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    public void CreateIntersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3)
    {
      _lstIntersection.Add(new Intersection(uuid, trafficLights1, trafficLights2, trafficLights3));
    }

    /// <summary>
    /// Creates a new intersection in the remote object referece with 4 traffic lights.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    /// <param name="trafficLights4"></param>
    public void CreateIntersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3,
      string trafficLights4)
    {
      _lstIntersection.Add(new Intersection(uuid, trafficLights1, trafficLights2, trafficLights3, trafficLights4));
    }

    #endregion

    #region ### PUBLIC STATIC INTERFACES ###

    public static string GetRemoteObjectIdentifier()
    {
      return Globals.RemoteObjectIdentifier;
    }

    public static int GetIpcPort()
    {
      return Globals.IpcPort;
    }
    #endregion


  }
}
