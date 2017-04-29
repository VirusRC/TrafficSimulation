namespace RemoteObject
{
  public class TrafficLights
  {
    #region ### CONSTRUCTOR ###

    /// <summary>
    /// Constructor for a traffic lights
    /// </summary>
    /// <param name="trafficLightsId"></param>
    /// <param name="tlPosition">Indicates where the traffic lights is located on the intersection. True = Hor, False = Ver</param>
    public TrafficLights(string trafficLightsId, bool tlPosition)
    {
      Id = trafficLightsId;
      HorOrVer = tlPosition;
      CurrentStatus = Enum.TrafficLightsStatus.Red;
    }
    #endregion

    #region ### PRIVATE PROPERTEIS ###

    public string Id;

    /// <summary>
    /// True = Hor, False = Ver
    /// </summary>
    public bool HorOrVer;

    #endregion

    #region ### PUBLIC PROPERTEIS ###
    /// <summary>
    /// Holds the current status of the traffic lights.
    /// </summary>
    public Enum.TrafficLightsStatus CurrentStatus;

    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###

    #endregion
  }
}
