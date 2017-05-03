
using System.Linq;
using Assets.Scripts;
using Client;
using RemoteObject;

public class Simulation
{

  private static Simulation instance;
  private IpcClient ipcClient;

  //private string ipAddressServer= "78.104.168.206";
  private string ipAddressServer = "pwnhofer.at";

  private Simulation()
  {
    instance = this;
    ipcClient = new IpcClient();
    ipcClient.StartRemoteConnection(ipAddressServer);
  }

  public static Simulation getInstance()
  {
    if (instance == null)
    {
      new Simulation();
    }
    return instance;
  }

  public RemoteObject.RemoteObject getRemoteObject()
  {
    return null;
  }

  public void createNewTrafficLight(string uuid, string id1, string id2, string id3, string id4)
  {
    ipcClient.CreateIntersection(uuid, id1, id2, id3, id4);
    TrafficLightsBuffer test = TrafficLightsBuffer.Instance;
    test.lstTLs.Add(new myTL(uuid, id1));
    test.lstTLs.Add(new myTL(uuid, id2));
    test.lstTLs.Add(new myTL(uuid, id3));
    test.lstTLs.Add(new myTL(uuid, id4));
  }

  public void createNewTrafficLight(string uuid, string id1, string id2, string id3)
  {
    ipcClient.CreateIntersection(uuid, id1, id2, id3);
    TrafficLightsBuffer test = TrafficLightsBuffer.Instance;
    test.lstTLs.Add(new myTL(uuid, id1));
    test.lstTLs.Add(new myTL(uuid, id2));
    test.lstTLs.Add(new myTL(uuid, id3));
  }

  public Enum.TrafficLightsStatus getTrafficLightState(string uuid, string id)
  {
    //return ipcClient.GetTrafficLightsStatus(uuid, id);
    TrafficLightsBuffer tmp = TrafficLightsBuffer.Instance;
    var result = tmp.lstTLs.Where(item => item.uuid == uuid && item.id == id).First().status;
    return result;
  }

  public float getCarSpeed()
  {
    return 10f;
  }

}
