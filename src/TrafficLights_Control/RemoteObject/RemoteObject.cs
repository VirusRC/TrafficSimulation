using System;

namespace RemoteObject
{
  public class RemoteObject : MarshalByRefObject
  {
    #region ### PRIVATE PROPERTIES ###
    
    #endregion

    #region ### PUBLIC PROPERTIES ###

    #endregion

    #region ### PRIVATE METHODS ###

    #endregion

    #region ### PUBLIC METHODS ###
    
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
