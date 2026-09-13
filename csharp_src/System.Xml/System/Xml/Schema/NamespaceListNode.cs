using System.Collections;

namespace System.Xml.Schema;

internal class NamespaceListNode : SyntaxTreeNode
{
	protected NamespaceList namespaceList;

	protected object particle;

	public override bool IsNullable
	{
		get
		{
			throw new InvalidOperationException();
		}
	}

	public NamespaceListNode(NamespaceList namespaceList, object particle)
	{
		this.namespaceList = namespaceList;
		this.particle = particle;
	}

	public override SyntaxTreeNode Clone(Positions positions)
	{
		throw new InvalidOperationException();
	}

	public virtual ICollection GetResolvedSymbols(SymbolsDictionary symbols)
	{
		return symbols.GetNamespaceListSymbols(namespaceList);
	}

	public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
	{
		SyntaxTreeNode syntaxTreeNode = null;
		foreach (int resolvedSymbol in GetResolvedSymbols(symbols))
		{
			if (symbols.GetParticle(resolvedSymbol) != particle)
			{
				symbols.IsUpaEnforced = false;
			}
			LeafNode leafNode = new LeafNode(positions.Add(resolvedSymbol, particle));
			syntaxTreeNode = ((syntaxTreeNode != null) ? ((SyntaxTreeNode)new ChoiceNode
			{
				LeftChild = syntaxTreeNode,
				RightChild = leafNode
			}) : ((SyntaxTreeNode)leafNode));
		}
		if (parent.LeftChild == this)
		{
			parent.LeftChild = syntaxTreeNode;
		}
		else
		{
			parent.RightChild = syntaxTreeNode;
		}
	}

	public override void ConstructPos(BitSet firstpos, BitSet lastpos, BitSet[] followpos)
	{
		throw new InvalidOperationException();
	}
}
