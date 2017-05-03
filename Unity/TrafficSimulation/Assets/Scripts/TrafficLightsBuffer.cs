using System.Threading;
using Boo.Lang;
using RemoteObject;


namespace Assets.Scripts
{
  /// <summary>
  /// Singleton class which holds the current information of the traffic lights.
  /// </summary>
  public class TrafficLightsBuffer
  {
    private static TrafficLightsBuffer instance;

    public List<myTL> lstTLs = new List<myTL>();

    private TrafficLightsBuffer() { }

    public static TrafficLightsBuffer Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new TrafficLightsBuffer();
        }
        return instance;
      }
    }
  }

  public class myTL
  {
    public myTL(string uuid, string id)
    {
      this.uuid = uuid;
      this.id = id;
    }

    public string uuid;
    public string id;

    public Enum.TrafficLightsStatus status;
  }

}
