namespace System.Xml.Schema;

internal class LeafNode : SyntaxTreeNode
{
	private int pos;

	public int Pos
	{
		get
		{
			return pos;
		}
		set
		{
			pos = value;
		}
	}

	public override bool IsNullable => false;

	public LeafNode(int pos)
	{
		this.pos = pos;
	}

	public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
	{
	}

	public override SyntaxTreeNode Clone(Positions positions)
	{
		return new LeafNode(positions.Add(positions[pos].symbol, positions[pos].particle));
	}

	public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
	{
		firstpos.Set(pos);
		lastpos.Set(pos);
	}
}
