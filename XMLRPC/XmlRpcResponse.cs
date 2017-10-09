// Type: Nwc.XmlRpc.XmlRpcResponse
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Collections;

namespace Nwc.XmlRpc
{
  public class XmlRpcResponse
  {
    private object _value;
    public bool IsFault;

    public int FaultCode
    {
      get
      {
        if (!this.IsFault)
          return 0;
        else
          return (int) ((Hashtable) this._value)[(object) "faultCode"];
      }
    }

    public string FaultString
    {
      get
      {
        if (!this.IsFault)
          return "";
        else
          return (string) ((Hashtable) this._value)[(object) "faultString"];
      }
    }

    public object Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this.IsFault = false;
        this._value = value;
      }
    }

    public XmlRpcResponse()
    {
      this.Value = (object) null;
      this.IsFault = false;
    }

    public XmlRpcResponse(int code, string message)
      : this()
    {
      this.SetFault(code, message);
    }

    public void SetFault(int code, string message)
    {
      this.Value = (object) new Hashtable()
      {
        {
          (object) "faultCode",
          (object) code
        },
        {
          (object) "faultString",
          (object) message
        }
      };
      this.IsFault = true;
    }

    public override string ToString()
    {
      return ((XmlRpcSerializer) XmlRpcResponseSerializer.Singleton).Serialize((object) this);
    }
  }
}
