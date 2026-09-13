using System.Collections;

namespace System.Xml.Schema;

internal class Positions
{
	private ArrayList positions = new ArrayList();

	public Position this[int pos] => (Position)positions[pos];

	public int Count => positions.Count;

	public int Add(int symbol, object particle)
	{
		return positions.Add(new Position(symbol, particle));
	}
}
