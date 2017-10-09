// Type: Nwc.XmlRpc.AcceptAllCertificatePolicy
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Nwc.XmlRpc
{
  internal class AcceptAllCertificatePolicy : ICertificatePolicy
  {
    public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
    {
      return true;
    }
  }
}
