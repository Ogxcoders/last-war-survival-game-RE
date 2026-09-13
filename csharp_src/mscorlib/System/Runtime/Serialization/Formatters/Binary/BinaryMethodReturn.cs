using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryMethodReturn : IStreamable
{
	private object returnValue;

	private object[] args;

	private Exception exception;

	private object callContext;

	private string scallContext;

	private object properties;

	private Type[] argTypes;

	private bool bArgsPrimitive = true;

	private MessageEnum messageEnum;

	private object[] callA;

	private Type returnType;

	private static object instanceOfVoid;

	[SecuritySafeCritical]
	static BinaryMethodReturn()
	{
		instanceOfVoid = FormatterServices.GetUninitializedObject(Converter.typeofSystemVoid);
	}

	internal BinaryMethodReturn()
	{
	}

	internal object[] WriteArray(object returnValue, object[] args, Exception exception, object callContext, object[] properties)
	{
		this.returnValue = returnValue;
		this.args = args;
		this.exception = exception;
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
					if (Converter.ToCode(argTypes[i]) == InternalPrimitiveTypeE.Invalid && (object)argTypes[i] != Converter.typeofString)
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
		if (returnValue == null)
		{
			messageEnum |= MessageEnum.NoReturnValue;
		}
		else if (returnValue.GetType() == typeof(void))
		{
			messageEnum |= MessageEnum.ReturnValueVoid;
		}
		else
		{
			returnType = returnValue.GetType();
			if (Converter.ToCode(returnType) != InternalPrimitiveTypeE.Invalid || (object)returnType == Converter.typeofString)
			{
				messageEnum |= MessageEnum.ReturnValueInline;
			}
			else
			{
				num++;
				messageEnum |= MessageEnum.ReturnValueInArray;
			}
		}
		if (exception != null)
		{
			num++;
			messageEnum |= MessageEnum.ExceptionInArray;
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
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueInArray))
			{
				callA[num2++] = returnValue;
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ExceptionInArray))
			{
				callA[num2++] = exception;
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

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(22);
		sout.WriteInt32((int)messageEnum);
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueInline))
		{
			IOUtil.WriteWithCode(returnType, returnValue, sout);
		}
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
	public void Read(__BinaryParser input)
	{
		messageEnum = (MessageEnum)input.ReadInt32();
		if (IOUtil.FlagTest(messageEnum, MessageEnum.NoReturnValue))
		{
			returnValue = null;
		}
		else if (IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueVoid))
		{
			returnValue = instanceOfVoid;
		}
		else if (IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueInline))
		{
			returnValue = IOUtil.ReadWithCode(input);
		}
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
	internal IMethodReturnMessage ReadArray(object[] returnA, IMethodCallMessage methodCallMessage, object handlerObject)
	{
		if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsIsArray))
		{
			args = returnA;
		}
		else
		{
			int num = 0;
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ArgsInArray))
			{
				if (returnA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				args = (object[])returnA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueInArray))
			{
				if (returnA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				returnValue = returnA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ExceptionInArray))
			{
				if (returnA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				exception = (Exception)returnA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.ContextInArray))
			{
				if (returnA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				callContext = returnA[num++];
			}
			if (IOUtil.FlagTest(messageEnum, MessageEnum.PropertyInArray))
			{
				if (returnA.Length < num)
				{
					throw new SerializationException(Environment.GetResourceString("Invalid MethodCall or MethodReturn stream format."));
				}
				properties = returnA[num++];
			}
		}
		return new MethodResponse(methodCallMessage, handlerObject, new BinaryMethodReturnMessage(returnValue, args, exception, (LogicalCallContext)callContext, (object[])properties));
	}

	public void Dump()
	{
	}

	[Conditional("_LOGGING")]
	private void DumpInternal()
	{
		if (!BCLDebug.CheckEnabled("BINARY"))
		{
			return;
		}
		IOUtil.FlagTest(messageEnum, MessageEnum.ReturnValueInline);
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
