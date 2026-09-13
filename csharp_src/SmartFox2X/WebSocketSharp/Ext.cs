using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;

namespace WebSocketSharp;

public static class Ext
{
	private const string _tspecials = "()<>@,;:\\\"/[]?={} \t";

	private static byte[] compress(this byte[] data)
	{
		if (data.LongLength == 0L)
		{
			return data;
		}
		using MemoryStream stream = new MemoryStream(data);
		return stream.compressToArray();
	}

	private static MemoryStream compress(this Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (stream.Length == 0L)
		{
			return memoryStream;
		}
		stream.Position = 0L;
		using DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, leaveOpen: true);
		stream.CopyTo(deflateStream);
		deflateStream.Close();
		memoryStream.Position = 0L;
		return memoryStream;
	}

	private static byte[] compressToArray(this Stream stream)
	{
		using MemoryStream memoryStream = stream.compress();
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	private static byte[] decompress(this byte[] data)
	{
		if (data.LongLength == 0L)
		{
			return data;
		}
		using MemoryStream stream = new MemoryStream(data);
		return stream.decompressToArray();
	}

	private static MemoryStream decompress(this Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (stream.Length == 0L)
		{
			return memoryStream;
		}
		stream.Position = 0L;
		using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, leaveOpen: true);
		deflateStream.CopyTo(memoryStream);
		memoryStream.Position = 0L;
		return memoryStream;
	}

	private static byte[] decompressToArray(this Stream stream)
	{
		using MemoryStream memoryStream = stream.decompress();
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	private static byte[] readBytes(this Stream stream, byte[] buffer, int offset, int length)
	{
		int i = 0;
		try
		{
			i = stream.Read(buffer, offset, length);
			if (i < 1)
			{
				return buffer.SubArray(0, offset);
			}
			int num;
			for (; i < length; i += num)
			{
				num = stream.Read(buffer, offset + i, length - i);
				if (num < 1)
				{
					break;
				}
			}
		}
		catch
		{
		}
		if (i >= length)
		{
			return buffer;
		}
		return buffer.SubArray(0, offset + i);
	}

	private static bool readBytes(this Stream stream, byte[] buffer, int offset, int length, Stream destination)
	{
		byte[] array = stream.readBytes(buffer, offset, length);
		int num = array.Length;
		destination.Write(array, 0, num);
		return num == offset + length;
	}

	private static void times(this ulong n, Action action)
	{
		for (ulong num = 0uL; num < n; num++)
		{
			action();
		}
	}

	internal static byte[] Append(this ushort code, string reason)
	{
		using MemoryStream memoryStream = new MemoryStream();
		byte[] buffer = code.InternalToByteArray(ByteOrder.Big);
		memoryStream.Write(buffer, 0, 2);
		if (reason != null && reason.Length > 0)
		{
			buffer = Encoding.UTF8.GetBytes(reason);
			memoryStream.Write(buffer, 0, buffer.Length);
		}
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	internal static string CheckIfCanRead(this Stream stream)
	{
		if (stream != null)
		{
			if (stream.CanRead)
			{
				return null;
			}
			return "'stream' cannot be read.";
		}
		return "'stream' is null.";
	}

	internal static string CheckIfClosable(this WebSocketState state)
	{
		return (string)(state switch
		{
			WebSocketState.Closed => "The WebSocket connection has already been closed.", 
			WebSocketState.Closing => "While closing the WebSocket connection.", 
			_ => null, 
		});
	}

	internal static string CheckIfConnectable(this WebSocketState state)
	{
		if (state != WebSocketState.Open && state != WebSocketState.Closing)
		{
			return null;
		}
		return "A WebSocket connection has already been established.";
	}

	internal static string CheckIfOpen(this WebSocketState state)
	{
		return (string)(state switch
		{
			WebSocketState.Closed => "The WebSocket connection has already been closed.", 
			WebSocketState.Closing => "While closing the WebSocket connection.", 
			WebSocketState.Connecting => "A WebSocket connection isn't established.", 
			_ => null, 
		});
	}

	internal static string CheckIfStart(this ServerState state)
	{
		return (string)(state switch
		{
			ServerState.Stop => "The server has already stopped.", 
			ServerState.ShuttingDown => "The server is shutting down.", 
			ServerState.Ready => "The server hasn't yet started.", 
			_ => null, 
		});
	}

	internal static string CheckIfStartable(this ServerState state)
	{
		return (string)(state switch
		{
			ServerState.ShuttingDown => "The server is shutting down.", 
			ServerState.Start => "The server has already started.", 
			_ => null, 
		});
	}

	internal static string CheckIfValidCloseParameters(this ushort code, string reason)
	{
		if (code.IsCloseStatusCode())
		{
			if (!code.IsNoStatusCode() || reason.IsNullOrEmpty())
			{
				if (reason.IsNullOrEmpty() || Encoding.UTF8.GetBytes(reason).Length <= 123)
				{
					return null;
				}
				return "A reason has greater than the allowable max size.";
			}
			return "NoStatusCode cannot have a reason.";
		}
		return "An invalid close status code.";
	}

	internal static string CheckIfValidCloseParameters(this CloseStatusCode code, string reason)
	{
		if (!code.IsNoStatusCode() || reason.IsNullOrEmpty())
		{
			if (reason.IsNullOrEmpty() || Encoding.UTF8.GetBytes(reason).Length <= 123)
			{
				return null;
			}
			return "A reason has greater than the allowable max size.";
		}
		return "NoStatusCode cannot have a reason.";
	}

	internal static string CheckIfValidCloseStatusCode(this ushort code)
	{
		if (code.IsCloseStatusCode())
		{
			return null;
		}
		return "An invalid close status code.";
	}

	internal static string CheckIfValidControlData(this byte[] data, string paramName)
	{
		if (data.Length <= 125)
		{
			return null;
		}
		return "'" + paramName + "' has greater than the allowable max size.";
	}

	internal static string CheckIfValidProtocols(this string[] protocols)
	{
		if (!protocols.Contains((string protocol) => protocol == null || protocol.Length == 0 || !protocol.IsToken()))
		{
			if (!protocols.ContainsTwice())
			{
				return null;
			}
			return "Contains a value twice.";
		}
		return "Contains an invalid value.";
	}

	internal static string CheckIfValidSendData(this byte[] data)
	{
		if (data != null)
		{
			return null;
		}
		return "'data' is null.";
	}

	internal static string CheckIfValidSendData(this FileInfo file)
	{
		if (file != null)
		{
			return null;
		}
		return "'file' is null.";
	}

	internal static string CheckIfValidSendData(this string data)
	{
		if (data != null)
		{
			return null;
		}
		return "'data' is null.";
	}

	internal static string CheckIfValidServicePath(this string path)
	{
		if (path != null && path.Length != 0)
		{
			if (path[0] == '/')
			{
				if (path.IndexOfAny(new char[2] { '?', '#' }) <= -1)
				{
					return null;
				}
				return "'path' includes either or both query and fragment components.";
			}
			return "'path' isn't an absolute path.";
		}
		return "'path' is null or empty.";
	}

	internal static string CheckIfValidSessionID(this string id)
	{
		if (id != null && id.Length != 0)
		{
			return null;
		}
		return "'id' is null or empty.";
	}

	internal static string CheckIfValidWaitTime(this TimeSpan time)
	{
		if (!(time <= TimeSpan.Zero))
		{
			return null;
		}
		return "A wait time is zero or less.";
	}

	internal static void Close(this WebSocketSharp.Net.HttpListenerResponse response, WebSocketSharp.Net.HttpStatusCode code)
	{
		response.StatusCode = (int)code;
		response.OutputStream.Close();
	}

	internal static void CloseWithAuthChallenge(this WebSocketSharp.Net.HttpListenerResponse response, string challenge)
	{
		response.Headers.InternalSet("WWW-Authenticate", challenge, response: true);
		response.Close(WebSocketSharp.Net.HttpStatusCode.Unauthorized);
	}

	internal static byte[] Compress(this byte[] data, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return data;
		}
		return data.compress();
	}

	internal static Stream Compress(this Stream stream, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return stream;
		}
		return stream.compress();
	}

	internal static byte[] CompressToArray(this Stream stream, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return stream.ToByteArray();
		}
		return stream.compressToArray();
	}

	internal static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> condition)
	{
		foreach (T item in source)
		{
			if (condition(item))
			{
				return true;
			}
		}
		return false;
	}

	internal static bool ContainsTwice(this string[] values)
	{
		int len = values.Length;
		Func<int, bool> contains = null;
		contains = delegate(int idx)
		{
			if (idx < len - 1)
			{
				for (int i = idx + 1; i < len; i++)
				{
					if (values[i] == values[idx])
					{
						return true;
					}
				}
				return contains(++idx);
			}
			return false;
		};
		return contains(0);
	}

	internal static T[] Copy<T>(this T[] source, long length)
	{
		T[] array = new T[length];
		Array.Copy(source, 0L, array, 0L, length);
		return array;
	}

	internal static void CopyTo(this Stream source, Stream destination)
	{
		int num = 256;
		byte[] buffer = new byte[num];
		int num2 = 0;
		while ((num2 = source.Read(buffer, 0, num)) > 0)
		{
			destination.Write(buffer, 0, num2);
		}
	}

	internal static byte[] Decompress(this byte[] data, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return data;
		}
		return data.decompress();
	}

	internal static Stream Decompress(this Stream stream, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return stream;
		}
		return stream.decompress();
	}

	internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method)
	{
		if (method != CompressionMethod.Deflate)
		{
			return stream.ToByteArray();
		}
		return stream.decompressToArray();
	}

	internal static bool EqualsWith(this int value, char c, Action<int> action)
	{
		action(value);
		return value == c;
	}

	internal static string GetAbsolutePath(this Uri uri)
	{
		if (uri.IsAbsoluteUri)
		{
			return uri.AbsolutePath;
		}
		string originalString = uri.OriginalString;
		if (originalString[0] != '/')
		{
			return null;
		}
		int num = originalString.IndexOfAny(new char[2] { '?', '#' });
		if (num <= 0)
		{
			return originalString;
		}
		return originalString.Substring(0, num);
	}

	internal static string GetMessage(this CloseStatusCode code)
	{
		return (string)(code switch
		{
			CloseStatusCode.TlsHandshakeFailure => "An error has occurred while handshaking.", 
			CloseStatusCode.ServerError => "WebSocket server got an internal error.", 
			CloseStatusCode.IgnoreExtension => "WebSocket client didn't receive expected extension(s).", 
			CloseStatusCode.TooBig => "A too big data has been received.", 
			CloseStatusCode.PolicyViolation => "A policy violation has occurred.", 
			CloseStatusCode.InconsistentData => "An inconsistent data has been received.", 
			CloseStatusCode.Abnormal => "An exception has occurred.", 
			CloseStatusCode.IncorrectData => "An incorrect data has been received.", 
			CloseStatusCode.ProtocolError => "A WebSocket protocol error has occurred.", 
			_ => string.Empty, 
		});
	}

	internal static string GetName(this string nameAndValue, char separator)
	{
		int num = nameAndValue.IndexOf(separator);
		if (num <= 0)
		{
			return null;
		}
		return nameAndValue.Substring(0, num).Trim();
	}

	internal static string GetValue(this string nameAndValue, char separator)
	{
		int num = nameAndValue.IndexOf(separator);
		if (num <= -1 || num >= nameAndValue.Length - 1)
		{
			return null;
		}
		return nameAndValue.Substring(num + 1).Trim();
	}

	internal static string GetValue(this string nameAndValue, char separator, bool unquote)
	{
		int num = nameAndValue.IndexOf(separator);
		if (num < 0 || num == nameAndValue.Length - 1)
		{
			return null;
		}
		string text = nameAndValue.Substring(num + 1).Trim();
		if (!unquote)
		{
			return text;
		}
		return text.Unquote();
	}

	internal static TcpListenerWebSocketContext GetWebSocketContext(this TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger logger)
	{
		return new TcpListenerWebSocketContext(tcpClient, protocol, secure, sslConfig, logger);
	}

	internal static byte[] InternalToByteArray(this ushort value, ByteOrder order)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		if (!order.IsHostOrder())
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}

	internal static byte[] InternalToByteArray(this ulong value, ByteOrder order)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		if (!order.IsHostOrder())
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}

	internal static bool IsCompressionExtension(this string value, CompressionMethod method)
	{
		return value.StartsWith(method.ToExtensionString());
	}

	internal static bool IsNoStatusCode(this ushort code)
	{
		return code == 1005;
	}

	internal static bool IsNoStatusCode(this CloseStatusCode code)
	{
		return code == CloseStatusCode.NoStatusCode;
	}

	internal static bool IsPortNumber(this int value)
	{
		if (value > 0)
		{
			return value < 65536;
		}
		return false;
	}

	internal static bool IsReserved(this ushort code)
	{
		if (code != 1004 && code != 1005 && code != 1006)
		{
			return code == 1015;
		}
		return true;
	}

	internal static bool IsReserved(this CloseStatusCode code)
	{
		if (code != CloseStatusCode.Undefined && code != CloseStatusCode.NoStatusCode && code != CloseStatusCode.Abnormal)
		{
			return code == CloseStatusCode.TlsHandshakeFailure;
		}
		return true;
	}

	internal static bool IsText(this string value)
	{
		int length = value.Length;
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			if (c < ' ' && !Contains("\r\n\t", c))
			{
				return false;
			}
			switch (c)
			{
			case '\u007f':
				return false;
			case '\n':
				if (++i < length)
				{
					c = value[i];
					if (!Contains(" \t", c))
					{
						return false;
					}
				}
				break;
			}
		}
		return true;
	}

	internal static bool IsToken(this string value)
	{
		foreach (char c in value)
		{
			if (c < ' ' || c >= '\u007f' || Contains("()<>@,;:\\\"/[]?={} \t", c))
			{
				return false;
			}
		}
		return true;
	}

	internal static string Quote(this string value)
	{
		return string.Format("\"{0}\"", value.Replace("\"", "\\\""));
	}

	internal static byte[] ReadBytes(this Stream stream, int length)
	{
		return stream.readBytes(new byte[length], 0, length);
	}

	internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength)
	{
		using MemoryStream memoryStream = new MemoryStream();
		long num = length / bufferLength;
		int num2 = (int)(length % bufferLength);
		byte[] buffer = new byte[bufferLength];
		bool flag = false;
		for (long num3 = 0L; num3 < num; num3++)
		{
			if (!stream.readBytes(buffer, 0, bufferLength, memoryStream))
			{
				flag = true;
				break;
			}
		}
		if (!flag && num2 > 0)
		{
			stream.readBytes(new byte[num2], 0, num2, memoryStream);
		}
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	internal static void ReadBytesAsync(this Stream stream, int length, Action<byte[]> completed, Action<Exception> error)
	{
		byte[] buff = new byte[length];
		stream.BeginRead(buff, 0, length, delegate(IAsyncResult ar)
		{
			try
			{
				byte[] array = null;
				try
				{
					int num = stream.EndRead(ar);
					array = ((num < 1) ? new byte[0] : ((num < length) ? stream.readBytes(buff, num, length - num) : buff));
				}
				catch
				{
					array = new byte[0];
				}
				if (completed != null)
				{
					completed(array);
				}
			}
			catch (Exception obj2)
			{
				if (error != null)
				{
					error(obj2);
				}
			}
		}, null);
	}

	internal static string RemovePrefix(this string value, params string[] prefixes)
	{
		int num = 0;
		foreach (string text in prefixes)
		{
			if (value.StartsWith(text))
			{
				num = text.Length;
				break;
			}
		}
		if (num <= 0)
		{
			return value;
		}
		return value.Substring(num);
	}

	internal static T[] Reverse<T>(this T[] array)
	{
		int num = array.Length;
		T[] array2 = new T[num];
		int num2 = num - 1;
		for (int i = 0; i <= num2; i++)
		{
			array2[i] = array[num2 - i];
		}
		return array2;
	}

	internal static IEnumerable<string> SplitHeaderValue(this string value, params char[] separators)
	{
		int len = value.Length;
		string seps = new string(separators);
		StringBuilder buff = new StringBuilder(32);
		bool escaped = false;
		bool quoted = false;
		for (int i = 0; i < len; i++)
		{
			char c = value[i];
			switch (c)
			{
			case '"':
				if (escaped)
				{
					escaped = !escaped;
				}
				else
				{
					quoted = !quoted;
				}
				break;
			case '\\':
				if (i < len - 1 && value[i + 1] == '"')
				{
					escaped = true;
				}
				break;
			default:
				if (Contains(seps, c) && !quoted)
				{
					yield return buff.ToString();
					buff.Length = 0;
					continue;
				}
				break;
			}
			buff.Append(c);
		}
		if (buff.Length > 0)
		{
			yield return buff.ToString();
		}
	}

	internal static byte[] ToByteArray(this Stream stream)
	{
		using MemoryStream memoryStream = new MemoryStream();
		stream.Position = 0L;
		stream.CopyTo(memoryStream);
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	internal static CompressionMethod ToCompressionMethod(this string value)
	{
		foreach (CompressionMethod value2 in Enum.GetValues(typeof(CompressionMethod)))
		{
			if (value2.ToExtensionString() == value)
			{
				return value2;
			}
		}
		return CompressionMethod.None;
	}

	internal static string ToExtensionString(this CompressionMethod method, params string[] parameters)
	{
		if (method == CompressionMethod.None)
		{
			return string.Empty;
		}
		string text = "permessage-" + method.ToString().ToLower();
		if (parameters == null || parameters.Length == 0)
		{
			return text;
		}
		return string.Format("{0}; {1}", text, parameters.ToString("; "));
	}

	internal static IPAddress ToIPAddress(this string hostNameOrAddress)
	{
		try
		{
			return Dns.GetHostAddresses(hostNameOrAddress)[0];
		}
		catch
		{
			return null;
		}
	}

	internal static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
	{
		return new List<TSource>(source);
	}

	internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder)
	{
		return BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);
	}

	internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder)
	{
		return BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);
	}

	internal static string TrimEndSlash(this string value)
	{
		value = value.TrimEnd(new char[1] { '/' });
		if (value.Length <= 0)
		{
			return "/";
		}
		return value;
	}

	internal static bool TryCreateWebSocketUri(this string uriString, out Uri result, out string message)
	{
		result = null;
		Uri uri = uriString.ToUri();
		if (!uri.IsAbsoluteUri)
		{
			message = "Not an absolute URI: " + uriString;
			return false;
		}
		string scheme = uri.Scheme;
		if (scheme != "ws" && scheme != "wss")
		{
			message = "The scheme part isn't 'ws' or 'wss': " + uriString;
			return false;
		}
		if (uri.Fragment.Length > 0)
		{
			message = "Includes the fragment component: " + uriString;
			return false;
		}
		int port = uri.Port;
		if (port > 0)
		{
			if (port > 65535)
			{
				message = "The port part is greater than 65535: " + uriString;
				return false;
			}
			if ((scheme == "ws" && port == 443) || (scheme == "wss" && port == 80))
			{
				message = "An invalid pair of scheme and port: " + uriString;
				return false;
			}
		}
		else
		{
			uri = new Uri(string.Format("{0}://{1}:{2}{3}", scheme, uri.Host, (scheme == "ws") ? 80 : 443, uri.PathAndQuery));
		}
		result = uri;
		message = string.Empty;
		return true;
	}

	internal static string Unquote(this string value)
	{
		int num = value.IndexOf('"');
		if (num < 0)
		{
			return value;
		}
		int num2 = value.LastIndexOf('"') - num - 1;
		if (num2 >= 0)
		{
			if (num2 != 0)
			{
				return value.Substring(num + 1, num2).Replace("\\\"", "\"");
			}
			return string.Empty;
		}
		return value;
	}

	internal static void WriteBytes(this Stream stream, byte[] bytes)
	{
		using MemoryStream memoryStream = new MemoryStream(bytes);
		memoryStream.CopyTo(stream);
	}

	public static bool Contains(this string value, params char[] chars)
	{
		if (chars != null && chars.Length != 0)
		{
			if (value != null && value.Length != 0)
			{
				return value.IndexOfAny(chars) > -1;
			}
			return false;
		}
		return true;
	}

	public static bool Contains(this NameValueCollection collection, string name)
	{
		if (collection != null && collection.Count > 0)
		{
			return collection[name] != null;
		}
		return false;
	}

	public static bool Contains(this NameValueCollection collection, string name, string value)
	{
		if (collection == null || collection.Count == 0)
		{
			return false;
		}
		string text = collection[name];
		if (text == null)
		{
			return false;
		}
		string[] array = text.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Trim().Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static void Emit(this EventHandler eventHandler, object sender, EventArgs e)
	{
		eventHandler?.Invoke(sender, e);
	}

	public static void Emit<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs
	{
		eventHandler?.Invoke(sender, e);
	}

	public static WebSocketSharp.Net.CookieCollection GetCookies(this NameValueCollection headers, bool response)
	{
		string name = (response ? "Set-Cookie" : "Cookie");
		if (headers == null || !headers.Contains(name))
		{
			return new WebSocketSharp.Net.CookieCollection();
		}
		return WebSocketSharp.Net.CookieCollection.Parse(headers[name], response);
	}

	public static string GetDescription(this WebSocketSharp.Net.HttpStatusCode code)
	{
		return ((int)code).GetStatusDescription();
	}

	public static string GetStatusDescription(this int code)
	{
		return code switch
		{
			100 => "Continue", 
			101 => "Switching Protocols", 
			102 => "Processing", 
			200 => "OK", 
			201 => "Created", 
			202 => "Accepted", 
			203 => "Non-Authoritative Information", 
			204 => "No Content", 
			205 => "Reset Content", 
			206 => "Partial Content", 
			207 => "Multi-Status", 
			300 => "Multiple Choices", 
			301 => "Moved Permanently", 
			302 => "Found", 
			303 => "See Other", 
			304 => "Not Modified", 
			305 => "Use Proxy", 
			307 => "Temporary Redirect", 
			400 => "Bad Request", 
			401 => "Unauthorized", 
			402 => "Payment Required", 
			403 => "Forbidden", 
			404 => "Not Found", 
			405 => "Method Not Allowed", 
			406 => "Not Acceptable", 
			407 => "Proxy Authentication Required", 
			408 => "Request Timeout", 
			409 => "Conflict", 
			410 => "Gone", 
			411 => "Length Required", 
			412 => "Precondition Failed", 
			413 => "Request Entity Too Large", 
			414 => "Request-Uri Too Long", 
			415 => "Unsupported Media Type", 
			416 => "Requested Range Not Satisfiable", 
			417 => "Expectation Failed", 
			422 => "Unprocessable Entity", 
			423 => "Locked", 
			424 => "Failed Dependency", 
			500 => "Internal Server Error", 
			501 => "Not Implemented", 
			502 => "Bad Gateway", 
			503 => "Service Unavailable", 
			504 => "Gateway Timeout", 
			505 => "Http Version Not Supported", 
			507 => "Insufficient Storage", 
			_ => string.Empty, 
		};
	}

	public static bool IsCloseStatusCode(this ushort value)
	{
		if (value > 999)
		{
			return value < 5000;
		}
		return false;
	}

	public static bool IsEnclosedIn(this string value, char c)
	{
		if (value != null && value.Length > 1 && value[0] == c)
		{
			return value[value.Length - 1] == c;
		}
		return false;
	}

	public static bool IsHostOrder(this ByteOrder order)
	{
		return BitConverter.IsLittleEndian == (order == ByteOrder.Little);
	}

	public static bool IsLocal(this IPAddress address)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (address.Equals(IPAddress.Any) || IPAddress.IsLoopback(address))
		{
			return true;
		}
		IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
		foreach (IPAddress obj in hostAddresses)
		{
			if (address.Equals(obj))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsNullOrEmpty(this string value)
	{
		if (value != null)
		{
			return value.Length == 0;
		}
		return true;
	}

	public static bool IsPredefinedScheme(this string value)
	{
		if (value == null || value.Length < 2)
		{
			return false;
		}
		char c = value[0];
		switch (c)
		{
		case 'h':
			if (!(value == "http"))
			{
				return value == "https";
			}
			return true;
		case 'w':
			if (!(value == "ws"))
			{
				return value == "wss";
			}
			return true;
		case 'f':
			if (!(value == "file"))
			{
				return value == "ftp";
			}
			return true;
		case 'n':
			c = value[1];
			if (c == 'e')
			{
				if (!(value == "news") && !(value == "net.pipe"))
				{
					return value == "net.tcp";
				}
				return true;
			}
			return value == "nntp";
		case 'g':
			if (value == "gopher")
			{
				return true;
			}
			goto default;
		default:
			if (c == 'm')
			{
				return value == "mailto";
			}
			return false;
		}
	}

	public static bool IsUpgradeTo(this WebSocketSharp.Net.HttpListenerRequest request, string protocol)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (protocol == null)
		{
			throw new ArgumentNullException("protocol");
		}
		if (protocol.Length == 0)
		{
			throw new ArgumentException("An empty string.", "protocol");
		}
		if (request.Headers.Contains("Upgrade", protocol))
		{
			return request.Headers.Contains("Connection", "Upgrade");
		}
		return false;
	}

	public static bool MaybeUri(this string value)
	{
		if (value == null || value.Length == 0)
		{
			return false;
		}
		int num = value.IndexOf(':');
		if (num == -1)
		{
			return false;
		}
		if (num >= 10)
		{
			return false;
		}
		return value.Substring(0, num).IsPredefinedScheme();
	}

	public static T[] SubArray<T>(this T[] array, int startIndex, int length)
	{
		int num;
		if (array == null || (num = array.Length) == 0)
		{
			return new T[0];
		}
		if (startIndex < 0 || length <= 0 || startIndex + length > num)
		{
			return new T[0];
		}
		if (startIndex == 0 && length == num)
		{
			return array;
		}
		T[] array2 = new T[length];
		Array.Copy(array, startIndex, array2, 0, length);
		return array2;
	}

	public static T[] SubArray<T>(this T[] array, long startIndex, long length)
	{
		long num;
		if (array == null || (num = array.LongLength) == 0L)
		{
			return new T[0];
		}
		if (startIndex < 0 || length <= 0 || startIndex + length > num)
		{
			return new T[0];
		}
		if (startIndex == 0L && length == num)
		{
			return array;
		}
		T[] array2 = new T[length];
		Array.Copy(array, startIndex, array2, 0L, length);
		return array2;
	}

	public static void Times(this int n, Action action)
	{
		if (n > 0 && action != null)
		{
			((ulong)n).times(action);
		}
	}

	public static void Times(this long n, Action action)
	{
		if (n > 0 && action != null)
		{
			((ulong)n).times(action);
		}
	}

	public static void Times(this uint n, Action action)
	{
		if (n != 0 && action != null)
		{
			times(n, action);
		}
	}

	public static void Times(this ulong n, Action action)
	{
		if (n != 0L && action != null)
		{
			n.times(action);
		}
	}

	public static void Times(this int n, Action<int> action)
	{
		if (n > 0 && action != null)
		{
			for (int i = 0; i < n; i++)
			{
				action(i);
			}
		}
	}

	public static void Times(this long n, Action<long> action)
	{
		if (n > 0 && action != null)
		{
			for (long num = 0L; num < n; num++)
			{
				action(num);
			}
		}
	}

	public static void Times(this uint n, Action<uint> action)
	{
		if (n != 0 && action != null)
		{
			for (uint num = 0u; num < n; num++)
			{
				action(num);
			}
		}
	}

	public static void Times(this ulong n, Action<ulong> action)
	{
		if (n != 0L && action != null)
		{
			for (ulong num = 0uL; num < n; num++)
			{
				action(num);
			}
		}
	}

	public static T To<T>(this byte[] source, ByteOrder sourceOrder) where T : struct
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Length == 0)
		{
			return default(T);
		}
		Type typeFromHandle = typeof(T);
		byte[] value = source.ToHostOrder(sourceOrder);
		if (!(typeFromHandle == typeof(bool)))
		{
			if (!(typeFromHandle == typeof(char)))
			{
				if (!(typeFromHandle == typeof(double)))
				{
					if (!(typeFromHandle == typeof(short)))
					{
						if (!(typeFromHandle == typeof(int)))
						{
							if (!(typeFromHandle == typeof(long)))
							{
								if (!(typeFromHandle == typeof(float)))
								{
									if (!(typeFromHandle == typeof(ushort)))
									{
										if (!(typeFromHandle == typeof(uint)))
										{
											if (!(typeFromHandle == typeof(ulong)))
											{
												return default(T);
											}
											return (T)(object)BitConverter.ToUInt64(value, 0);
										}
										return (T)(object)BitConverter.ToUInt32(value, 0);
									}
									return (T)(object)BitConverter.ToUInt16(value, 0);
								}
								return (T)(object)BitConverter.ToSingle(value, 0);
							}
							return (T)(object)BitConverter.ToInt64(value, 0);
						}
						return (T)(object)BitConverter.ToInt32(value, 0);
					}
					return (T)(object)BitConverter.ToInt16(value, 0);
				}
				return (T)(object)BitConverter.ToDouble(value, 0);
			}
			return (T)(object)BitConverter.ToChar(value, 0);
		}
		return (T)(object)BitConverter.ToBoolean(value, 0);
	}

	public static byte[] ToByteArray<T>(this T value, ByteOrder order) where T : struct
	{
		Type typeFromHandle = typeof(T);
		byte[] array = ((typeFromHandle == typeof(bool)) ? BitConverter.GetBytes((bool)(object)value) : ((!(typeFromHandle != typeof(byte))) ? new byte[1] { (byte)(object)value } : ((typeFromHandle == typeof(char)) ? BitConverter.GetBytes((char)(object)value) : ((typeFromHandle == typeof(double)) ? BitConverter.GetBytes((double)(object)value) : ((typeFromHandle == typeof(short)) ? BitConverter.GetBytes((short)(object)value) : ((typeFromHandle == typeof(int)) ? BitConverter.GetBytes((int)(object)value) : ((typeFromHandle == typeof(long)) ? BitConverter.GetBytes((long)(object)value) : ((typeFromHandle == typeof(float)) ? BitConverter.GetBytes((float)(object)value) : ((typeFromHandle == typeof(ushort)) ? BitConverter.GetBytes((ushort)(object)value) : ((typeFromHandle == typeof(uint)) ? BitConverter.GetBytes((uint)(object)value) : ((typeFromHandle == typeof(ulong)) ? BitConverter.GetBytes((ulong)(object)value) : new byte[0])))))))))));
		if (array.Length > 1 && !order.IsHostOrder())
		{
			Array.Reverse((Array)array);
		}
		return array;
	}

	public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Length <= 1 || sourceOrder.IsHostOrder())
		{
			return source;
		}
		return source.Reverse();
	}

	public static string ToString<T>(this T[] array, string separator)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		int num = array.Length;
		if (num == 0)
		{
			return string.Empty;
		}
		if (separator == null)
		{
			separator = string.Empty;
		}
		StringBuilder buff = new StringBuilder(64);
		(num - 1).Times(delegate(int i)
		{
			buff.AppendFormat("{0}{1}", array[i].ToString(), separator);
		});
		buff.Append(array[num - 1].ToString());
		return buff.ToString();
	}

	public static Uri ToUri(this string uriString)
	{
		if (!Uri.TryCreate(uriString, uriString.MaybeUri() ? UriKind.Absolute : UriKind.Relative, out var result))
		{
			return null;
		}
		return result;
	}

	public static string UrlDecode(this string value)
	{
		if (value == null || value.Length <= 0)
		{
			return value;
		}
		return HttpUtility.UrlDecode(value);
	}

	public static string UrlEncode(this string value)
	{
		if (value == null || value.Length <= 0)
		{
			return value;
		}
		return HttpUtility.UrlEncode(value);
	}

	public static void WriteContent(this WebSocketSharp.Net.HttpListenerResponse response, byte[] content)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		int num = 0;
		if (content != null && (num = content.Length) != 0)
		{
			Stream outputStream = response.OutputStream;
			response.ContentLength64 = num;
			outputStream.Write(content, 0, num);
			outputStream.Close();
		}
	}
}
