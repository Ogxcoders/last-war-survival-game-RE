using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use DisableInPrefabInstance or DisableInPrefabAsset instead.", false)]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class EnableForPrefabOnlyAttribute : Attribute
{
}
