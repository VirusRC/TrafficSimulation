using System;

namespace RemoteObject
{
  public class RemoteObject : MarshalByRefObject
  {
    #region ### PRIVATE PROPERTIES ###
    private int _callCount;
    #endregion

    #region ### PUBLIC PROPERTIES ###

    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###
    public int GetCount()
    {
      Console.WriteLine("GetCount has been called.");
      _callCount++;
      return _callCount;
    }
    #endregion

    #region ### PUBLIC STATIC INTERFACES ###

    public static string GetRemoteObjectIdentifier()
    {
      return Globals.RemoteObjectIdentifier;
    }

    public static int GetIpcPort()
    {
      return Globals.IpcPort;
    }
    #endregion


  }
}
