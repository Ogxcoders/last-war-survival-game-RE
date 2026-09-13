using System;
using System.Collections.Generic;
using UnityEngine.Bindings;

namespace UnityEngine;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class AssetFileNameExtensionAttribute : Attribute
{
	public string preferredExtension { get; }

	public IEnumerable<string> otherExtensions { get; }

	public AssetFileNameExtensionAttribute(string preferredExtension, params string[] otherExtensions)
	{
		this.preferredExtension = preferredExtension;
		this.otherExtensions = otherExtensions;
	}
}
