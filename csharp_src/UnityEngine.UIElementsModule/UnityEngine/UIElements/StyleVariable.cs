namespace UnityEngine.UIElements;

internal struct StyleVariable
{
	public readonly string name;

	public readonly StyleSheet sheet;

	public readonly StyleValueHandle[] handles;

	public StyleVariable(string name, StyleSheet sheet, StyleValueHandle[] handles)
	{
		this.name = name;
		this.sheet = sheet;
		this.handles = handles;
	}

	public override int GetHashCode()
	{
		int hashCode = name.GetHashCode();
		hashCode = (hashCode * 397) ^ sheet.GetHashCode();
		return (hashCode * 397) ^ handles.GetHashCode();
	}
}
