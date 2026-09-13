using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets;

public class HttpListenerWebSocketContext : WebSocketContext
{
	private HttpListenerContext _context;

	private WebSocket _websocket;

	internal Logger Log => _context.Listener.Log;

	internal Stream Stream => _context.Connection.Stream;

	public override CookieCollection CookieCollection => _context.Request.Cookies;

	public override NameValueCollection Headers => _context.Request.Headers;

	public override string Host => _context.Request.Headers["Host"];

	public override bool IsAuthenticated => _context.User != null;

	public override bool IsLocal => _context.Request.IsLocal;

	public override bool IsSecureConnection => _context.Connection.IsSecure;

	public override bool IsWebSocketRequest => _context.Request.IsWebSocketRequest;

	public override string Origin => _context.Request.Headers["Origin"];

	public override NameValueCollection QueryString => _context.Request.QueryString;

	public override Uri RequestUri => _context.Request.Url;

	public override string SecWebSocketKey => _context.Request.Headers["Sec-WebSocket-Key"];

	public override IEnumerable<string> SecWebSocketProtocols
	{
		get
		{
			string text = _context.Request.Headers["Sec-WebSocket-Protocol"];
			if (text != null)
			{
				string[] array = text.Split(new char[1] { ',' });
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					yield return text2.Trim();
				}
			}
		}
	}

	public override string SecWebSocketVersion => _context.Request.Headers["Sec-WebSocket-Version"];

	public override IPEndPoint ServerEndPoint => _context.Connection.LocalEndPoint;

	public override IPrincipal User => _context.User;

	public override IPEndPoint UserEndPoint => _context.Connection.RemoteEndPoint;

	public override WebSocket WebSocket => _websocket;

	internal HttpListenerWebSocketContext(HttpListenerContext context, string protocol)
	{
		_context = context;
		_websocket = new WebSocket(this, protocol);
	}

	internal void Close()
	{
		_context.Connection.Close(force: true);
	}

	internal void Close(HttpStatusCode code)
	{
		_context.Response.Close(code);
	}

	public override string ToString()
	{
		return _context.Request.ToString();
	}
}
