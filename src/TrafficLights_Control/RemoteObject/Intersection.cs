using System.Collections.Generic;
using System.ComponentModel;

namespace RemoteObject
{
  class Intersection
  {
    #region ### CONTRUCTOR ###
    /// <summary>
    /// Contructor for intersection with 3 traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    /// <param name="greenDurationHorizontal">Optional Parameter, Default value: 5</param>
    /// <param name="greenDurationVertical">Optional Parameter, Default value: 5</param>
    public Intersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      Uuid = uuid;

      LstTrafficLights.Add(new TrafficLights(trafficLights1));
      LstTrafficLights.Add(new TrafficLights(trafficLights2));
      LstTrafficLights.Add(new TrafficLights(trafficLights3));

      if (greenDurationHorizontal <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationHorizontal = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      if (greenDurationVertical <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationVertical = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      _greenDurationHorizontal = greenDurationHorizontal - TrafficLightsDurations.BlinkGreenDuration;
      _greenDurationVertical = greenDurationVertical - TrafficLightsDurations.BlinkGreenDuration;

      _redDurationHorizontal = greenDurationHorizontal;
      _redDurationVertical = greenDurationVertical;

      StartTrafficLights();
    }

    /// <summary>
    /// Contructor for intersection with 4 traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="trafficLights1"></param>
    /// <param name="trafficLights2"></param>
    /// <param name="trafficLights3"></param>
    /// <param name="trafficLights4"></param>
    /// <param name="greenDurationHorizontal">Optional Parameter, Default value: 5</param>
    /// <param name="greenDurationVertical">Optional Parameter, Default value: 5</param>
    public Intersection(string uuid, string trafficLights1, string trafficLights2, string trafficLights3, string trafficLights4, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      Uuid = uuid;

      LstTrafficLights.Add(new TrafficLights(trafficLights1));
      LstTrafficLights.Add(new TrafficLights(trafficLights2));
      LstTrafficLights.Add(new TrafficLights(trafficLights3));
      LstTrafficLights.Add(new TrafficLights(trafficLights4));

      if (greenDurationHorizontal <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationHorizontal = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      if (greenDurationVertical <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationVertical = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      _greenDurationHorizontal = greenDurationHorizontal - TrafficLightsDurations.BlinkGreenDuration;
      _greenDurationVertical = greenDurationVertical - TrafficLightsDurations.BlinkGreenDuration;

      _redDurationHorizontal = greenDurationHorizontal;
      _redDurationVertical = greenDurationVertical;

      StartTrafficLights();
    }
    #endregion

    #region ### PRIVATE PROPERTEIS ###
    /// <summary>
    /// Unique identifier for intersection.
    /// </summary>
    public string Uuid;

    /// <summary>
    /// Defines the green duration of the horizontally aligned traffic lights.
    /// </summary>
    private int _greenDurationHorizontal;

    /// <summary>
    /// Defines the green duration of the vertically aligned traffic lights.
    /// </summary>
    private int _greenDurationVertical;

    /// <summary>
    /// Defines the red duration of the horizontally aligned traffic lights.
    /// Results from "_greenDurationVertical" value.
    /// </summary>
    private int _redDurationHorizontal;

    /// <summary>
    /// Defines the red duration of the vertically aligned traffic lights.
    /// Results from "_greenDurationHorizontal" value.
    /// </summary>
    private int _redDurationVertical;

    /// <summary>
    /// Holds the intersection´s traffic lights.
    /// </summary>
    public List<TrafficLights> LstTrafficLights = new List<TrafficLights>();

    /// <summary>
    /// Worker which does the simulation of the whole intersection.
    /// </summary>
    private BackgroundWorker _intersectionWorker;
    #endregion

    #region ### PUBLIC PROPERTEIS ###
    
    #endregion

    #region ### PRIVATE METHODS ###
    /// <summary>
    /// Starts the traffic lights simulation for the whole intersection.
    /// </summary>
    private void StartTrafficLights()
    {
      _intersectionWorker = new BackgroundWorker();

      _intersectionWorker.DoWork += StateMachine.Simulate;

      _intersectionWorker.RunWorkerAsync(this);
    }
    #endregion

    #region ### PUBLIC METHODS ###

    #endregion
  }
}
