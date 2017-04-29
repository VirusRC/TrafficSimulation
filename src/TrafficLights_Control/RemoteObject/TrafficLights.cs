namespace RemoteObject
{
  class TrafficLights
  {
    #region ### CONSTRUCTOR ###
    /// <summary>
    /// Constructor for a traffic lights
    /// </summary>
    /// <param name="trafficLightsId"></param>
    public TrafficLights(string trafficLightsId)
    {
      Id = trafficLightsId;
      CurrentStatus = Enum.TrafficLightsStatus.Red;
    }
    #endregion

    #region ### PRIVATE PROPERTEIS ###

    public string Id;

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
