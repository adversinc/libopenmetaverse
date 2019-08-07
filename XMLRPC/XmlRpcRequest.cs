// Type: Nwc.XmlRpc.XmlRpcRequest
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Nwc.XmlRpc {
	public class XmlRpcRequest {
		private XmlRpcResponseDeserializer _deserializer;
		private Encoding _encoding;
		private string _methodName;
		protected IList _params;
		private XmlRpcRequestSerializer _serializer;

		private IPAddress bindIPAddress = IPAddress.Any;

		public virtual string MethodName {
			get {
				return this._methodName;
			}
			set {
				this._methodName = value;
			}
		}

		public string MethodNameMethod {
			get {
				int num = this.MethodName.IndexOf(".");
				if (num == -1)
					return this.MethodName;
				else
					return this.MethodName.Substring(num + 1, this.MethodName.Length - num - 1);
			}
		}

		public string MethodNameObject {
			get {
				int length = this.MethodName.IndexOf(".");
				if (length == -1)
					return this.MethodName;
				else
					return this.MethodName.Substring(0, length);
			}
		}

		public virtual IList Params {
			get {
				return this._params;
			}
		}

		public XmlRpcRequest() {
			this._methodName = (string)null;
			this._encoding = (Encoding)new UTF8Encoding();
			this._serializer = new XmlRpcRequestSerializer();
			this._deserializer = new XmlRpcResponseDeserializer();
			this._params = (IList)null;
			this._params = (IList)new ArrayList();
		}

		public XmlRpcRequest(string methodName, IList parameters) {
			this._methodName = (string)null;
			this._encoding = (Encoding)new UTF8Encoding();
			this._serializer = new XmlRpcRequestSerializer();
			this._deserializer = new XmlRpcResponseDeserializer();
			this._params = (IList)null;
			this.MethodName = methodName;
			this._params = parameters;
		}

		public XmlRpcRequest(string methodName, IList parameters, IPAddress bind) {
			this._methodName = (string)null;
			this._encoding = (Encoding)new UTF8Encoding();
			this._serializer = new XmlRpcRequestSerializer();
			this._deserializer = new XmlRpcResponseDeserializer();
			this._params = (IList)null;
			this.MethodName = methodName;
			this._params = parameters;

			this.bindIPAddress = bind;
		}

		public object Invoke(string url) {
			XmlRpcResponse xmlRpcResponse = this.Send(url, 10000);
			if (xmlRpcResponse.IsFault)
				throw new XmlRpcException(xmlRpcResponse.FaultCode, xmlRpcResponse.FaultString);
			else
				return xmlRpcResponse.Value;
		}

		public XmlRpcResponse Send(string url, int timeout) {
			ServicePointManager.CertificatePolicy = (ICertificatePolicy)new AcceptAllCertificatePolicy();

			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			if (httpWebRequest == null) {
				throw new XmlRpcException(-32300, "Transport Layer Error: Could not create request with " + url);
			}

			httpWebRequest.ServicePoint.BindIPEndPointDelegate = delegate(
						ServicePoint servicePoint,
						IPEndPoint remoteEndPoint,
						int retryCount) {

				return new IPEndPoint(bindIPAddress, 0);
			};

			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "text/xml";
			httpWebRequest.AllowWriteStreamBuffering = true;
			httpWebRequest.Timeout = timeout;
			XmlTextWriter output = new XmlTextWriter(((WebRequest)httpWebRequest).GetRequestStream(), this._encoding);
			this._serializer.Serialize(output, (object)this);
			output.Flush();
			output.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			XmlRpcResponse xmlRpcResponse = (XmlRpcResponse)this._deserializer.Deserialize((TextReader)streamReader);
			streamReader.Close();
			httpWebResponse.Close();
			return xmlRpcResponse;
		}

		public override string ToString() {
			return ((XmlRpcSerializer)this._serializer).Serialize((object)this);
		}
	}
}