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
          //start thread when object is created
          Thread thread = new Thread(updateTLstatus);
          thread.Start();
        }
        return instance;
      }
    }

    public static void updateTLstatus()
    {
      var lala = Simulation.getInstance();
      TrafficLightsBuffer tmp = instance;
      while (true)
      {
        foreach (var item in tmp.lstTLs)
        {
          item.status = lala.getTrafficLightState(item.uuid, item.id);
        }
        Thread.Sleep(200);
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
