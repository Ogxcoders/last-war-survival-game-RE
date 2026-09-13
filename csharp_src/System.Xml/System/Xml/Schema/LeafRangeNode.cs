namespace System.Xml.Schema;

internal sealed class LeafRangeNode : LeafNode
{
	private decimal min;

	private decimal max;

	private BitSet nextIteration;

	public decimal Max => max;

	public decimal Min => min;

	public BitSet NextIteration
	{
		get
		{
			return nextIteration;
		}
		set
		{
			nextIteration = value;
		}
	}

	public override bool IsRangeNode => true;

	public LeafRangeNode(decimal min, decimal max)
		: this(-1, min, max)
	{
	}

	public LeafRangeNode(int pos, decimal min, decimal max)
		: base(pos)
	{
		this.min = min;
		this.max = max;
	}

	public override SyntaxTreeNode Clone(Positions positions)
	{
		return new LeafRangeNode(base.Pos, min, max);
	}

	public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
	{
		if (parent.LeftChild.IsNullable)
		{
			min = default(decimal);
		}
	}
}
