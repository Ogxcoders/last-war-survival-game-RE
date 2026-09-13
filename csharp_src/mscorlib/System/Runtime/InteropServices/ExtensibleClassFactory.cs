using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace System.Runtime.InteropServices;

[ComVisible(true)]
public sealed class ExtensibleClassFactory
{
	private static readonly Hashtable hashtable = new Hashtable();

	private ExtensibleClassFactory()
	{
	}

	internal static ObjectCreationDelegate GetObjectCreationCallback(Type t)
	{
		return hashtable[t] as ObjectCreationDelegate;
	}

	public static void RegisterObjectCreationCallback(ObjectCreationDelegate callback)
	{
		int i = 1;
		for (StackTrace stackTrace = new StackTrace(fNeedFileInfo: false); i < stackTrace.FrameCount; i++)
		{
			MethodBase method = stackTrace.GetFrame(i).GetMethod();
			if (method.MemberType == MemberTypes.Constructor && method.IsStatic)
			{
				hashtable.Add(method.DeclaringType, callback);
				return;
			}
		}
		throw new InvalidOperationException("RegisterObjectCreationCallback must be called from .cctor of class derived from ComImport type.");
	}
}
