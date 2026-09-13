using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Schema;

internal sealed class RangeContentValidator : ContentValidator
{
	private BitSet firstpos;

	private BitSet[] followpos;

	private BitSet positionsWithRangeTerminals;

	private SymbolsDictionary symbols;

	private Positions positions;

	private int minMaxNodesCount;

	private int endMarkerPos;

	internal RangeContentValidator(BitSet firstpos, BitSet[] followpos, SymbolsDictionary symbols, Positions positions, int endMarkerPos, XmlSchemaContentType contentType, bool isEmptiable, BitSet positionsWithRangeTerminals, int minmaxNodesCount)
		: base(contentType, isOpen: false, isEmptiable)
	{
		this.firstpos = firstpos;
		this.followpos = followpos;
		this.symbols = symbols;
		this.positions = positions;
		this.positionsWithRangeTerminals = positionsWithRangeTerminals;
		minMaxNodesCount = minmaxNodesCount;
		this.endMarkerPos = endMarkerPos;
	}

	public override void InitValidation(ValidationState context)
	{
		_ = positions.Count;
		List<RangePositionInfo> list = context.RunningPositions;
		if (list == null)
		{
			list = (context.RunningPositions = new List<RangePositionInfo>());
		}
		else
		{
			list.Clear();
		}
		RangePositionInfo item = new RangePositionInfo
		{
			curpos = firstpos.Clone(),
			rangeCounters = new decimal[minMaxNodesCount]
		};
		list.Add(item);
		context.CurrentState.NumberOfRunningPos = 1;
		context.HasMatched = item.curpos.Get(endMarkerPos);
	}

	public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
	{
		errorCode = 0;
		int num = symbols[name];
		bool flag = false;
		List<RangePositionInfo> runningPositions = context.RunningPositions;
		int numberOfRunningPos = context.CurrentState.NumberOfRunningPos;
		int i = 0;
		int num2 = -1;
		int num3 = -1;
		bool flag2 = false;
		for (; i < numberOfRunningPos; i++)
		{
			BitSet curpos = runningPositions[i].curpos;
			for (int num4 = curpos.NextSet(-1); num4 != -1; num4 = curpos.NextSet(num4))
			{
				if (num == positions[num4].symbol)
				{
					num2 = num4;
					if (num3 == -1)
					{
						num3 = i;
					}
					flag2 = true;
					break;
				}
			}
			if (flag2 && positions[num2].particle is XmlSchemaElement)
			{
				break;
			}
		}
		if (i == numberOfRunningPos && num2 != -1)
		{
			i = num3;
		}
		if (i < numberOfRunningPos)
		{
			if (i != 0)
			{
				runningPositions.RemoveRange(0, i);
			}
			numberOfRunningPos -= i;
			i = 0;
			while (i < numberOfRunningPos)
			{
				RangePositionInfo value = runningPositions[i];
				if (value.curpos.Get(num2))
				{
					value.curpos = followpos[num2];
					runningPositions[i] = value;
					i++;
					continue;
				}
				numberOfRunningPos--;
				if (numberOfRunningPos > 0)
				{
					RangePositionInfo value2 = runningPositions[numberOfRunningPos];
					runningPositions[numberOfRunningPos] = runningPositions[i];
					runningPositions[i] = value2;
				}
			}
		}
		else
		{
			numberOfRunningPos = 0;
		}
		if (numberOfRunningPos > 0)
		{
			if (numberOfRunningPos >= 10000)
			{
				context.TooComplex = true;
				numberOfRunningPos /= 2;
			}
			for (i = numberOfRunningPos - 1; i >= 0; i--)
			{
				int index = i;
				BitSet curpos2 = runningPositions[i].curpos;
				flag = flag || curpos2.Get(endMarkerPos);
				while (numberOfRunningPos < 10000 && curpos2.Intersects(positionsWithRangeTerminals))
				{
					BitSet bitSet = curpos2.Clone();
					bitSet.And(positionsWithRangeTerminals);
					int num5 = bitSet.NextSet(-1);
					LeafRangeNode leafRangeNode = positions[num5].particle as LeafRangeNode;
					RangePositionInfo value = runningPositions[index];
					if (numberOfRunningPos + 2 >= runningPositions.Count)
					{
						runningPositions.Add(default(RangePositionInfo));
						runningPositions.Add(default(RangePositionInfo));
					}
					RangePositionInfo value3 = runningPositions[numberOfRunningPos];
					if (value3.rangeCounters == null)
					{
						value3.rangeCounters = new decimal[minMaxNodesCount];
					}
					Array.Copy(value.rangeCounters, 0, value3.rangeCounters, 0, value.rangeCounters.Length);
					decimal num6 = ++value3.rangeCounters[leafRangeNode.Pos];
					if (num6 == leafRangeNode.Max)
					{
						value3.curpos = followpos[num5];
						value3.rangeCounters[leafRangeNode.Pos] = default(decimal);
						runningPositions[numberOfRunningPos] = value3;
						index = numberOfRunningPos++;
					}
					else
					{
						if (num6 < leafRangeNode.Min)
						{
							value3.curpos = leafRangeNode.NextIteration;
							runningPositions[numberOfRunningPos] = value3;
							numberOfRunningPos++;
							break;
						}
						value3.curpos = leafRangeNode.NextIteration;
						runningPositions[numberOfRunningPos] = value3;
						index = numberOfRunningPos + 1;
						value3 = runningPositions[index];
						if (value3.rangeCounters == null)
						{
							value3.rangeCounters = new decimal[minMaxNodesCount];
						}
						Array.Copy(value.rangeCounters, 0, value3.rangeCounters, 0, value.rangeCounters.Length);
						value3.curpos = followpos[num5];
						value3.rangeCounters[leafRangeNode.Pos] = default(decimal);
						runningPositions[index] = value3;
						numberOfRunningPos += 2;
					}
					curpos2 = runningPositions[index].curpos;
					flag = flag || curpos2.Get(endMarkerPos);
				}
			}
			context.HasMatched = flag;
			context.CurrentState.NumberOfRunningPos = numberOfRunningPos;
			return positions[num2].particle;
		}
		errorCode = -1;
		context.NeedValidateChildren = false;
		return null;
	}

