namespace System.Xml.Schema;

internal class LocatedActiveAxis : ActiveAxis
{
	private int column;

	internal bool isMatched;

	internal KeySequence Ks;

	internal int Column => column;

	internal LocatedActiveAxis(Asttree astfield, KeySequence ks, int column)
		: base(astfield)
	{
		Ks = ks;
		this.column = column;
		isMatched = false;
	}

	internal void Reactivate(KeySequence ks)
	{
		Reactivate();
		Ks = ks;
	}
}
