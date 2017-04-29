using System;
using System.Threading;
using Client;

namespace Client_TestApp
{
  class ClientTestApp
  {
    public static void Main(string[] args)
    {
      IpcClient tmp = new IpcClient();

      tmp.StartRemoteConnection("localhost");

      tmp.CreateIntersection("1","1", "2", "3", 10, 10);
      while (true)
      {
        var status = tmp.GetTrafficLightsStatus("1", "1");
        Console.WriteLine(status.ToString());
        Thread.Sleep(250);
      }
    }
  }
}
