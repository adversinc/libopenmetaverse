// Type: Nwc.XmlRpc.XmlRpcSerializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcSerializer : XmlRpcXmlTokens
  {
    public string Serialize(object obj)
    {
      using (MemoryStream memoryStream = new MemoryStream(4096))
      {
        XmlTextWriter output = new XmlTextWriter((Stream) memoryStream, (Encoding) null)
        {
          Formatting = Formatting.Indented,
          Indentation = 4
        };
        this.Serialize(output, obj);
        output.Flush();
        byte[] bytes = memoryStream.ToArray();
        string @string = new UTF8Encoding().GetString(bytes, 0, bytes.Length);
        output.Close();
        return @string;
      }
    }

    public virtual void Serialize(XmlTextWriter output, object obj)
    {
    }

    public void SerializeObject(XmlTextWriter output, object obj)
    {
      if (obj == null)
        return;
      if (obj is byte[])
      {
        byte[] buffer = (byte[]) obj;
        ((XmlWriter) output).WriteStartElement("base64");
        output.WriteBase64(buffer, 0, buffer.Length);
        output.WriteEndElement();
      }
      else if (obj is string)
        output.WriteElementString("string", obj.ToString());
      else if (obj is int)
        output.WriteElementString("i4", obj.ToString());
      else if (obj is DateTime)
        output.WriteElementString("dateTime.iso8601", ((DateTime) obj).ToString("yyyyMMdd\\THH\\:mm\\:ss"));
      else if (obj is double)
        output.WriteElementString("double", obj.ToString());
      else if (obj is bool)
        output.WriteElementString("boolean", (bool) obj ? "1" : "0");
      else if (obj is IList)
      {
        ((XmlWriter) output).WriteStartElement("array");
        ((XmlWriter) output).WriteStartElement("data");
        if (((ArrayList) obj).Count > 0)
        {
          foreach (object obj1 in (IEnumerable) obj)
          {
            ((XmlWriter) output).WriteStartElement("value");
            this.SerializeObject(output, obj1);
            output.WriteEndElement();
          }
        }
        output.WriteEndElement();
        output.WriteEndElement();
      }
      else
      {
        if (!(obj is IDictionary))
          return;
        IDictionary dictionary = (IDictionary) obj;
        ((XmlWriter) output).WriteStartElement("struct");
        foreach (string str in (IEnumerable) dictionary.Keys)
        {
          ((XmlWriter) output).WriteStartElement("member");
          output.WriteElementString("name", str);
          ((XmlWriter) output).WriteStartElement("value");
          this.SerializeObject(output, dictionary[(object) str]);
          output.WriteEndElement();
          output.WriteEndElement();
        }
        output.WriteEndElement();
      }
    }
  }
}
