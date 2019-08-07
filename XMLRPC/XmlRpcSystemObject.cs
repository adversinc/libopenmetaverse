// Type: Nwc.XmlRpc.XmlRpcSystemObject
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Collections;
using System.Reflection;

namespace Nwc.XmlRpc
{
  [XmlRpcExposed]
  public class XmlRpcSystemObject
  {
    private static IDictionary _methodHelp = (IDictionary) new Hashtable();
    private XmlRpcServer _server;

    public static IDictionary MethodHelp
    {
      get
      {
        return XmlRpcSystemObject._methodHelp;
      }
    }

    static XmlRpcSystemObject()
    {
    }

    public XmlRpcSystemObject(XmlRpcServer server)
    {
      this._server = server;
      server.Add("system", (object) this);
      XmlRpcSystemObject._methodHelp.Add((object) (this.GetType().FullName + ".methodHelp"), (object) "Return a string description.");
    }

    public static object Invoke(object target, string methodName, IList parameters)
    {
      if (target == null)
        throw new XmlRpcException(-32601, "Server Error, requested method not found: Invalid target object.");
      MethodInfo method = target.GetType().GetMethod(methodName);
      try
      {
        if (!XmlRpcExposedAttribute.ExposedMethod(target, methodName))
          throw new XmlRpcException(-32601, "Server Error, requested method not found: Method " + methodName + " is not exposed.");
      }
      catch (MissingMethodException ex)
      {
        throw new XmlRpcException(-32601, "Server Error, requested method not found: " + ex.Message);
      }
      object[] parameters1 = new object[parameters.Count];
      int index = 0;
      foreach (object obj in (IEnumerable) parameters)
      {
        parameters1[index] = obj;
        ++index;
      }
      try
      {
        object obj = method.Invoke(target, parameters1);
        if (obj == null)
          throw new XmlRpcException(-32500, "Application Error: Method returned NULL.");
        else
          return obj;
      }
      catch (XmlRpcException ex)
      {
        throw ex;
      }
      catch (ArgumentException ex)
      {
        Logger.WriteEntry("Server Error, invalid method parameters: " + ex.Message, LogLevel.Information);
        string str = methodName + "( ";
        foreach (object obj in parameters1)
          str = str + obj.GetType().Name + " ";
        throw new XmlRpcException(-32602, "Server Error, invalid method parameters: Arguement type mismatch invoking " + str + ")");
      }
      catch (TargetParameterCountException ex)
      {
        Logger.WriteEntry("Server Error, invalid method parameters: " + ex.Message, LogLevel.Information);
        throw new XmlRpcException(-32602, "Server Error, invalid method parameters: Arguement count mismatch invoking " + methodName);
      }
      catch (TargetInvocationException ex)
      {
        throw new XmlRpcException(-32500, "Application Error Invoked method " + methodName + ": " + ex.Message);
      }
    }

    [XmlRpcExposed]
    public IList listMethods()
    {
      IList list = (IList) new ArrayList();
      foreach (DictionaryEntry dictionaryEntry in this._server)
      {
        bool flag = XmlRpcExposedAttribute.IsExposed((MemberInfo) dictionaryEntry.Value.GetType());
        foreach (MemberInfo mi in dictionaryEntry.Value.GetType().GetMembers())
        {
          if (mi.MemberType == MemberTypes.Method && ((MethodBase) mi).IsPublic && (!flag || XmlRpcExposedAttribute.IsExposed(mi)))
            list.Add((object) ((string) dictionaryEntry.Key + (object) "." + mi.Name));
        }
      }
      return list;
    }

    [XmlRpcExposed]
    public string methodHelp(string name)
    {
      string str = (string) null;
      try
      {
        str = (string) XmlRpcSystemObject._methodHelp[(object) this._server.MethodName(name)];
      }
      catch (XmlRpcException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
      }
      if (str == null)
        str = "No help available for: " + name;
      return str;
    }

    [XmlRpcExposed]
    public IList methodSignature(string name)
    {
      IList list1 = (IList) new ArrayList();
      int length = name.IndexOf('.');
      if (length >= 0)
      {
        object obj = this._server[name.Substring(0, length)];
        if (obj == null)
          return list1;
        MemberInfo[] member = obj.GetType().GetMember(name.Substring(length + 1));
        if (member != null)
        {
          if (member.Length == 1)
          {
            MethodInfo methodInfo;
            try
            {
              methodInfo = (MethodInfo) member[0];
            }
            catch (Exception ex)
            {
              Logger.WriteEntry(string.Concat(new object[4]
              {
                (object) "Attempted methodSignature call on ",
                (object) member[0],
                (object) " caused: ",
                (object) ex
              }), LogLevel.Information);
              return list1;
            }
            if (!methodInfo.IsPublic)
              return list1;
            IList list2 = (IList) new ArrayList();
            list2.Add((object) methodInfo.ReturnType.Name);
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
              list2.Add((object) parameterInfo.ParameterType.Name);
            list1.Add((object) list2);
            goto label_14;
          }
        }
        return list1;
      }
label_14:
      return list1;
    }

    [XmlRpcExposed]
    public IList multiCall(IList calls)
    {
      IList list1 = (IList) new ArrayList();
      XmlRpcResponse xmlRpcResponse = new XmlRpcResponse();
      foreach (IDictionary dictionary in (IEnumerable) calls)
      {
        try
        {
          object obj = this._server.Invoke(new XmlRpcRequest((string) dictionary[(object) "methodName"], (IList) dictionary[(object) "params"]));
          IList list2 = (IList) new ArrayList();
          list2.Add(obj);
          list1.Add((object) list2);
        }
        catch (XmlRpcException ex)
        {
          xmlRpcResponse.SetFault(ex.FaultCode, ex.FaultString);
          list1.Add(xmlRpcResponse.Value);
        }
        catch (Exception ex)
        {
          xmlRpcResponse.SetFault(-32500, "Application Error: " + ex.Message);
          list1.Add(xmlRpcResponse.Value);
        }
      }
      return list1;
    }
  }
}
