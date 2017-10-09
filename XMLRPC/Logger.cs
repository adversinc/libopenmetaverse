// Type: Nwc.XmlRpc.Logger
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

namespace Nwc.XmlRpc
{
  public class Logger
  {
    public static Logger.LoggerDelegate Delegate;

    static Logger()
    {
    }

    public static void WriteEntry(string message, LogLevel level)
    {
      if (Logger.Delegate == null)
        return;
      Logger.Delegate(message, level);
    }

    public delegate void LoggerDelegate(string message, LogLevel level);
  }
}
