using System.Globalization;
using System.Text;

namespace System.Net.WebSockets;

internal static class WebSocketValidate
{
	internal const int MaxControlFramePayloadLength = 123;

	private const int CloseStatusCodeAbort = 1006;

	private const int CloseStatusCodeFailedTLSHandshake = 1015;

	private const int InvalidCloseStatusCodesFrom = 0;

	private const int InvalidCloseStatusCodesTo = 999;

	private const string Separators = "()<>@,;:\\\"/[]?={} ";

	internal static void ThrowIfInvalidState(WebSocketState currentState, bool isDisposed, WebSocketState[] validStates)
	{
		string p = string.Empty;
		if (validStates != null && validStates.Length != 0)
		{
			foreach (WebSocketState webSocketState in validStates)
			{
				if (currentState == webSocketState)
				{
					if (isDisposed)
					{
						throw new ObjectDisposedException("WebSocket");
					}
					return;
				}
			}
			p = string.Join(", ", validStates);
		}
		throw new WebSocketException(WebSocketError.InvalidState, global::SR.Format("The WebSocket is in an invalid state ('{0}') for this operation. Valid states are: '{1}'", currentState, p));
	}

	internal static void ValidateSubprotocol(string subProtocol)
	{
		if (string.IsNullOrWhiteSpace(subProtocol))
		{
			throw new ArgumentException("Empty string is not a valid subprotocol value. Please use \\\"null\\\" to specify no value.", "subProtocol");
		}
		string text = null;
		for (int i = 0; i < subProtocol.Length; i++)
		{
			char c = subProtocol[i];
			if (c < '!' || c > '~')
			{
				text = string.Format(CultureInfo.InvariantCulture, "[{0}]", (int)c);
				break;
			}
			if (!char.IsLetterOrDigit(c) && "()<>@,;:\\\"/[]?={} ".IndexOf(c) >= 0)
			{
				text = c.ToString();
				break;
			}
		}
		if (text != null)
		{
			throw new ArgumentException(global::SR.Format("The WebSocket protocol '{0}' is invalid because it contains the invalid character '{1}'.", subProtocol, text), "subProtocol");
		}
	}

	internal static void ValidateCloseStatus(WebSocketCloseStatus closeStatus, string statusDescription)
	{
		if (closeStatus == WebSocketCloseStatus.Empty && !string.IsNullOrEmpty(statusDescription))
		{
			throw new ArgumentException(global::SR.Format("The close status description '{0}' is invalid. When using close status code '{1}' the description must be null.", statusDescription, WebSocketCloseStatus.Empty), "statusDescription");
		}
		if ((closeStatus >= (WebSocketCloseStatus)0 && closeStatus <= (WebSocketCloseStatus)999) || closeStatus == (WebSocketCloseStatus)1006 || closeStatus == (WebSocketCloseStatus)1015)
		{
			throw new ArgumentException(global::SR.Format("The close status code '{0}' is reserved for system use only and cannot be specified when calling this method.", (int)closeStatus), "closeStatus");
		}
		int num = 0;
		if (!string.IsNullOrEmpty(statusDescription))
		{
			num = Encoding.UTF8.GetByteCount(statusDescription);
		}
		if (num > 123)
		{
			throw new ArgumentException(global::SR.Format("The close status description '{0}' is too long. The UTF8-representation of the status description must not be longer than {1} bytes.", statusDescription, 123), "statusDescription");
		}
	}

	internal static void ThrowPlatformNotSupportedException()
	{
		throw new PlatformNotSupportedException("The WebSocket protocol is not supported on this platform.");
	}

	internal static void ValidateArraySegment(ArraySegment<byte> arraySegment, string parameterName)
	{
		if (arraySegment.Array == null)
		{
			throw new ArgumentNullException(parameterName + ".Array");
		}
		if (arraySegment.Offset < 0 || arraySegment.Offset > arraySegment.Array.Length)
		{
			throw new ArgumentOutOfRangeException(parameterName + ".Offset");
		}
		if (arraySegment.Count < 0 || arraySegment.Count > arraySegment.Array.Length - arraySegment.Offset)
		{
			throw new ArgumentOutOfRangeException(parameterName + ".Count");
		}
	}

	internal static void ValidateBuffer(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
	}
}
