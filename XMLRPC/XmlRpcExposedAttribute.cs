// Type: Nwc.XmlRpc.XmlRpcExposedAttribute
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Reflection;

namespace Nwc.XmlRpc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public class XmlRpcExposedAttribute : Attribute
  {
    public static bool ExposedMethod(object obj, string methodName)
    {
      Type type = obj.GetType();
      MethodInfo method = type.GetMethod(methodName);
      if (method == null)
        throw new MissingMethodException("Method " + methodName + " not found.");
      if (XmlRpcExposedAttribute.IsExposed((MemberInfo) type))
        return XmlRpcExposedAttribute.IsExposed((MemberInfo) method);
      else
        return true;
    }

    public static bool ExposedObject(object obj)
    {
      return XmlRpcExposedAttribute.IsExposed((MemberInfo) obj.GetType());
    }

    public static bool IsExposed(MemberInfo mi)
    {
      foreach (Attribute attribute in mi.GetCustomAttributes(true))
      {
        if (attribute is XmlRpcExposedAttribute)
          return true;
      }
      return false;
    }
  }
}
