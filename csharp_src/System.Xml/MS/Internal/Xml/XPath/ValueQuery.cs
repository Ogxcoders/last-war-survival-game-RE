using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal abstract class ValueQuery : Query
{
	public sealed override XPathNavigator Current
	{
		get
		{
			throw XPathException.Create("Expression must evaluate to a node-set.");
		}
	}

	public sealed override int CurrentPosition
	{
		get
		{
			throw XPathException.Create("Expression must evaluate to a node-set.");
		}
	}

	public sealed override int Count
	{
		get
		{
			throw XPathException.Create("Expression must evaluate to a node-set.");
		}
	}

	public ValueQuery()
	{
	}

	protected ValueQuery(ValueQuery other)
		: base(other)
	{
	}

	public sealed override void Reset()
	{
	}

	public sealed override XPathNavigator Advance()
	{
		throw XPathException.Create("Expression must evaluate to a node-set.");
	}
}
