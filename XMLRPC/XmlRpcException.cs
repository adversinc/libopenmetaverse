// Type: Nwc.XmlRpc.XmlRpcException
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;

namespace Nwc.XmlRpc
{
  public class XmlRpcException : Exception
  {
    private int _code;

    public int FaultCode
    {
      get
      {
        return this._code;
      }
    }

    public string FaultString
    {
      get
      {
        return this.Message;
      }
    }

    public XmlRpcException(int code, string message)
      : base(message)
    {
      this._code = code;
    }

    public override string ToString()
    {
      return string.Concat(new object[4]
      {
        (object) "Code: ",
        (object) this.FaultCode,
        (object) " Message: ",
        (object) base.ToString()
      });
    }
  }
}
