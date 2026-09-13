using System.Collections;

namespace System.Xml.Schema;

internal sealed class ParticleContentValidator : ContentValidator
{
	private SymbolsDictionary symbols;

	private Positions positions;

	private Stack stack;

	private SyntaxTreeNode contentNode;

	private bool isPartial;

	private int minMaxNodesCount;

	private bool enableUpaCheck;

	public ParticleContentValidator(XmlSchemaContentType contentType)
		: this(contentType, enableUpaCheck: true)
	{
	}

	public ParticleContentValidator(XmlSchemaContentType contentType, bool enableUpaCheck)
		: base(contentType)
	{
		this.enableUpaCheck = enableUpaCheck;
	}

	public override void InitValidation(ValidationState context)
	{
		throw new InvalidOperationException();
	}

	public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
	{
		throw new InvalidOperationException();
	}

	public override bool CompleteValidation(ValidationState context)
	{
		throw new InvalidOperationException();
	}

	public void Start()
	{
		symbols = new SymbolsDictionary();
		positions = new Positions();
		stack = new Stack();
	}

	public void OpenGroup()
	{
		stack.Push(null);
	}

	public void CloseGroup()
	{
		SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)stack.Pop();
		if (syntaxTreeNode == null)
		{
			return;
		}
		if (stack.Count == 0)
		{
			contentNode = syntaxTreeNode;
			isPartial = false;
			return;
		}
		InteriorNode interiorNode = (InteriorNode)stack.Pop();
		if (interiorNode != null)
		{
			interiorNode.RightChild = syntaxTreeNode;
			syntaxTreeNode = interiorNode;
			isPartial = true;
		}
		else
		{
			isPartial = false;
		}
		stack.Push(syntaxTreeNode);
	}

	public bool Exists(XmlQualifiedName name)
	{
		if (symbols.Exists(name))
		{
			return true;
		}
		return false;
	}

	public void AddName(XmlQualifiedName name, object particle)
	{
		AddLeafNode(new LeafNode(positions.Add(symbols.AddName(name, particle), particle)));
	}

	public void AddNamespaceList(NamespaceList namespaceList, object particle)
	{
		symbols.AddNamespaceList(namespaceList, particle, allowLocal: false);
		AddLeafNode(new NamespaceListNode(namespaceList, particle));
	}

	private void AddLeafNode(SyntaxTreeNode node)
	{
		if (stack.Count > 0)
		{
			InteriorNode interiorNode = (InteriorNode)stack.Pop();
			if (interiorNode != null)
			{
				interiorNode.RightChild = node;
				node = interiorNode;
			}
		}
		stack.Push(node);
		isPartial = true;
	}

	public void AddChoice()
	{
		SyntaxTreeNode leftChild = (SyntaxTreeNode)stack.Pop();
		InteriorNode interiorNode = new ChoiceNode();
		interiorNode.LeftChild = leftChild;
		stack.Push(interiorNode);
	}

	public void AddSequence()
	{
		SyntaxTreeNode leftChild = (SyntaxTreeNode)stack.Pop();
		InteriorNode interiorNode = new SequenceNode();
		interiorNode.LeftChild = leftChild;
		stack.Push(interiorNode);
	}

	public void AddStar()
	{
		Closure(new StarNode());
	}

	public void AddPlus()
	{
		Closure(new PlusNode());
	}

	public void AddQMark()
	{
		Closure(new QmarkNode());
	}

	public void AddLeafRange(decimal min, decimal max)
	{
		LeafRangeNode leafRangeNode = new LeafRangeNode(min, max);
		int pos = positions.Add(-2, leafRangeNode);
		leafRangeNode.Pos = pos;
		InteriorNode interiorNode = new SequenceNode();
		interiorNode.RightChild = leafRangeNode;
		Closure(interiorNode);
		minMaxNodesCount++;
	}

	private void Closure(InteriorNode node)
	{
		if (stack.Count > 0)
		{
			SyntaxTreeNode syntaxTreeNode = (SyntaxTreeNode)stack.Pop();
			InteriorNode interiorNode = syntaxTreeNode as InteriorNode;
			if (isPartial && interiorNode != null)
			{
				node.LeftChild = interiorNode.RightChild;
				interiorNode.RightChild = node;
			}
			else
			{
				node.LeftChild = syntaxTreeNode;
				syntaxTreeNode = node;
			}
			stack.Push(syntaxTreeNode);
		}
		else if (contentNode != null)
		{
			node.LeftChild = contentNode;
			contentNode = node;
		}
	}

	public ContentValidator Finish()
	{
		return Finish(useDFA: true);
	}

	public ContentValidator Finish(bool useDFA)
	{
		if (contentNode == null)
		{
			if (base.ContentType == XmlSchemaContentType.Mixed)
			{
				_ = base.IsOpen;
				if (!base.IsOpen)
				{
					return ContentValidator.TextOnly;
				}
				return ContentValidator.Any;
			}
			return ContentValidator.Empty;
		}
		InteriorNode interiorNode = new SequenceNode();
		interiorNode.LeftChild = contentNode;
		LeafNode leafNode = (LeafNode)(interiorNode.RightChild = new LeafNode(positions.Add(symbols.AddName(XmlQualifiedName.Empty, null), null)));
		contentNode.ExpandTree(interiorNode, symbols, positions);
		_ = symbols.Count;
		int count = positions.Count;
		BitSet bitSet = new BitSet(count);
		BitSet lastpos = new BitSet(count);
		BitSet[] array = new BitSet[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new BitSet(count);
		}
		interiorNode.ConstructPos(bitSet, lastpos, array);
		if (minMaxNodesCount > 0)
		{
			BitSet posWithRangeTerminals;
			BitSet[] minmaxFollowPos = CalculateTotalFollowposForRangeNodes(bitSet, array, out posWithRangeTerminals);
			if (enableUpaCheck)
			{
				CheckCMUPAWithLeafRangeNodes(GetApplicableMinMaxFollowPos(bitSet, posWithRangeTerminals, minmaxFollowPos));
				for (int j = 0; j < count; j++)
				{
					CheckCMUPAWithLeafRangeNodes(GetApplicableMinMaxFollowPos(array[j], posWithRangeTerminals, minmaxFollowPos));
				}
			}
			return new RangeContentValidator(bitSet, array, symbols, positions, leafNode.Pos, base.ContentType, interiorNode.LeftChild.IsNullable, posWithRangeTerminals, minMaxNodesCount);
		}
		int[][] array2 = null;
		if (!symbols.IsUpaEnforced)
		{
			if (enableUpaCheck)
			{
				CheckUniqueParticleAttribution(bitSet, array);
			}
		}
		else if (useDFA)
		{
			array2 = BuildTransitionTable(bitSet, array, leafNode.Pos);
		}
		if (array2 != null)
		{
			return new DfaContentValidator(array2, symbols, base.ContentType, base.IsOpen, interiorNode.LeftChild.IsNullable);
		}
		return new NfaContentValidator(bitSet, array, symbols, positions, leafNode.Pos, base.ContentType, base.IsOpen, interiorNode.LeftChild.IsNullable);
	}

	private BitSet[] CalculateTotalFollowposForRangeNodes(BitSet firstpos, BitSet[] followpos, out BitSet posWithRangeTerminals)
	{
		int count = positions.Count;
		posWithRangeTerminals = new BitSet(count);
		BitSet[] array = new BitSet[minMaxNodesCount];
		int num = 0;
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			Position position = positions[num2];
			if (position.symbol == -2)
			{
				LeafRangeNode leafRangeNode = position.particle as LeafRangeNode;
				BitSet bitSet = new BitSet(count);
				bitSet.Clear();
				bitSet.Or(followpos[num2]);
				if (leafRangeNode.Min != leafRangeNode.Max)
				{
					bitSet.Or(leafRangeNode.NextIteration);
				}
				for (int num3 = bitSet.NextSet(-1); num3 != -1; num3 = bitSet.NextSet(num3))
				{
					if (num3 > num2)
					{
						Position position2 = positions[num3];
						if (position2.symbol == -2)
						{
							LeafRangeNode leafRangeNode2 = position2.particle as LeafRangeNode;
							bitSet.Or(array[leafRangeNode2.Pos]);
						}
					}
				}
				array[num] = bitSet;
				leafRangeNode.Pos = num++;
				posWithRangeTerminals.Set(num2);
			}
		}
		return array;
	}

	private void CheckCMUPAWithLeafRangeNodes(BitSet curpos)
	{
		object[] array = new object[symbols.Count];
		for (int num = curpos.NextSet(-1); num != -1; num = curpos.NextSet(num))
		{
			Position position = positions[num];
			int symbol = position.symbol;
			if (symbol >= 0)
			{
				if (array[symbol] != null)
				{
					throw new UpaException(array[symbol], position.particle);
				}
				array[symbol] = position.particle;
			}
		}
	}

	private BitSet GetApplicableMinMaxFollowPos(BitSet curpos, BitSet posWithRangeTerminals, BitSet[] minmaxFollowPos)
	{
		if (curpos.Intersects(posWithRangeTerminals))
		{
			BitSet bitSet = new BitSet(positions.Count);
			bitSet.Or(curpos);
			bitSet.And(posWithRangeTerminals);
			curpos = curpos.Clone();
			for (int num = bitSet.NextSet(-1); num != -1; num = bitSet.NextSet(num))
			{
				LeafRangeNode leafRangeNode = positions[num].particle as LeafRangeNode;
				curpos.Or(minmaxFollowPos[leafRangeNode.Pos]);
			}
		}
		return curpos;
	}

	private void CheckUniqueParticleAttribution(BitSet firstpos, BitSet[] followpos)
	{
		CheckUniqueParticleAttribution(firstpos);
		for (int i = 0; i < positions.Count; i++)
		{
			CheckUniqueParticleAttribution(followpos[i]);
		}
	}

	private void CheckUniqueParticleAttribution(BitSet curpos)
	{
		object[] array = new object[symbols.Count];
		for (int num = curpos.NextSet(-1); num != -1; num = curpos.NextSet(num))
		{
			int symbol = positions[num].symbol;
			if (array[symbol] == null)
			{
				array[symbol] = positions[num].particle;
			}
			else if (array[symbol] != positions[num].particle)
			{
				throw new UpaException(array[symbol], positions[num].particle);
			}
		}
	}

	private int[][] BuildTransitionTable(BitSet firstpos, BitSet[] followpos, int endMarkerPos)
	{
		int count = positions.Count;
		int num = 8192 / count;
		int count2 = symbols.Count;
		ArrayList arrayList = new ArrayList();
		Hashtable hashtable = new Hashtable();
		hashtable.Add(new BitSet(count), -1);
		Queue queue = new Queue();
		int num2 = 0;
		queue.Enqueue(firstpos);
		hashtable.Add(firstpos, 0);
		arrayList.Add(new int[count2 + 1]);
		while (queue.Count > 0)
		{
			BitSet bitSet = (BitSet)queue.Dequeue();
			int[] array = (int[])arrayList[num2];
			if (bitSet[endMarkerPos])
			{
				array[count2] = 1;
			}
			for (int i = 0; i < count2; i++)
			{
				BitSet bitSet2 = new BitSet(count);
				for (int num3 = bitSet.NextSet(-1); num3 != -1; num3 = bitSet.NextSet(num3))
				{
					if (i == positions[num3].symbol)
					{
						bitSet2.Or(followpos[num3]);
					}
				}
				object obj = hashtable[bitSet2];
				if (obj != null)
				{
					array[i] = (int)obj;
					continue;
				}
				int num4 = hashtable.Count - 1;
				if (num4 >= num)
				{
					return null;
				}
				queue.Enqueue(bitSet2);
				hashtable.Add(bitSet2, num4);
				arrayList.Add(new int[count2 + 1]);
				array[i] = num4;
			}
			num2++;
		}
		return (int[][])arrayList.ToArray(typeof(int[]));
	}
}
