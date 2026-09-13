using System.Collections.Generic;

namespace System.Xml.Schema;

internal abstract class InteriorNode : SyntaxTreeNode
{
	private SyntaxTreeNode leftChild;

	private SyntaxTreeNode rightChild;

	public SyntaxTreeNode LeftChild
	{
		get
		{
			return leftChild;
		}
		set
		{
			leftChild = value;
		}
	}

	public SyntaxTreeNode RightChild
	{
		get
		{
			return rightChild;
		}
		set
		{
			rightChild = value;
		}
	}

	public override SyntaxTreeNode Clone(Positions positions)
	{
		InteriorNode interiorNode = (InteriorNode)MemberwiseClone();
		interiorNode.LeftChild = leftChild.Clone(positions);
		if (rightChild != null)
		{
			interiorNode.RightChild = rightChild.Clone(positions);
		}
		return interiorNode;
	}

	protected void ExpandTreeNoRecursive(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
	{
		Stack<InteriorNode> stack = new Stack<InteriorNode>();
		InteriorNode interiorNode = this;
		while (interiorNode.leftChild is ChoiceNode || interiorNode.leftChild is SequenceNode)
		{
			stack.Push(interiorNode);
			interiorNode = (InteriorNode)interiorNode.leftChild;
		}
		interiorNode.leftChild.ExpandTree(interiorNode, symbols, positions);
		while (true)
		{
			if (interiorNode.rightChild != null)
			{
				interiorNode.rightChild.ExpandTree(interiorNode, symbols, positions);
			}
			if (stack.Count != 0)
			{
				interiorNode = stack.Pop();
				continue;
			}
			break;
		}
	}

	public override void ExpandTree(InteriorNode parent, SymbolsDictionary symbols, Positions positions)
	{
		leftChild.ExpandTree(this, symbols, positions);
		if (rightChild != null)
		{
			rightChild.ExpandTree(this, symbols, positions);
		}
	}
}
