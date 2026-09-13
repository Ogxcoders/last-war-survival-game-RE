using System.Collections;

namespace System.Drawing.Design;

public sealed class CategoryNameCollection : ReadOnlyCollectionBase
{
	public string this[int index]
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public CategoryNameCollection(CategoryNameCollection value)
	{
		throw new PlatformNotSupportedException();
	}

	public CategoryNameCollection(string[] value)
	{
		throw new PlatformNotSupportedException();
	}

	public bool Contains(string value)
	{
		throw new PlatformNotSupportedException();
	}

	public void CopyTo(string[] array, int index)
	{
		throw new PlatformNotSupportedException();
	}

	public int IndexOf(string value)
	{
		throw new PlatformNotSupportedException();
	}
}
