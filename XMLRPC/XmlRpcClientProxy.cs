// Type: Nwc.XmlRpc.XmlRpcClientProxy
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Nwc.XmlRpc
{
  public class XmlRpcClientProxy : RealProxy
  {
    private XmlRpcRequest _client;
    private string _remoteObjectName;
    private string _url;

    private XmlRpcClientProxy(string remoteObjectName, string url, Type t)
      : base(t)
    {
      this._client = new XmlRpcRequest();
      this._remoteObjectName = remoteObjectName;
      this._url = url;
    }

    public static object createProxy(string remoteObjectName, string url, Type anInterface)
    {
      return new XmlRpcClientProxy(remoteObjectName, url, anInterface).GetTransparentProxy();
    }

    public override IMessage Invoke(IMessage msg)
    {
      IMethodCallMessage mcm = (IMethodCallMessage) msg;
      this._client.MethodName = this._remoteObjectName + "." + mcm.MethodName;
      this._client.Params.Clear();
      foreach (object obj in mcm.Args)
        this._client.Params.Add(obj);
      try
      {
        return (IMessage) new ReturnMessage(this._client.Invoke(this._url), (object[]) null, 0, mcm.LogicalCallContext, mcm);
      }
      catch (Exception ex)
      {
        return (IMessage) new ReturnMessage(ex, mcm);
      }
    }
  }
}
