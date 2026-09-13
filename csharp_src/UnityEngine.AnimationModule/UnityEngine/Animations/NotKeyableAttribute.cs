using System;
using UnityEngine.Scripting;

namespace UnityEngine.Animations;

[RequiredByNativeCode]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
public class NotKeyableAttribute : Attribute
{
}
