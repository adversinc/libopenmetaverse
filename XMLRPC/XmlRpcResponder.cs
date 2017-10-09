// Type: Nwc.XmlRpc.XmlRpcResponder
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.IO;
using System.Net.Sockets;
using System.Xml;

namespace Nwc.XmlRpc
{
  public class XmlRpcResponder
  {
    private XmlRpcRequestDeserializer _deserializer = new XmlRpcRequestDeserializer();
    private XmlRpcResponseSerializer _serializer = new XmlRpcResponseSerializer();
    private TcpClient _client;
    private SimpleHttpRequest _httpReq;
    private XmlRpcServer _server;

    public SimpleHttpRequest HttpReq
    {
      get
      {
        return this._httpReq;
      }
    }

    public XmlRpcResponder(XmlRpcServer server, TcpClient client)
    {
      this._server = server;
      this._client = client;
      this._httpReq = new SimpleHttpRequest(this._client);
    }

    ~XmlRpcResponder()
    {
      this.Close();
    }

    public void Close()
    {
      if (this._httpReq != null)
      {
        this._httpReq.Close();
        this._httpReq = (SimpleHttpRequest) null;
      }
      if (this._client == null)
        return;
      this._client.Close();
      this._client = (TcpClient) null;
    }

    public void Respond()
    {
      this.Respond(this.HttpReq);
    }

    public void Respond(SimpleHttpRequest httpReq)
    {
      XmlRpcRequest req = (XmlRpcRequest) this._deserializer.Deserialize((TextReader) httpReq.Input);
      XmlRpcResponse xmlRpcResponse = new XmlRpcResponse();
      try
      {
        xmlRpcResponse.Value = this._server.Invoke(req);
      }
      catch (XmlRpcException ex)
      {
        xmlRpcResponse.SetFault(ex.FaultCode, ex.FaultString);
      }
      catch (Exception ex)
      {
        xmlRpcResponse.SetFault(-32500, "Application Error: " + ex.Message);
      }
      if (Logger.Delegate != null)
        Logger.WriteEntry(xmlRpcResponse.ToString(), LogLevel.Information);
      XmlRpcServer.HttpHeader(httpReq.Protocol, "text/xml", 0L, " 200 OK", (TextWriter) httpReq.Output);
      ((TextWriter) httpReq.Output).Flush();
      XmlTextWriter output = new XmlTextWriter((TextWriter) httpReq.Output);
      this._serializer.Serialize(output, (object) xmlRpcResponse);
      output.Flush();
      ((TextWriter) httpReq.Output).Flush();
    }
  }
}
