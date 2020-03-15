// Type: Nwc.XmlRpc.SimpleHttpRequest
// Assembly: XMLRPC, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B0462D50-6E56-4782-B7A8-0305A6ABDF2A
// Assembly location: C:\Program Files (x86)\Radegast\XMLRPC.dll

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace Nwc.XmlRpc
{
  public class SimpleHttpRequest
  {
    private string __filePath;
    private TcpClient _client;
    private string _filePathDir;
    private string _filePathFile;
    private Hashtable _headers;
    private string _httpMethod;
    private StreamReader _input;
    private StreamWriter _output;
    private string _protocol;

    private string _filePath
    {
      get
      {
        return this.__filePath;
      }
      set
      {
        this.__filePath = value;
        this._filePathDir = (string) null;
        this._filePathFile = (string) null;
      }
    }

    public TcpClient Client
    {
      get
      {
        return this._client;
      }
    }

    public string FilePath
    {
      get
      {
        return this._filePath;
      }
    }

    public string FilePathDir
    {
      get
      {
        if (this._filePathDir == null)
        {
          int num = this.FilePath.LastIndexOf("/");
          if (num == -1)
            return "";
          this._filePathDir = this.FilePath.Substring(0, num + 1);
        }
        return this._filePathDir;
      }
    }

    public string FilePathFile
    {
      get
      {
        if (this._filePathFile == null)
        {
          int num = this.FilePath.LastIndexOf("/");
          if (num == -1)
            return "";
          int startIndex = num + 1;
          this._filePathFile = this.FilePath.Substring(startIndex, this.FilePath.Length - startIndex);
        }
        return this._filePathFile;
      }
    }

    public string HttpMethod
    {
      get
      {
        return this._httpMethod;
      }
    }

    public StreamReader Input
    {
      get
      {
        return this._input;
      }
    }

    public StreamWriter Output
    {
      get
      {
        return this._output;
      }
    }

    public string Protocol
    {
      get
      {
        return this._protocol;
      }
    }

    public SimpleHttpRequest(TcpClient client)
    {
      this._client = client;
      this._output = new StreamWriter((Stream) client.GetStream());
      this._input = new StreamReader((Stream) client.GetStream());
      this.GetRequestMethod();
      this.GetRequestHeaders();
    }

    public void Close()
    {
      ((TextWriter) this._output).Flush();
      this._output.Close();
      this._input.Close();
      this._client.Close();
    }

		private void GetRequestHeaders() {
			this._headers = new Hashtable();
			string str1;

			while ((str1 = this._input.ReadLine()) != "" && str1 != null) {
				int length = str1.IndexOf(':');
				if (length == -1 || length == str1.Length - 1) {
					Logger.WriteEntry("Malformed header line: " + str1, LogLevel.Information);
				} else {
					string str2 = str1.Substring(0, length);
					string str3 = str1.Substring(length + 1);
					try {
						this._headers.Add((object)str2, (object)str3);
					}
					catch {
						Logger.WriteEntry("Duplicate header key in line: " + str1, LogLevel.Information);
					}
				}
			}
		}

    private void GetRequestMethod()
    {
      string str1 = this._input.ReadLine();
      if (str1 == null)
        throw new ApplicationException("Void request.");
      if (string.Compare("GET ", str1.Substring(0, 4), true) == 0)
      {
        this._httpMethod = "GET";
      }
      else
      {
        if (string.Compare("POST ", str1.Substring(0, 5), true) != 0)
          throw new InvalidOperationException("Unrecognized method in query: " + str1);
        this._httpMethod = "POST";
      }
      string str2 = str1.TrimEnd(new char[0]);
      int startIndex = str2.IndexOf(' ') + 1;
      if (startIndex >= str2.Length)
        throw new ApplicationException("What do you want?");
      string str3 = str2.Substring(startIndex);
      int num = str3.IndexOf(' ');
      if (num == -1)
        num = str3.Length;
      this._filePath = str3.Substring(0, num).Trim();
      this._protocol = str3.Substring(num).Trim();
    }

    public override string ToString()
    {
      return this.HttpMethod + " " + this.FilePath + " " + this.Protocol;
    }
  }
}
