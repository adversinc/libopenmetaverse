// Type: Nwc.XmlRpc.XmlRpcResponseSerializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcResponseSerializer : XmlRpcSerializer
  {
    private static XmlRpcResponseSerializer _singleton;

    public static XmlRpcResponseSerializer Singleton
    {
      get
      {
        if (XmlRpcResponseSerializer._singleton == null)
          XmlRpcResponseSerializer._singleton = new XmlRpcResponseSerializer();
        return XmlRpcResponseSerializer._singleton;
      }
    }

    public override void Serialize(XmlTextWriter output, object obj)
    {
      XmlRpcResponse xmlRpcResponse = (XmlRpcResponse) obj;
      output.WriteStartDocument();
      ((XmlWriter) output).WriteStartElement("methodResponse");
      if (xmlRpcResponse.IsFault)
      {
        ((XmlWriter) output).WriteStartElement("fault");
      }
      else
      {
        ((XmlWriter) output).WriteStartElement("params");
        ((XmlWriter) output).WriteStartElement("param");
      }
      ((XmlWriter) output).WriteStartElement("value");
      this.SerializeObject(output, xmlRpcResponse.Value);
      output.WriteEndElement();
      output.WriteEndElement();
      if (!xmlRpcResponse.IsFault)
        output.WriteEndElement();
      output.WriteEndElement();
    }
  }
}
