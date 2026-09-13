using System;
using UnityEngine.Bindings;

namespace UnityEngine;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Method)]
internal class ThreadAndSerializationSafeAttribute : Attribute
{
}
