using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
[ExtensionOfNativeClass]
[NativeClass(null)]
[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
public class ScriptableObject : Object
{
	public ScriptableObject()
	{
		CreateScriptableObject(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_MONO")]
	[Obsolete("Use EditorUtility.SetDirty instead")]
	public extern void SetDirty();

	public static ScriptableObject CreateInstance(string className)
	{
		return CreateScriptableObjectInstanceFromName(className);
	}

	public static ScriptableObject CreateInstance(Type type)
	{
		return CreateScriptableObjectInstanceFromType(type, applyDefaultsAndReset: true);
	}

	public static T CreateInstance<T>() where T : ScriptableObject
	{
		return (T)CreateInstance(typeof(T));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void CreateScriptableObject([Writable] ScriptableObject self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Scripting::CreateScriptableObject")]
	private static extern ScriptableObject CreateScriptableObjectInstanceFromName(string className);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Scripting::CreateScriptableObjectWithType")]
	internal static extern ScriptableObject CreateScriptableObjectInstanceFromType(Type type, bool applyDefaultsAndReset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Scripting::ResetAndApplyDefaultInstances")]
	internal static extern void ResetAndApplyDefaultInstances(Object obj);
}
