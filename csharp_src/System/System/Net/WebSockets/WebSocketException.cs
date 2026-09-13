using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.WebSockets;

[Serializable]
public sealed class WebSocketException : Win32Exception
{
	private readonly WebSocketError _webSocketErrorCode;

	public override int ErrorCode => base.NativeErrorCode;

	public WebSocketError WebSocketErrorCode => _webSocketErrorCode;

	public WebSocketException()
		: this(Marshal.GetLastWin32Error())
	{
	}

	public WebSocketException(WebSocketError error)
		: this(error, GetErrorMessage(error))
	{
	}

	public WebSocketException(WebSocketError error, string message)
		: base(message)
	{
		_webSocketErrorCode = error;
	}

	public WebSocketException(WebSocketError error, Exception innerException)
		: this(error, GetErrorMessage(error), innerException)
	{
	}

	public WebSocketException(WebSocketError error, string message, Exception innerException)
		: base(message, innerException)
	{
		_webSocketErrorCode = error;
	}

	public WebSocketException(int nativeError)
		: base(nativeError)
	{
		_webSocketErrorCode = ((!Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
		SetErrorCodeOnError(nativeError);
	}

	public WebSocketException(int nativeError, string message)
		: base(nativeError, message)
	{
		_webSocketErrorCode = ((!Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
		SetErrorCodeOnError(nativeError);
	}

	public WebSocketException(int nativeError, Exception innerException)
		: base("An internal WebSocket error occurred. Please see the innerException, if present, for more details.", innerException)
	{
		_webSocketErrorCode = ((!Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
		SetErrorCodeOnError(nativeError);
	}

	public WebSocketException(WebSocketError error, int nativeError)
		: this(error, nativeError, GetErrorMessage(error))
	{
	}

	public WebSocketException(WebSocketError error, int nativeError, string message)
		: base(message)
	{
		_webSocketErrorCode = error;
		SetErrorCodeOnError(nativeError);
	}

	public WebSocketException(WebSocketError error, int nativeError, Exception innerException)
		: this(error, nativeError, GetErrorMessage(error), innerException)
	{
	}

	public WebSocketException(WebSocketError error, int nativeError, string message, Exception innerException)
		: base(message, innerException)
	{
		_webSocketErrorCode = error;
		SetErrorCodeOnError(nativeError);
	}

	public WebSocketException(string message)
		: base(message)
	{
	}

	public WebSocketException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private WebSocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("WebSocketErrorCode", _webSocketErrorCode);
	}

	private static string GetErrorMessage(WebSocketError error)
	{
		return error switch
		{
			WebSocketError.InvalidMessageType => global::SR.Format("The received  message type is invalid after calling {0}. {0} should only be used if no more data is expected from the remote endpoint. Use '{1}' instead to keep being able to receive data but close the output channel.", "WebSocket.CloseAsync", "WebSocket.CloseOutputAsync"), 
			WebSocketError.Faulted => "An exception caused the WebSocket to enter the Aborted state. Please see the InnerException, if present, for more details.", 
			WebSocketError.NotAWebSocket => "A WebSocket operation was called on a request or response that is not a WebSocket.", 
			WebSocketError.UnsupportedVersion => "Unsupported WebSocket version.", 
			WebSocketError.UnsupportedProtocol => "The WebSocket request or response operation was called with unsupported protocol(s).", 
			WebSocketError.HeaderError => "The WebSocket request or response contained unsupported header(s).", 
			WebSocketError.ConnectionClosedPrematurely => "The remote party closed the WebSocket connection without completing the close handshake.", 
			WebSocketError.InvalidState => "The WebSocket instance cannot be used for communication because it has been transitioned into an invalid state.", 
			_ => "An internal WebSocket error occurred. Please see the innerException, if present, for more details.", 
		};
	}

	private void SetErrorCodeOnError(int nativeError)
	{
		if (!Succeeded(nativeError))
		{
			base.HResult = nativeError;
		}
	}

	private static bool Succeeded(int hr)
	{
		return hr >= 0;
	}
}
