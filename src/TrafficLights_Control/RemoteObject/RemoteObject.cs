using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <param name="horTrafficLights1"></param>
    /// <param name="horTrafficLights2"></param>
    /// <param name="verTrafficLights1"></param>
    /// <param name="greenDurationHorizontal"></param>
    /// <param name="greenDurationVertical"></param>
    public void CreateIntersection(string uuid, string horTrafficLights1, string horTrafficLights2, string verTrafficLights1, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      _lstIntersection.Add(new Intersection(uuid, horTrafficLights1, horTrafficLights2, verTrafficLights1, greenDurationHorizontal, greenDurationVertical));
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
      _lstIntersection.Add(new Intersection(uuid, horTrafficLights1, horTrafficLights2, verTrafficLights1, verTrafficLights2, greenDurationHorizontal, greenDurationVertical));
    }

    /// <summary>
    /// Returns the current status of a specific traffic lights from an specific intersection.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights"></param>
    /// <returns></returns>
    public Enum.TrafficLightsStatus GetStatus(string uuid, string trafficLights)
    {
      try
      {
        Enum.TrafficLightsStatus? currentState = _lstIntersection.Where(item => item.Uuid == uuid)
          .First()?
          .LstTrafficLights.Where(item => item.Id == trafficLights)
          .First()?
          .CurrentStatus;

        if (currentState == null || currentState == Enum.TrafficLightsStatus.Error)
        {
          Console.WriteLine(
            $"Error on getting current status from intersection: {uuid}, traffic lights: {trafficLights}");
          return Enum.TrafficLightsStatus.Error;
        }
        return (Enum.TrafficLightsStatus)currentState;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return Enum.TrafficLightsStatus.Error;
      }
    }

    /// <summary>
    /// Sets the horizontal and vertical green durations for the intersection.
    /// The red durations for the intersection result from these values.
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="greenDurationHorizontal"></param>
    /// <param name="greenDurationVertical"></param>
    public void SetIntersectionDurations(string uuid, int greenDurationHorizontal, int greenDurationVertical)
    {
      try
      {
        var intersection = _lstIntersection.First(item => item.Uuid == uuid);

        if (intersection == null)
        {
          Console.WriteLine($"Intersection with uuid: {uuid} not found.");
          throw new NullReferenceException();
        }

        intersection.SetCycleTimes(greenDurationHorizontal, greenDurationVertical);
      }
      catch (Exception)
      {
        Console.WriteLine("Error on setting intersection´s durations.");
      }
    }

    /// <summary>
    /// Resets the remote object and kills the simulation processes.
    /// </summary>
    public void Reset()
    {
      foreach (var intersectionItem in _lstIntersection)
      {
        foreach (var trafficlightsItem in intersectionItem.LstTrafficLights)
        {
          try
          {
            //aborts the simulation threads for each traffic lights by its reference
            trafficlightsItem.SimulationThread.Abort();
          }
          catch (Exception)
          {
            Console.WriteLine($"Thread with ID {trafficlightsItem.SimulationThread.ManagedThreadId} aborted.");
          }
        }
        intersectionItem.LstTrafficLights.Clear();
      }
      _lstIntersection.Clear();
      GC.Collect();
      GC.WaitForPendingFinalizers();
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
