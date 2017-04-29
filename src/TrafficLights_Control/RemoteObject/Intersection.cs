using System.Collections.Generic;

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
      _uuid = uuid;

      _lstTrafficLights.Add(new TrafficLights(trafficLights1));
      _lstTrafficLights.Add(new TrafficLights(trafficLights2));
      _lstTrafficLights.Add(new TrafficLights(trafficLights3));

      _greenDurationHorizontal = greenDurationHorizontal;
      _greenDurationVertical = greenDurationVertical;

      _redDurationHorizontal = _greenDurationVertical;
      _redDurationVertical = _greenDurationHorizontal;
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
      _uuid = uuid;

      _lstTrafficLights.Add(new TrafficLights(trafficLights1));
      _lstTrafficLights.Add(new TrafficLights(trafficLights2));
      _lstTrafficLights.Add(new TrafficLights(trafficLights3));
      _lstTrafficLights.Add(new TrafficLights(trafficLights4));

      _greenDurationHorizontal = greenDurationHorizontal;
      _greenDurationVertical = greenDurationVertical;

      _redDurationHorizontal = _greenDurationVertical;
      _redDurationVertical = _greenDurationHorizontal;
    }
    #endregion

    #region ### PRIVATE PROPERTEIS ###
    /// <summary>
    /// Unique identifier for intersection.
    /// </summary>
    private string _uuid;

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
    private List<TrafficLights> _lstTrafficLights = new List<TrafficLights>();
    #endregion

    #region ### PUBLIC PROPERTEIS ###

    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###

    #endregion
  }
}
