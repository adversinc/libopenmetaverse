// Type: Nwc.XmlRpc.XmlRpcResponseDeserializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.IO;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcResponseDeserializer : XmlRpcDeserializer
  {
    private static XmlRpcResponseDeserializer _singleton;

    [Obsolete("This object is now thread safe, just use an instance.", false)]
    public static XmlRpcResponseDeserializer Singleton
    {
      get
      {
        if (XmlRpcResponseDeserializer._singleton == null)
          XmlRpcResponseDeserializer._singleton = new XmlRpcResponseDeserializer();
        return XmlRpcResponseDeserializer._singleton;
      }
    }

    public override object Deserialize(TextReader xmlData)
    {
      XmlTextReader reader = new XmlTextReader(xmlData);
      XmlRpcResponse xmlRpcResponse = new XmlRpcResponse();
      bool flag = false;
      lock (this)
      {
        this.Reset();
        while (!flag)
        {
          if (reader.Read())
          {
						try {
							this.DeserializeNode(reader);
						} catch(Exception x) {
							Console.WriteLine(x.ToString());
						}

            if (reader.NodeType == XmlNodeType.EndElement)
            {
              switch (reader.Name)
              {
                case "fault":
                  xmlRpcResponse.Value = this._value;
                  xmlRpcResponse.IsFault = true;
                  continue;
                case "param":
                  xmlRpcResponse.Value = this._value;
                  this._value = (object) null;
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

			return (object) xmlRpcResponse;
    }
  }
}
