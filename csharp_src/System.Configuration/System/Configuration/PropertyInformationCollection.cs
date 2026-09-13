using System.Collections.Specialized;
using Unity;

namespace System.Configuration;

[Serializable]
public sealed class PropertyInformationCollection : NameObjectCollectionBase
{
	public PropertyInformation this[string propertyName]
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	internal PropertyInformationCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public void CopyTo(PropertyInformation[] array, int index)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
