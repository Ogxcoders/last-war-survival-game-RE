using System;

namespace UnityEngine.Scripting.APIUpdating;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
public class MovedFromAttribute : Attribute
{
	internal MovedFromAttributeData data;

	internal bool AffectsAPIUpdater => !data.classHasChanged && !data.assemblyHasChanged;

	public bool IsInDifferentAssembly => data.assemblyHasChanged;

	public MovedFromAttribute(bool autoUpdateAPI, string sourceNamespace = null, string sourceAssembly = null, string sourceClassName = null)
	{
		data.Set(autoUpdateAPI, sourceNamespace, sourceAssembly, sourceClassName);
	}

	public MovedFromAttribute(string sourceNamespace)
	{
		data.Set(autoUpdateAPI: true, sourceNamespace);
	}
}
