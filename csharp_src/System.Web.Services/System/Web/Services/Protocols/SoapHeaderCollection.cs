using System.Collections;

namespace System.Web.Services.Protocols;

public class SoapHeaderCollection : CollectionBase
{
	public SoapHeader this[int index]
	{
		get
		{
			return (SoapHeader)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public int Add(SoapHeader header)
	{
		Insert(base.Count, header);
		return base.Count - 1;
	}

	public bool Contains(SoapHeader header)
	{
		return base.List.Contains(header);
	}

	public void CopyTo(SoapHeader[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public int IndexOf(SoapHeader header)
	{
		return base.List.IndexOf(header);
	}

	public void Insert(int index, SoapHeader header)
	{
		if (index < 0 || index > base.Count)
		{
			throw new ArgumentOutOfRangeException();
		}
		base.List.Insert(index, header);
	}

	public void Remove(SoapHeader header)
	{
		base.List.Remove(header);
	}
}
