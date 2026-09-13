namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class BestFitMappingAttribute : Attribute
{
	internal bool _bestFitMapping;

	public bool ThrowOnUnmappableChar;

	public bool BestFitMapping => _bestFitMapping;

	public BestFitMappingAttribute(bool BestFitMapping)
	{
		_bestFitMapping = BestFitMapping;
	}
}
