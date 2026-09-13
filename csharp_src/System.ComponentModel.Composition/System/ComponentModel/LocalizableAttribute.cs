using System.Diagnostics;

namespace System.ComponentModel;

[Conditional("NOT_FEATURE_LEGACYCOMPONENTMODEL")]
internal sealed class LocalizableAttribute : Attribute
{
	public LocalizableAttribute(bool isLocalizable)
	{
	}
}
