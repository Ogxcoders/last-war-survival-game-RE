using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Joker;

public abstract class MessageService : IMessageService, IService
{
	protected readonly DoubleMap<Type, int> _typeOpcode = new DoubleMap<Type, int>();

	protected readonly Dictionary<Type, Type> _requestResponse = new Dictionary<Type, Type>();

	protected readonly List<Type> _messageTypes;

	public static MessageService Instance { get; private set; }

	public MessageService(List<Type> messageTypes)
	{
		Instance = this;
		_messageTypes = ((messageTypes != null) ? new List<Type>(messageTypes) : new List<Type>());
	}

	public void DefineMessageTypes(List<Type> messageTypes)
	{
		if (_typeOpcode.Keys.Any())
		{
			Log.Error("DefineMessageTypes after startup");
		}
		else
		{
			_messageTypes.AddRange(messageTypes);
		}
	}

	public void Awake()
	{
	}

	public virtual void Startup()
	{
		IEnumerable<Type> enumerable = null;
		enumerable = ((_messageTypes == null || _messageTypes.Count <= 0) ? ((IEnumerable<Type>)(AssemblyService.Instance?.GetTypes(typeof(MessageAttribute)))) : ((IEnumerable<Type>)_messageTypes));
		if (enumerable == null)
		{
			Log.Warning("MessageService no types found");
			return;
		}
		foreach (Type item in enumerable)
		{
			RegisterOpCode(item);
		}
	}

	public virtual void Shutdown()
	{
		_typeOpcode.Clear();
		_requestResponse.Clear();
		_messageTypes.Clear();
	}

	public virtual void Destroy()
	{
	}

	public abstract void Serialize(IMessage message, MemoryBuffer buffer, bool recurse = true);

	public abstract IMessage Deserialize(MemoryBuffer buffer, bool recurse = true);

	public IMessage CloneMessage(IMessage message, bool recurse = true)
	{
		MemoryBuffer memoryBuffer = MemoryBuffer.Fetch();
		Serialize(message, memoryBuffer, recurse);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		IMessage result = Deserialize(memoryBuffer, recurse);
		MemoryBuffer.Recycle(memoryBuffer);
		return result;
	}

	public int GetOpcode(Type type)
	{
		if (_typeOpcode.TryGetValueByKey(type, out var value))
		{
			return value;
		}
		throw new ArgumentException($"Unknown message type: {type}");
	}

	public Type GetType(int opcode)
	{
		Type keyByValue = _typeOpcode.GetKeyByValue(opcode);
		if (keyByValue == null)
		{
			throw new Exception($"OpcodeType not found type: {opcode}");
		}
		return keyByValue;
	}

	public Type GetResponseType(Type request)
	{
		if (!_requestResponse.TryGetValue(request, out var value))
		{
			throw new Exception("not found response type, request type: " + request.FullName);
		}
		return value;
	}

	protected void RegisterOpCode(Type type)
	{
		int num = 0;
		if (type.GetCustomAttribute(typeof(MessageAttribute), inherit: false) is MessageAttribute messageAttribute)
		{
			num = messageAttribute.Opcode;
		}
		if (num == 0)
		{
			num = type.HashCodeFNV1A32();
		}
		if (!_typeOpcode.Add(type, num))
		{
			Log.Error($"Message ({type.FullName}) opcode ({num}) is conflict with ({_typeOpcode.GetKeyByValue(num).FullName}).");
		}
	}
}