	public override bool CompleteValidation(ValidationState context)
	{
		return context.HasMatched;
	}

	public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
	{
		ArrayList arrayList = null;
		if (context.RunningPositions != null)
		{
			List<RangePositionInfo> runningPositions = context.RunningPositions;
			BitSet bitSet = new BitSet(positions.Count);
			for (int num = context.CurrentState.NumberOfRunningPos - 1; num >= 0; num--)
			{
				bitSet.Or(runningPositions[num].curpos);
			}
			for (int num2 = bitSet.NextSet(-1); num2 != -1; num2 = bitSet.NextSet(num2))
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				if (positions[num2].symbol >= 0)
				{
					if (!(positions[num2].particle is XmlSchemaParticle { NameString: var nameString }))
					{
						string text = symbols.NameOf(positions[num2].symbol);
						if (text.Length != 0)
						{
							arrayList.Add(text);
						}
					}
					else if (!arrayList.Contains(nameString))
					{
						arrayList.Add(nameString);
					}
				}
			}
		}
		return arrayList;
	}

	public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly, XmlSchemaSet schemaSet)
	{
		ArrayList arrayList = new ArrayList();
		if (context.RunningPositions != null)
		{
			List<RangePositionInfo> runningPositions = context.RunningPositions;
			BitSet bitSet = new BitSet(positions.Count);
			for (int num = context.CurrentState.NumberOfRunningPos - 1; num >= 0; num--)
			{
				bitSet.Or(runningPositions[num].curpos);
			}
			for (int num2 = bitSet.NextSet(-1); num2 != -1; num2 = bitSet.NextSet(num2))
			{
				if (positions[num2].symbol >= 0 && positions[num2].particle is XmlSchemaParticle p)
				{
					ContentValidator.AddParticleToExpected(p, schemaSet, arrayList);
				}
			}
		}
		return arrayList;
	}
}
