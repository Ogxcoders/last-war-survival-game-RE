using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryMethodCall
{
	private string uri;

	private string methodName;

	private string typeName;

	private Type[] instArgs;

	private object[] args;

	private object methodSignature;

	private object callContext;

	private string scallContext;

	private object properties;

	private Type[] argTypes;

	private bool bArgsPrimitive = true;

	private MessageEnum messageEnum;

	private object[] callA;

	internal object[] WriteArray(string uri, string methodName, string typeName, Type[] instArgs, object[] args, object methodSignature, object callContext, object[] properties)
	{
		this.uri = uri;
		this.methodName = methodName;
		this.typeName = typeName;
		this.instArgs = instArgs;
		this.args = args;
		this.methodSignature = methodSignature;
		this.callContext = callContext;
		this.properties = properties;
		int num = 0;
		if (args == null || args.Length == 0)
		{
			messageEnum = MessageEnum.NoArgs;
		}
		else
		{
			argTypes = new Type[args.Length];
			bArgsPrimitive = true;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] != null)
				{
					argTypes[i] = args[i].GetType();
					if ((Converter.ToCode(argTypes[i]) == InternalPrimitiveTypeE.Invalid && (object)argTypes[i] != Converter.typeofString) || args[i] is ISerializable)
					{
						bArgsPrimitive = false;
						break;
					}
				}
			}
			if (bArgsPrimitive)
			{
				messageEnum = MessageEnum.ArgsInline;
			}
			else
			{
				num++;
				messageEnum = MessageEnum.ArgsInArray;
			}
		}
		if (instArgs != null)
		{
			num++;
			messageEnum |= MessageEnum.GenericMethod;
		}
		if (methodSignature != null)
		{
			num++;
			messageEnum |= MessageEnum.MethodSignatureInArray;
		}
		if (callContext == null)
		{
			messageEnum |= MessageEnum.NoContext;
		}
		else if (callContext is string)
		{
			messageEnum |= MessageEnum.ContextInline;
		}
		else
		{
			num++;
			messageEnum |= MessageEnum.ContextInArray;
		}
		if (properties != null)
		{
			num++;
			messageEnum |= MessageEnum.PropertyInArray;
		}
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInArray) && num == 1)
		{
			messageEnum ^= MessageEnum.ArgsInArray;
			messageEnum |= MessageEnum.ArgsIsArray;
			return args;
		}
		if (num > 0)
		{
			int num2 = 0;
			callA = new object[num];
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInArray))
			{
				callA[num2++] = args;
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.GenericMethod))
			{
				callA[num2++] = instArgs;
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.MethodSignatureInArray))
			{
				callA[num2++] = methodSignature;
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInArray))
			{
				callA[num2++] = callContext;
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.PropertyInArray))
			{
				callA[num2] = properties;
			}
			return callA;
		}
		return null;
	}

	internal void Write(__BinaryWriter sout)
	{
		sout.WriteByte(21);
		sout.WriteInt32((int)messageEnum);
		IOUtil.WriteStringWithCode(methodName, sout);
		IOUtil.WriteStringWithCode(typeName, sout);
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInline))
		{
			IOUtil.WriteStringWithCode((string)callContext, sout);
		}
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInline))
		{
			sout.WriteInt32(args.Length);
			for (int i = 0; i < args.Length; i++)
			{
				IOUtil.WriteWithCode(argTypes[i], args[i], sout);
			}
		}
	}

	[SecurityCritical]
	internal void Read(__BinaryParser input)
	{
		messageEnum = (MessageEnum)input.ReadInt32();
		methodName = (string)IOUtil.ReadWithCode(input);
		typeName = (string)IOUtil.ReadWithCode(input);
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInline))
		{
			scallContext = (string)IOUtil.ReadWithCode(input);
			LogicalCallContext logicalCallContext = new LogicalCallContext();
			logicalCallContext.RemotingData.LogicalCallID = scallContext;
			callContext = logicalCallContext;
		}
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInline))
		{
			args = IOUtil.ReadArgs(input);
		}
	}

	[SecurityCritical]
	internal IMethodCallMessage ReadArray(object[] callA, object handlerObject)
	{
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsIsArray))
		{
			args = callA;
		}
		else
		{
			int num = 0;
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInArray))
			{
				if (callA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				args = (object[])callA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.GenericMethod))
			{
				if (callA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				instArgs = (Type[])callA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.MethodSignatureInArray))
			{
				if (callA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				methodSignature = callA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInArray))
			{
				if (callA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				callContext = callA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.PropertyInArray))
			{
				if (callA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				properties = callA[num++];
			}
		}
		return new MethodCall(handlerObject, new BinaryMethodCallMessage(uri, methodName, typeName, instArgs, args, methodSignature, (LogicalCallContext)callContext, (object[])properties));
	}

	internal void Dump()
	{
	}

	[Conditional("_LOGGING")]
	private void DumpInternal()
	{
		if (!BCLDebug.CheckEnabled("BINARY"))
		{
			return;
		}
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInline))
		{
			_ = callContext is string;
		}
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInline))
		{
			for (int i = 0; i < args.Length; i++)
			{
			}
		}
	}
}
