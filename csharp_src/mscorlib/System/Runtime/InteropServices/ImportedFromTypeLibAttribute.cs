namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
[ComVisible(true)]
public sealed class ImportedFromTypeLibAttribute : Attribute
{
	internal string _val;

	public string Value => _val;

	public ImportedFromTypeLibAttribute(string tlbFile)
	{
		_val = tlbFile;
	}
}
