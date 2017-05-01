using System.Threading;

namespace RemoteObject
{
  public class StateMachine
  {
    /// <summary>
    /// State machine logic incl. setting initial state
    /// </summary>
    /// <param name="trafficlights"></param>
    /// <param name="intersection"></param>
    public static void Start(TrafficLights trafficlights, Intersection intersection)
    {
      int t1 = 0;
      int t2 = 0;

      while (true)
      {
        switch (trafficlights.CurrentStatus)
        {
          case Enum.TrafficLightsStatus.Green:
            GetDurationForIntersectionType(trafficlights, intersection, out t1, out t2);
            Thread.Sleep(t1 * TrafficLightsDurations.SecondsMultiplier);
            trafficlights.CurrentStatus = Enum.TrafficLightsStatus.BlinkGreen;
            break;

          case Enum.TrafficLightsStatus.Red:
            GetDurationForIntersectionType(trafficlights, intersection, out t1, out t2);
            Thread.Sleep(t2 * TrafficLightsDurations.SecondsMultiplier);
            trafficlights.CurrentStatus = Enum.TrafficLightsStatus.RedYellow;
            break;

          case Enum.TrafficLightsStatus.BlinkGreen:
            Thread.Sleep(TrafficLightsDurations.BlinkGreenDuration * TrafficLightsDurations.SecondsMultiplier);
            trafficlights.CurrentStatus = Enum.TrafficLightsStatus.Yellow;
            break;

          case Enum.TrafficLightsStatus.Yellow:
            Thread.Sleep(TrafficLightsDurations.YellowDuration * TrafficLightsDurations.SecondsMultiplier);
            trafficlights.CurrentStatus = Enum.TrafficLightsStatus.Red;
            break;

          case Enum.TrafficLightsStatus.RedYellow:
            Thread.Sleep(TrafficLightsDurations.YellowRedDuration * TrafficLightsDurations.SecondsMultiplier);
            trafficlights.CurrentStatus = Enum.TrafficLightsStatus.Green;
            break;
        }
      }
    }

    /// <summary>
    /// Returns the correct durations according to the type of the intersection.
    /// </summary>
    /// <param name="trafficlights"></param>
    /// <param name="intersection"></param>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    private static void GetDurationForIntersectionType(TrafficLights trafficlights, Intersection intersection, out int t1, out int t2)
    {
      if (trafficlights.HorOrVer)
      {
        t1 = intersection.GreenDurationHorizontal;
        t2 = intersection.RedDurationHorizontal;
      }
      else
      {
        t1 = intersection.GreenDurationVertical;
        t2 = intersection.RedDurationVertical;
      }
    }
  }
}
