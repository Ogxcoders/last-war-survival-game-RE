using System.Collections;

namespace System.Web.Services.Description;

public sealed class MimePartCollection : CollectionBase
{
	public MimePart this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (MimePart)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public int Add(MimePart mimePart)
	{
		Insert(base.Count, mimePart);
		return base.Count - 1;
	}

	public bool Contains(MimePart mimePart)
	{
		return base.List.Contains(mimePart);
	}

	public void CopyTo(MimePart[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public int IndexOf(MimePart mimePart)
	{
		return base.List.IndexOf(mimePart);
	}

	public void Insert(int index, MimePart mimePart)
	{
		base.List.Insert(index, mimePart);
	}

	public void Remove(MimePart mimePart)
	{
		base.List.Remove(mimePart);
	}
}
