using System;
using System.Reflection;

namespace Google.Protobuf.Reflection;

internal sealed class SingleFieldAccessor : FieldAccessorBase
{
	private readonly Action<IMessage, object> setValueDelegate;

	private readonly Action<IMessage> clearDelegate;

	private readonly Func<IMessage, bool> hasDelegate;

	internal SingleFieldAccessor(PropertyInfo property, FieldDescriptor descriptor)
		: base(property, descriptor)
	{
		if (!property.CanWrite)
		{
			throw new ArgumentException("Not all required properties/methods available");
		}
		setValueDelegate = ReflectionUtil.CreateActionIMessageObject(property.GetSetMethod());
		if (descriptor.File.Syntax == Syntax.Proto3)
		{
			hasDelegate = delegate
			{
				throw new InvalidOperationException("HasValue is not implemented for proto3 fields");
			};
			Type propertyType = property.PropertyType;
			object defaultValue = ((descriptor.FieldType == FieldType.Message) ? null : ((propertyType == typeof(string)) ? "" : ((propertyType == typeof(ByteString)) ? ByteString.Empty : Activator.CreateInstance(propertyType))));
			clearDelegate = delegate(IMessage message)
			{
				SetValue(message, defaultValue);
			};
			return;
		}
		MethodInfo getMethod = property.DeclaringType.GetRuntimeProperty("Has" + property.Name).GetMethod;
		if (getMethod == null)
		{
			throw new ArgumentException("Not all required properties/methods are available");
		}
		hasDelegate = ReflectionUtil.CreateFuncIMessageBool(getMethod);
		MethodInfo runtimeMethod = property.DeclaringType.GetRuntimeMethod("Clear" + property.Name, ReflectionUtil.EmptyTypes);
		if (runtimeMethod == null)
		{
			throw new ArgumentException("Not all required properties/methods are available");
		}
		clearDelegate = ReflectionUtil.CreateActionIMessage(runtimeMethod);
	}

	public override void Clear(IMessage message)
	{
		clearDelegate(message);
	}

	public override bool HasValue(IMessage message)
	{
		return hasDelegate(message);
	}

	public override void SetValue(IMessage message, object value)
	{
		setValueDelegate(message, value);
	}
}
