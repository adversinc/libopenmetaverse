// Type: Nwc.XmlRpc.XmlRpcServer
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Nwc.XmlRpc
{
  public class XmlRpcServer : IEnumerable
  {
    private const int RESPONDER_COUNT = 10;
    private IPAddress _address;
    private IDictionary _handlers;
    private TcpListener _myListener;
    private int _port;
    private XmlRpcSystemObject _system;
    private WaitCallback _wc;

    public object this[string name]
    {
      get
      {
        return this._handlers[(object) name];
      }
    }

    public XmlRpcServer(int port)
      : this(IPAddress.Any, port)
    {
    }

    public XmlRpcServer(IPAddress address, int port)
    {
      this._port = port;
      this._address = address;
      this._handlers = (IDictionary) new Hashtable();
      this._system = new XmlRpcSystemObject(this);
      this._wc = new WaitCallback(this.WaitCallback);
    }

    public void Add(string name, object obj)
    {
      this._handlers.Add((object) name, obj);
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) this._handlers.GetEnumerator();
    }

    public static void HttpHeader(string sHttpVersion, string sMIMEHeader, long iTotBytes, string sStatusCode, TextWriter output)
    {
      string str1 = "";
      if (sMIMEHeader.Length == 0)
        sMIMEHeader = "text/html";
      string str2 = str1 + sHttpVersion + sStatusCode + "\r\nConnection: close\r\n";
      if (iTotBytes > 0L)
        str2 = string.Concat(new object[4]
        {
          (object) str2,
          (object) "Content-Length: ",
          (object) iTotBytes,
          (object) "\r\n"
        });
      string str3 = str2 + "Server: XmlRpcServer \r\nContent-Type: " + sMIMEHeader + "\r\n\r\n";
      output.Write(str3);
    }

    public object Invoke(XmlRpcRequest req)
    {
      return this.Invoke(req.MethodNameObject, req.MethodNameMethod, req.Params);
    }

    public object Invoke(string objectName, string methodName, IList parameters)
    {
      object target = this._handlers[(object) objectName];
      if (target == null)
        throw new XmlRpcException(-32601, "Server Error, requested method not found: Object " + objectName + " not found");
      else
        return XmlRpcSystemObject.Invoke(target, methodName, parameters);
    }

    public string MethodName(string methodName)
    {
      int length = methodName.LastIndexOf('.');
      if (length == -1)
        throw new XmlRpcException(-32601, "Server Error, requested method not found: Bad method name " + methodName);
      string str = methodName.Substring(0, length);
      object obj = this._handlers[(object) str];
      if (obj == null)
        throw new XmlRpcException(-32601, "Server Error, requested method not found: Object " + str + " not found");
      else
        return obj.GetType().FullName + "." + methodName.Substring(length + 1);
    }

    public void Start()
    {
      try
      {
        this.Stop();
        lock (this)
        {
          this._myListener = new TcpListener(IPAddress.Any, this._port);
          this._myListener.Start();
          new Thread(new ThreadStart(this.StartListen)).Start();
        }
      }
      catch (Exception ex)
      {
        Logger.WriteEntry("An Exception Occurred while Listening :" + ex.ToString(), LogLevel.Error);
      }
    }

    public void StartListen()
    {
      while (this._myListener != null)
        ThreadPool.QueueUserWorkItem(this._wc, (object) new XmlRpcResponder(this, this._myListener.AcceptTcpClient()));
    }

    public void Stop()
    {
      try
      {
        if (this._myListener == null)
          return;
        lock (this)
        {
          this._myListener.Stop();
          this._myListener = (TcpListener) null;
        }
      }
      catch (Exception ex)
      {
        Logger.WriteEntry("An Exception Occurred while stopping :" + ex.ToString(), LogLevel.Error);
      }
    }

    public void WaitCallback(object responder)
    {
      XmlRpcResponder xmlRpcResponder = (XmlRpcResponder) responder;
      if (xmlRpcResponder.HttpReq.HttpMethod == "POST")
      {
        try
        {
          xmlRpcResponder.Respond();
        }
        catch (Exception ex)
        {
          Logger.WriteEntry("Failed on post: " + (object) ex, LogLevel.Error);
        }
      }
      else
        Logger.WriteEntry("Only POST methods are supported: " + xmlRpcResponder.HttpReq.HttpMethod + " ignored", LogLevel.Error);
      xmlRpcResponder.Close();
    }
  }
}
