using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
[ComVisible(true)]
public sealed class RequiredAttributeAttribute : Attribute
{
	private Type requiredContract;

	public Type RequiredContract => requiredContract;

	public RequiredAttributeAttribute(Type requiredContract)
	{
		this.requiredContract = requiredContract;
	}
}
