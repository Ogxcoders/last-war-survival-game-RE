using System;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.ServerSentEvents;
using BestHTTP.WebSocket;

namespace BestHTTP;

public static class HTTPProtocolFactory
{
	public static HTTPResponse Get(SupportedProtocols protocol, HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
	{
		return protocol switch
		{
			SupportedProtocols.WebSocket => new WebSocketResponse(request, stream, isStreamed, isFromCache), 
			SupportedProtocols.ServerSentEvents => new EventSourceResponse(request, stream, isStreamed, isFromCache), 
			_ => new HTTPResponse(request, new ReadOnlyBufferedStream(stream), isStreamed, isFromCache), 
		};
	}

	public static SupportedProtocols GetProtocolFromUri(Uri uri)
	{
		if (uri == null || uri.Scheme == null)
		{
			throw new Exception("Malformed URI in GetProtocolFromUri");
		}
		string text = uri.Scheme.ToLowerInvariant();
		if (text == "ws" || text == "wss")
		{
			return SupportedProtocols.WebSocket;
		}
		return SupportedProtocols.HTTP;
	}

	public static bool IsSecureProtocol(Uri uri)
	{
		if (uri == null || uri.Scheme == null)
		{
			throw new Exception("Malformed URI in IsSecureProtocol");
		}
		string text = uri.Scheme.ToLowerInvariant();
		if (text == "https" || text == "wss")
		{
			return true;
		}
		return false;
	}
}
