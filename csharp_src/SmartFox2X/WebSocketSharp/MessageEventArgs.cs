using System;
using System.Text;

namespace WebSocketSharp;

public class MessageEventArgs : EventArgs
{
	private string _data;

	private Opcode _opcode;

	private byte[] _rawData;

	public string Data => _data;

	public byte[] RawData => _rawData;

	public Opcode Type => _opcode;

	internal MessageEventArgs(WebSocketFrame frame)
	{
		_opcode = frame.Opcode;
		_rawData = frame.PayloadData.ApplicationData;
		_data = convertToString(_opcode, _rawData);
	}

	internal MessageEventArgs(Opcode opcode, byte[] rawData)
	{
		if ((ulong)rawData.LongLength > 9223372036854775807uL)
		{
			throw new WebSocketException(CloseStatusCode.TooBig);
		}
		_opcode = opcode;
		_rawData = rawData;
		_data = convertToString(opcode, rawData);
	}

	private static string convertToString(Opcode opcode, byte[] rawData)
	{
		if (rawData.LongLength != 0L)
		{
			if (opcode != Opcode.Text)
			{
				return opcode.ToString();
			}
			return Encoding.UTF8.GetString(rawData);
		}
		return string.Empty;
	}
}
