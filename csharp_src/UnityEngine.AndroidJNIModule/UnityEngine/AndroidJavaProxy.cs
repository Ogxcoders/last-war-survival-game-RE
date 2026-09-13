using System;
using System.Reflection;

namespace UnityEngine;

public class AndroidJavaProxy
{
	public readonly AndroidJavaClass javaInterface;

	internal IntPtr proxyObject = IntPtr.Zero;

	private static readonly GlobalJavaObjectRef s_JavaLangSystemClass = new GlobalJavaObjectRef(AndroidJNISafe.FindClass("java/lang/System"));

	private static readonly IntPtr s_HashCodeMethodID = AndroidJNIHelper.GetMethodID(s_JavaLangSystemClass, "identityHashCode", "(Ljava/lang/Object;)I", isStatic: true);

	public AndroidJavaProxy(string javaInterface)
		: this(new AndroidJavaClass(javaInterface))
	{
	}

	public AndroidJavaProxy(AndroidJavaClass javaInterface)
	{
		this.javaInterface = javaInterface;
	}

	~AndroidJavaProxy()
	{
		AndroidJNISafe.DeleteWeakGlobalRef(proxyObject);
	}

	public virtual AndroidJavaObject Invoke(string methodName, object[] args)
	{
		Exception ex = null;
		BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		Type[] array = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			array[i] = ((args[i] == null) ? typeof(AndroidJavaObject) : args[i].GetType());
		}
		try
		{
			MethodInfo method = GetType().GetMethod(methodName, bindingAttr, null, array, null);
			if ((object)method != null)
			{
				return _AndroidJNIHelper.Box(method.Invoke(this, args));
			}
		}
		catch (TargetInvocationException ex2)
		{
			ex = ex2.InnerException;
		}
		catch (Exception ex3)
		{
			ex = ex3;
		}
		string[] array2 = new string[args.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array2[j] = array[j].ToString();
		}
		if (ex != null)
		{
			throw new TargetInvocationException(string.Concat(GetType(), ".", methodName, "(", string.Join(",", array2), ")"), ex);
		}
		AndroidReflection.SetNativeExceptionOnProxy(GetRawProxy(), new Exception(string.Concat("No such proxy method: ", GetType(), ".", methodName, "(", string.Join(",", array2), ")")), methodNotFound: true);
		return null;
	}

	public virtual AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
	{
		object[] array = new object[javaArgs.Length];
		for (int i = 0; i < javaArgs.Length; i++)
		{
			array[i] = _AndroidJNIHelper.Unbox(javaArgs[i]);
			if (!(array[i] is AndroidJavaObject) && javaArgs[i] != null)
			{
				javaArgs[i].Dispose();
			}
		}
		return Invoke(methodName, array);
	}

	public virtual bool equals(AndroidJavaObject obj)
	{
		IntPtr obj2 = obj?.GetRawObject() ?? IntPtr.Zero;
		return AndroidJNI.IsSameObject(proxyObject, obj2);
	}

	public virtual int hashCode()
	{
		jvalue[] array = new jvalue[1];
		array[0].l = GetRawProxy();
		return AndroidJNISafe.CallStaticIntMethod(s_JavaLangSystemClass, s_HashCodeMethodID, array);
	}

	public virtual string toString()
	{
		return ToString() + " <c# proxy java object>";
	}

	internal AndroidJavaObject GetProxyObject()
	{
		return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(GetRawProxy());
	}

	internal IntPtr GetRawProxy()
	{
		IntPtr intPtr = IntPtr.Zero;
		if (proxyObject != IntPtr.Zero)
		{
			intPtr = AndroidJNI.NewLocalRef(proxyObject);
			if (intPtr == IntPtr.Zero)
			{
				AndroidJNI.DeleteWeakGlobalRef(proxyObject);
				proxyObject = IntPtr.Zero;
			}
		}
		if (intPtr == IntPtr.Zero)
		{
			intPtr = AndroidJNIHelper.CreateJavaProxy(this);
			proxyObject = AndroidJNI.NewWeakGlobalRef(intPtr);
		}
		return intPtr;
	}
}
