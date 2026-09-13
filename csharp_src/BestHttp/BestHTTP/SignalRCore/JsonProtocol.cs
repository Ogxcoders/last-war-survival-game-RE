using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore;

public sealed class JsonProtocol : IProtocol
{
	public const char Separator = '\u001e';

	public TransferModes Type => TransferModes.Text;

	public IEncoder Encoder { get; private set; }

	public HubConnection Connection { get; set; }

	public JsonProtocol(IEncoder encoder)
	{
		if (encoder == null)
		{
			throw new ArgumentNullException("encoder");
		}
		if (encoder.Name != "json")
		{
			throw new ArgumentException("Encoder must be a json encoder!");
		}
		Encoder = encoder;
	}

	public void ParseMessages(string data, ref List<Message> messages)
	{
		int num = 0;
		int num2 = data.IndexOf('\u001e');
		if (num2 == -1)
		{
			throw new Exception("Missing separator!");
		}
		while (num2 != -1)
		{
			string text = data.Substring(num, num2 - num);
			Message item = Encoder.DecodeAs<Message>(text);
			messages.Add(item);
			num = num2 + 1;
			num2 = data.IndexOf('\u001e', num);
		}
	}

	public void ParseMessages(byte[] data, ref List<Message> messages)
	{
	}

	public byte[] EncodeMessage(Message message)
	{
		string text = null;
		switch (message.type)
		{
		case MessageTypes.Invocation:
		case MessageTypes.StreamInvocation:
			text = Encoder.EncodeAsText(new InvocationMessage
			{
				type = message.type,
				invocationId = message.invocationId,
				nonblocking = message.nonblocking,
				target = message.target,
				arguments = message.arguments
			});
			break;
		case MessageTypes.CancelInvocation:
			text = Encoder.EncodeAsText(new CancelInvocationMessage
			{
				invocationId = message.invocationId
			});
			break;
		case MessageTypes.Ping:
			text = Encoder.EncodeAsText(default(PingMessage));
			break;
		}
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return WithSeparator(text);
	}

	public object[] GetRealArguments(Type[] argTypes, object[] arguments)
	{
		if (arguments == null || arguments.Length == 0)
		{
			return null;
		}
		if (argTypes.Length > arguments.Length)
		{
			throw new Exception($"argType.Length({argTypes.Length}) < arguments.length({arguments.Length})");
		}
		object[] array = new object[arguments.Length];
		for (int i = 0; i < arguments.Length; i++)
		{
			array[i] = ConvertTo(argTypes[i], arguments[i]);
		}
		return array;
	}

	public object ConvertTo(Type toType, object obj)
	{
		if (obj == null)
		{
			return null;
		}
		if (toType.IsPrimitive || toType.IsEnum)
		{
			return Convert.ChangeType(obj, toType);
		}
		if (toType == typeof(string))
		{
			return obj.ToString();
		}
		return Encoder.ConvertTo(toType, obj);
	}

	public static byte[] WithSeparator(string str)
	{
		int byteCount = Encoding.UTF8.GetByteCount(str);
		byte[] array = new byte[byteCount + 1];
		Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
		array[byteCount] = 30;
		return array;
	}
}
