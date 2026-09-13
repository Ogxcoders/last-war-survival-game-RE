using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use HideInPrefabInstance or HideInPrefabAsset instead.", false)]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class ShowForPrefabOnlyAttribute : Attribute
{
}
