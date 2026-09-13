using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
[ComVisible(true)]
public sealed class IDispatchConstantAttribute : CustomConstantAttribute
{
	public override object Value => new DispatchWrapper(null);
}
