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

      tmp.CreateIntersection("1","1", "2", "3", 5, 5);

      int t = 0;

      while (true)
      {
        var status = tmp.GetTrafficLightsStatus("1", "1");
        Console.WriteLine(status.ToString());
        Thread.Sleep(1000);
        t++;

        if (t > 20)
        {
          Console.WriteLine("break");
          break;
        }
      }
      Console.WriteLine("reset");
      tmp.Reset();

      Console.WriteLine("create new intersection");
      tmp.CreateIntersection("1", "1", "2", "3", 5, 5);
      Console.WriteLine("set intersection duration");
      tmp.SetIntersectionDurations("1", 10, 10);

      t = 0;

      Console.WriteLine("next while loop");
      while (true)
      {
        var status = tmp.GetTrafficLightsStatus("1", "1");
        Console.WriteLine($"Hor: {status}");
        status = tmp.GetTrafficLightsStatus("1", "3");
        Console.WriteLine($"Ver: {status}");
        Console.WriteLine();
        Thread.Sleep(1000);
      }

    }
  }
}
