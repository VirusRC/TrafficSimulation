using Client;

namespace Client_TestApp
{
  class ClientTestApp
  {
    public static void Main(string[] args)
    {
      IpcClient tmp = new IpcClient();

      tmp.StartRemoteConnection("localhost");





    }
  }
}
