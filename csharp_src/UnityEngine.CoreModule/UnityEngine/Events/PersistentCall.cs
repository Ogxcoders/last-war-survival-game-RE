using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events;

[Serializable]
internal class PersistentCall
{
	[FormerlySerializedAs("instance")]
	[SerializeField]
	private Object m_Target;

	[FormerlySerializedAs("methodName")]
	[SerializeField]
	private string m_MethodName;

	[SerializeField]
	[FormerlySerializedAs("mode")]
	private PersistentListenerMode m_Mode = PersistentListenerMode.EventDefined;

	[SerializeField]
	[FormerlySerializedAs("arguments")]
	private ArgumentCache m_Arguments = new ArgumentCache();

	[FormerlySerializedAs("enabled")]
	[SerializeField]
	[FormerlySerializedAs("m_Enabled")]
	private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;

	public Object target => m_Target;

	public string methodName => m_MethodName;

	public PersistentListenerMode mode
	{
		get
		{
			return m_Mode;
		}
		set
		{
			m_Mode = value;
		}
	}

	public ArgumentCache arguments => m_Arguments;

	public UnityEventCallState callState
	{
		get
		{
			return m_CallState;
		}
		set
		{
			m_CallState = value;
		}
	}

	public bool IsValid()
	{
		return target != null && !string.IsNullOrEmpty(methodName);
	}

	public BaseInvokableCall GetRuntimeCall(UnityEventBase theEvent)
	{
		if (m_CallState == UnityEventCallState.Off || theEvent == null)
		{
			return null;
		}
		MethodInfo methodInfo = theEvent.FindMethod(this);
		if ((object)methodInfo == null)
		{
			return null;
		}
		if (!methodInfo.IsStatic && target == null)
		{
			return null;
		}
		return m_Mode switch
		{
			PersistentListenerMode.EventDefined => theEvent.GetDelegate(target, methodInfo), 
			PersistentListenerMode.Object => GetObjectCall(target, methodInfo, m_Arguments), 
			PersistentListenerMode.Float => new CachedInvokableCall<float>(target, methodInfo, m_Arguments.floatArgument), 
			PersistentListenerMode.Int => new CachedInvokableCall<int>(target, methodInfo, m_Arguments.intArgument), 
			PersistentListenerMode.String => new CachedInvokableCall<string>(target, methodInfo, m_Arguments.stringArgument), 
			PersistentListenerMode.Bool => new CachedInvokableCall<bool>(target, methodInfo, m_Arguments.boolArgument), 
			PersistentListenerMode.Void => new InvokableCall(target, methodInfo), 
			_ => null, 
		};
	}

	private static BaseInvokableCall GetObjectCall(Object target, MethodInfo method, ArgumentCache arguments)
	{
		Type type = typeof(Object);
		if (!string.IsNullOrEmpty(arguments.unityObjectArgumentAssemblyTypeName))
		{
			type = Type.GetType(arguments.unityObjectArgumentAssemblyTypeName, throwOnError: false) ?? typeof(Object);
		}
		Type typeFromHandle = typeof(CachedInvokableCall<>);
		Type type2 = typeFromHandle.MakeGenericType(type);
		ConstructorInfo constructor = type2.GetConstructor(new Type[3]
		{
			typeof(Object),
			typeof(MethodInfo),
			type
		});
		Object obj = arguments.unityObjectArgument;
		if (obj != null && !type.IsAssignableFrom(obj.GetType()))
		{
			obj = null;
		}
		return constructor.Invoke(new object[3] { target, method, obj }) as BaseInvokableCall;
	}

	public void RegisterPersistentListener(Object ttarget, string mmethodName)
	{
		m_Target = ttarget;
		m_MethodName = mmethodName;
	}

	public void UnregisterPersistentListener()
	{
		m_MethodName = string.Empty;
		m_Target = null;
	}
}
