namespace System.Xml;

internal abstract class BaseTreeIterator
{
	protected DataSetMapper mapper;

	internal abstract XmlNode CurrentNode { get; }

	internal BaseTreeIterator(DataSetMapper mapper)
	{
		this.mapper = mapper;
	}

	internal abstract bool Next();

	internal abstract bool NextRight();

	internal bool NextRowElement()
	{
		while (Next())
		{
			if (OnRowElement())
			{
				return true;
			}
		}
		return false;
	}

	internal bool NextRightRowElement()
	{
		if (NextRight())
		{
			if (OnRowElement())
			{
				return true;
			}
			return NextRowElement();
		}
		return false;
	}

	internal bool OnRowElement()
	{
		if (CurrentNode is XmlBoundElement xmlBoundElement)
		{
			return xmlBoundElement.Row != null;
		}
		return false;
	}
}
