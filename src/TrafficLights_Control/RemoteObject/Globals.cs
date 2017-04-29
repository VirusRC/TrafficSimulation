namespace RemoteObject
{
  public class Globals
  {
    public static string RemoteObjectIdentifier = "RemoteObject.rem";
    public static int IpcPort = 12810;
  }

  public static class Enum
  {
    public enum TrafficLightsStatus
    {
      Error = 0,
      Green = 1,
      BlinkGreen = 2,
      Yellow = 3,
      Red = 4,
      RedYellow = 5,
      BlinkYellow = 6
    }
  }

  public static class TrafficLightsDurations
  {
    public static int BlinkGreenDuration = 5;
    public static int YellowDuration = 5;
    public static int RedYellowDuration = 5;
    public static int BlinkYellowDuration = 5;
  }

}
