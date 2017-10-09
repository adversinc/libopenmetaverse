// Type: Nwc.XmlRpc.XmlRpcDeserializer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcDeserializer : XmlRpcXmlTokens
  {
    private static DateTimeFormatInfo _dateFormat = new DateTimeFormatInfo();
    private object _container;
    private Stack _containerStack;
    protected string _name;
    protected string _text;
    protected object _value;

    static XmlRpcDeserializer()
    {
    }

    public XmlRpcDeserializer()
    {
      this.Reset();
      XmlRpcDeserializer._dateFormat.FullDateTimePattern = "yyyyMMdd\\THH\\:mm\\:ss";
    }

    public virtual object Deserialize(TextReader xmlData)
    {
      return (object) null;
    }

    public object Deserialize(string xmlData)
    {
      return this.Deserialize((TextReader) new StringReader(xmlData));
    }

    protected void DeserializeNode(XmlTextReader reader)
    {
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          if (Logger.Delegate != null)
            Logger.WriteEntry("START " + reader.Name, LogLevel.Information);
          switch (reader.Name)
          {
            case "value":
              this._value = (object) null;
              this._text = (string) null;
              return;
            case "struct":
              this.PushContext();
              this._container = (object) new Hashtable();
              return;
            case "array":
              this.PushContext();
              this._container = (object) new ArrayList();
              return;
            case null:
              return;
            default:
              return;
          }
        case XmlNodeType.Text:
          if (Logger.Delegate != null)
            Logger.WriteEntry("Text " + reader.Value, LogLevel.Information);
          this._text = reader.Value;
          break;
        case XmlNodeType.EndElement:
          if (Logger.Delegate != null)
            Logger.WriteEntry("END " + reader.Name, LogLevel.Information);
          switch (reader.Name)
          {
            case "base64":
              this._value = (object) Convert.FromBase64String(this._text);
              return;
            case "boolean":
              switch (short.Parse(this._text))
              {
                case (short) 0:
                  this._value = (object) false;
                  return;
                case (short) 1:
                  this._value = (object) true;
                  return;
                default:
                  return;
              }
            case "string":
              this._value = (object) this._text;
              return;
            case "double":
              this._value = (object) double.Parse(this._text, (IFormatProvider) CultureInfo.InvariantCulture);
              return;
            case "i4":
            case "int":
							try {
								this._value = (object)int.Parse(this._text);
							} catch(Exception ex) {
								// DINO: one of the bots received an "integer overflow" here. Fixing.
								if(ex is OverflowException) { this._value = (object)int.MaxValue; }
							}
              return;
            case "dateTime.iso8601":
              this._value = (object) DateTime.ParseExact(this._text, "F", (IFormatProvider) XmlRpcDeserializer._dateFormat);
              return;
            case "name":
              this._name = this._text;
              return;
            case "value":
              if (this._value == null)
                this._value = (object) this._text;
              if (this._container == null || !(this._container is IList))
                return;
              ((IList) this._container).Add(this._value);
              return;
            case "member":
              if (this._container == null || !(this._container is IDictionary))
                return;
              ((IDictionary) this._container).Add((object) this._name, this._value);
              return;
            case "array":
            case "struct":
              this._value = this._container;
              this.PopContext();
              return;
            case null:
              return;
            default:
              return;
          }
      }
    }

    private void PopContext()
    {
      Context context = (Context) this._containerStack.Pop();
      this._container = context.Container;
      this._name = context.Name;
    }

    private void PushContext()
    {
      Context context;
      context.Container = this._container;
      context.Name = this._name;
      this._containerStack.Push((object) context);
    }

    protected void Reset()
    {
      this._text = (string) null;
      this._value = (object) null;
      this._name = (string) null;
      this._container = (object) null;
      this._containerStack = new Stack();
    }
  }
}
