using System;
using System.IO;
using System.Text;
using BestHTTP.Authentication;
using BestHTTP.Extensions;

namespace BestHTTP;

public sealed class HTTPProxy : Proxy
{
	public bool IsTransparent { get; set; }

	public bool SendWholeUri { get; set; }

	public bool NonTransparentForHTTPS { get; set; }

	public HTTPProxy(Uri address)
		: this(address, null, isTransparent: false)
	{
	}

	public HTTPProxy(Uri address, Credentials credentials)
		: this(address, credentials, isTransparent: false)
	{
	}

	public HTTPProxy(Uri address, Credentials credentials, bool isTransparent)
		: this(address, credentials, isTransparent, sendWholeUri: true)
	{
	}

	public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri)
		: this(address, credentials, isTransparent, sendWholeUri, nonTransparentForHTTPS: true)
	{
	}

	public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri, bool nonTransparentForHTTPS)
		: base(address, credentials)
	{
		IsTransparent = isTransparent;
		SendWholeUri = sendWholeUri;
		NonTransparentForHTTPS = nonTransparentForHTTPS;
	}

	internal override string GetRequestPath(Uri uri)
	{
		if (!SendWholeUri)
		{
			return uri.GetRequestPathAndQueryURL();
		}
		return uri.OriginalString;
	}

	internal override void Connect(Stream stream, HTTPRequest request)
	{
		bool flag = HTTPProtocolFactory.IsSecureProtocol(request.CurrentUri);
		if (IsTransparent && (!flag || !NonTransparentForHTTPS))
		{
			return;
		}
		using WriteOnlyBufferedStream output = new WriteOnlyBufferedStream(stream, HTTPRequest.UploadChunkSize);
		using BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8);
		bool flag2;
		do
		{
			flag2 = false;
			string text = $"CONNECT {request.CurrentUri.Host}:{request.CurrentUri.Port.ToString()} HTTP/1.1";
			HTTPManager.Logger.Information("HTTPConnection", "Sending " + text);
			binaryWriter.SendAsASCII(text);
			binaryWriter.Write(HTTPRequest.EOL);
			binaryWriter.SendAsASCII("Proxy-Connection: Keep-Alive");
			binaryWriter.Write(HTTPRequest.EOL);
			binaryWriter.SendAsASCII("Connection: Keep-Alive");
			binaryWriter.Write(HTTPRequest.EOL);
			binaryWriter.SendAsASCII($"Host: {request.CurrentUri.Host}:{request.CurrentUri.Port.ToString()}");
			binaryWriter.Write(HTTPRequest.EOL);
			if (base.Credentials != null)
			{
				switch (base.Credentials.Type)
				{
				case AuthenticationTypes.Basic:
					binaryWriter.Write(string.Format("Proxy-Authorization: {0}", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(base.Credentials.UserName + ":" + base.Credentials.Password))).GetASCIIBytes());
					binaryWriter.Write(HTTPRequest.EOL);
					break;
				case AuthenticationTypes.Unknown:
				case AuthenticationTypes.Digest:
				{
					Digest digest = DigestStore.Get(base.Address);
					if (digest == null)
					{
						break;
					}
					string text2 = digest.GenerateResponseHeader(request, base.Credentials, isProxy: true);
					if (!string.IsNullOrEmpty(text2))
					{
						string text3 = $"Proxy-Authorization: {text2}";
						if ((int)HTTPManager.Logger.Level <= 1)
						{
							HTTPManager.Logger.Information("HTTPConnection", "Sending proxy authorization header: " + text3);
						}
						byte[] aSCIIBytes = text3.GetASCIIBytes();
						binaryWriter.Write(aSCIIBytes);
						binaryWriter.Write(HTTPRequest.EOL);
						VariableSizedBufferPool.Release(aSCIIBytes);
					}
					break;
				}
				}
			}
			binaryWriter.Write(HTTPRequest.EOL);
			binaryWriter.Flush();
			request.ProxyResponse = new HTTPResponse(request, stream, isStreamed: false, isFromCache: false);
			if (!request.ProxyResponse.Receive())
			{
				throw new Exception("Connection to the Proxy Server failed!");
			}
			if ((int)HTTPManager.Logger.Level <= 1)
			{
				HTTPManager.Logger.Information("HTTPConnection", "Proxy returned - status code: " + request.ProxyResponse.StatusCode + " message: " + request.ProxyResponse.Message + " Body: " + request.ProxyResponse.DataAsText);
			}
			int statusCode = request.ProxyResponse.StatusCode;
			if (statusCode == 407)
			{
				string text4 = DigestStore.FindBest(request.ProxyResponse.GetHeaderValues("proxy-authenticate"));
				if (!string.IsNullOrEmpty(text4))
				{
					Digest orCreate = DigestStore.GetOrCreate(base.Address);
					orCreate.ParseChallange(text4);
					if (base.Credentials != null && orCreate.IsUriProtected(base.Address) && (!request.HasHeader("Proxy-Authorization") || orCreate.Stale))
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					throw new Exception($"Can't authenticate Proxy! Status Code: \"{request.ProxyResponse.StatusCode}\", Message: \"{request.ProxyResponse.Message}\" and Response: {request.ProxyResponse.DataAsText}");
				}
			}
			else if (!request.ProxyResponse.IsSuccess)
			{
				throw new Exception($"Proxy returned Status Code: \"{request.ProxyResponse.StatusCode}\", Message: \"{request.ProxyResponse.Message}\" and Response: {request.ProxyResponse.DataAsText}");
			}
		}
		while (flag2);
	}
}
