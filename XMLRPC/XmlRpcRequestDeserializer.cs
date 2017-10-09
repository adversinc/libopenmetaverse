// Type: Nwc.XmlRpc.XmlRpcRequestDeserializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.IO;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcRequestDeserializer : XmlRpcDeserializer
  {
    private static XmlRpcRequestDeserializer _singleton;

    [Obsolete("This object is now thread safe, just use an instance.", false)]
    public static XmlRpcRequestDeserializer Singleton
    {
      get
      {
        if (XmlRpcRequestDeserializer._singleton == null)
          XmlRpcRequestDeserializer._singleton = new XmlRpcRequestDeserializer();
        return XmlRpcRequestDeserializer._singleton;
      }
    }

    public override object Deserialize(TextReader xmlData)
    {
      XmlTextReader reader = new XmlTextReader(xmlData);
      XmlRpcRequest xmlRpcRequest = new XmlRpcRequest();
      bool flag = false;
      lock (this)
      {
        this.Reset();
        while (!flag)
        {
          if (reader.Read())
          {
            this.DeserializeNode(reader);
            if (reader.NodeType == XmlNodeType.EndElement)
            {
              switch (reader.Name)
              {
                case "methodName":
                  xmlRpcRequest.MethodName = this._text;
                  continue;
                case "methodCall":
                  flag = true;
                  continue;
                case "param":
                  xmlRpcRequest.Params.Add(this._value);
                  this._text = (string) null;
                  continue;
                default:
                  continue;
              }
            }
          }
          else
            break;
        }
      }
      return (object) xmlRpcRequest;
    }
  }
}
