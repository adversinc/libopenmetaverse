// Type: Nwc.XmlRpc.XmlRpcBoxcarRequest
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Collections;

namespace Nwc.XmlRpc
{
  public class XmlRpcBoxcarRequest : XmlRpcRequest
  {
    public IList Requests = (IList) new ArrayList();

    public override string MethodName
    {
      get
      {
        return "system.multiCall";
      }
    }

    public override IList Params
    {
      get
      {
        this._params.Clear();
        ArrayList arrayList = new ArrayList();
        foreach (XmlRpcRequest xmlRpcRequest in (IEnumerable) this.Requests)
          arrayList.Add((object) new Hashtable()
          {
            {
              (object) "methodName",
              (object) xmlRpcRequest.MethodName
            },
            {
              (object) "params",
              (object) xmlRpcRequest.Params
            }
          });
        this._params.Add((object) arrayList);
        return this._params;
      }
    }
  }
}
