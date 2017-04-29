using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Timers;

namespace RemoteObject
{
  public class Intersection
  {
    #region ### CONTRUCTOR ###

    /// <summary>
    /// Contructor for intersection with 3 traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="verTrafficLights1"></param>
    /// <param name="greenDurationHorizontal">Optional Parameter, Default value: 5</param>
    /// <param name="greenDurationVertical">Optional Parameter, Default value: 5</param>
    /// <param name="horTrafficLights1"></param>
    /// <param name="horTrafficLights2"></param>
    public Intersection(string uuid, string horTrafficLights1, string horTrafficLights2, string verTrafficLights1, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      Uuid = uuid;

      LstTrafficLights.Add(new TrafficLights(horTrafficLights1, true));
      LstTrafficLights.Add(new TrafficLights(horTrafficLights2, true));
      LstTrafficLights.Add(new TrafficLights(verTrafficLights1, false));

      SetCycleTimes(greenDurationHorizontal, greenDurationVertical);

      StartTrafficLights();
    }

    /// <summary>
    /// Contructor for intersection with 4 traffic lights
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="verTrafficLights2"></param>
    /// <param name="greenDurationHorizontal">Optional Parameter, Default value: 5</param>
    /// <param name="greenDurationVertical">Optional Parameter, Default value: 5</param>
    /// <param name="horTrafficLights1"></param>
    /// <param name="horTrafficLights2"></param>
    /// <param name="verTrafficLights1"></param>
    public Intersection(string uuid, string horTrafficLights1, string horTrafficLights2, string verTrafficLights1, string verTrafficLights2, int greenDurationHorizontal = 5, int greenDurationVertical = 5)
    {
      Uuid = uuid;

      LstTrafficLights.Add(new TrafficLights(horTrafficLights1, true));
      LstTrafficLights.Add(new TrafficLights(horTrafficLights2, true));
      LstTrafficLights.Add(new TrafficLights(verTrafficLights1, false));
      LstTrafficLights.Add(new TrafficLights(verTrafficLights2, false));

      SetCycleTimes(greenDurationHorizontal, greenDurationVertical);

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
    public int GreenDurationHorizontal;

    /// <summary>
    /// Defines the green duration of the vertically aligned traffic lights.
    /// </summary>
    public int GreenDurationVertical;

    /// <summary>
    /// Defines the red duration of the horizontally aligned traffic lights.
    /// Results from "_greenDurationVertical" value.
    /// </summary>
    public int RedDurationHorizontal;

    /// <summary>
    /// Defines the red duration of the vertically aligned traffic lights.
    /// Results from "_greenDurationHorizontal" value.
    /// </summary>
    public int RedDurationVertical;

    /// <summary>
    /// Holds the intersection´s traffic lights.
    /// </summary>
    public List<TrafficLights> LstTrafficLights = new List<TrafficLights>();
    #endregion

    #region ### PUBLIC PROPERTEIS ###

    #endregion

    #region ### PRIVATE METHODS ###
    /// <summary>
    /// Starts the traffic lights simulation for the whole intersection.
    /// </summary>
    private void StartTrafficLights()
    {
      //set initial state of traffic lights on intersection, horizontal starts with green
      foreach (TrafficLights item in this.LstTrafficLights)
      {
        if (item.HorOrVer)
        {
          item.CurrentStatus = Enum.TrafficLightsStatus.Green;

        }
        else
        {
          item.CurrentStatus = Enum.TrafficLightsStatus.Red;
        }

        Thread tmp = new Thread(() => StateMachine.Start(item, this));
        tmp.Start();
      }
    }

    /// <summary>
    /// Calculates the cycle times for the current intersection´s traffic lights.
    /// </summary>
    /// <param name="greenDurationHorizontal"></param>
    /// <param name="greenDurationVertical"></param>
    private void SetCycleTimes(int greenDurationHorizontal, int greenDurationVertical)
    {
      if (greenDurationHorizontal <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationHorizontal = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      if (greenDurationVertical <= TrafficLightsDurations.BlinkGreenDuration)
      {
        greenDurationVertical = TrafficLightsDurations.BlinkGreenDuration + 1;
      }

      GreenDurationHorizontal = greenDurationHorizontal - TrafficLightsDurations.BlinkGreenDuration;
      GreenDurationVertical = greenDurationVertical - TrafficLightsDurations.BlinkGreenDuration;

      RedDurationHorizontal = greenDurationVertical + TrafficLightsDurations.BlinkGreenDuration;
      RedDurationVertical = greenDurationHorizontal + TrafficLightsDurations.BlinkGreenDuration;
    }
    #endregion

    #region ### PUBLIC METHODS ###

    #endregion
  }
}
