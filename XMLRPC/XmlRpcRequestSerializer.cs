// Type: Nwc.XmlRpc.XmlRpcRequestSerializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Collections;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcRequestSerializer : XmlRpcSerializer
  {
    private static XmlRpcRequestSerializer _singleton;

    public static XmlRpcRequestSerializer Singleton
    {
      get
      {
        if (XmlRpcRequestSerializer._singleton == null)
          XmlRpcRequestSerializer._singleton = new XmlRpcRequestSerializer();
        return XmlRpcRequestSerializer._singleton;
      }
    }

    public override void Serialize(XmlTextWriter output, object obj)
    {
      XmlRpcRequest xmlRpcRequest = (XmlRpcRequest) obj;
      output.WriteStartDocument();
      ((XmlWriter) output).WriteStartElement("methodCall");
      output.WriteElementString("methodName", xmlRpcRequest.MethodName);
      ((XmlWriter) output).WriteStartElement("params");
      foreach (object obj1 in (IEnumerable) xmlRpcRequest.Params)
      {
        ((XmlWriter) output).WriteStartElement("param");
        ((XmlWriter) output).WriteStartElement("value");
        this.SerializeObject(output, obj1);
        output.WriteEndElement();
        output.WriteEndElement();
      }
      output.WriteEndElement();
      output.WriteEndElement();
    }
  }
}
