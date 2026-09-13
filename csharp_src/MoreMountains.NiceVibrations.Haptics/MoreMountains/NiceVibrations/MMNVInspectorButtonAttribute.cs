using System;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

[AttributeUsage(AttributeTargets.Field)]
public class MMNVInspectorButtonAttribute : PropertyAttribute
{
	public readonly string MethodName;

	public MMNVInspectorButtonAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}
}
