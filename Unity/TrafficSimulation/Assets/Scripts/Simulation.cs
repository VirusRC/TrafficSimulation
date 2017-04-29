using Client;
using RemoteObject;

public class Simulation{

    private static Simulation instance;
    private IpcClient ipcClient;

    private string ipAddressServer= "78.104.168.206";

    private Simulation()
    {
        instance = this;
        ipcClient = new IpcClient();
        //ipcClient.StartRemoteConnection(ipAddressServer);
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

    public float getCarSpeed()
    {
        return 10f;
    }

}
