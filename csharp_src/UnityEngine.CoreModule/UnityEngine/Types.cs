using System;
using System.ComponentModel;

namespace UnityEngine;

public static class Types
{
	[Obsolete("This was an internal method which is no longer used", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Type GetType(string typeName, string assemblyName)
	{
		return null;
	}
}
