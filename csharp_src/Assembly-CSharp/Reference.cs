using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
public sealed class Reference : PropertyAttribute
{
	[SerializeField]
	public string Path;

	public ReferenceType RefType { get; private set; }

	public Reference(ReferenceType refType)
	{
		RefType = refType;
	}
}
