namespace System.Runtime.InteropServices.WindowsRuntime;

[AttributeUsage(AttributeTargets.Delegate | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
public sealed class ReturnValueNameAttribute : Attribute
{
	private string m_Name;

	public string Name => m_Name;

	public ReturnValueNameAttribute(string name)
	{
		m_Name = name;
	}
}
