using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal class IteratorFilter : XPathNodeIterator
{
	private XPathNodeIterator _innerIterator;

	private string _name;

	private int _position;

	public override XPathNavigator Current => _innerIterator.Current;

	public override int CurrentPosition => _position;

	internal IteratorFilter(XPathNodeIterator innerIterator, string name)
	{
		_innerIterator = innerIterator;
		_name = name;
	}

	private IteratorFilter(IteratorFilter it)
	{
		_innerIterator = it._innerIterator.Clone();
		_name = it._name;
		_position = it._position;
	}

	public override XPathNodeIterator Clone()
	{
		return new IteratorFilter(this);
	}

	public override bool MoveNext()
	{
		while (_innerIterator.MoveNext())
		{
			if (_innerIterator.Current.LocalName == _name)
			{
				_position++;
				return true;
			}
		}
		return false;
	}
}
