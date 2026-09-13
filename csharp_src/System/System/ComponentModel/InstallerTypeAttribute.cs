namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public class InstallerTypeAttribute : Attribute
{
	private string _typeName;

	public virtual Type InstallerType => Type.GetType(_typeName);

	public InstallerTypeAttribute(Type installerType)
	{
		_typeName = installerType.AssemblyQualifiedName;
	}

	public InstallerTypeAttribute(string typeName)
	{
		_typeName = typeName;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is InstallerTypeAttribute installerTypeAttribute)
		{
			return installerTypeAttribute._typeName == _typeName;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
