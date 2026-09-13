using System.Collections;

namespace System.Web.Services.Description;

public sealed class MimeTextMatchCollection : CollectionBase
{
	public MimeTextMatch this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (MimeTextMatch)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public int Add(MimeTextMatch match)
	{
		Insert(base.Count, match);
		return base.Count - 1;
	}

	public bool Contains(MimeTextMatch match)
	{
		return base.List.Contains(match);
	}

	public void CopyTo(MimeTextMatch[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public int IndexOf(MimeTextMatch match)
	{
		return base.List.IndexOf(match);
	}

	public void Insert(int index, MimeTextMatch match)
	{
		SetParent(match, this);
		base.List.Insert(index, match);
	}

	public void Remove(MimeTextMatch match)
	{
		base.List.Remove(match);
	}

	private void SetParent(object value, object parent)
	{
		((MimeTextMatch)value).SetParent((MimeTextMatchCollection)parent);
	}
}
