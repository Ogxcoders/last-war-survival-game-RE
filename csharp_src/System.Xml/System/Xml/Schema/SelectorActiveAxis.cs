using System.Collections;

namespace System.Xml.Schema;

internal class SelectorActiveAxis : ActiveAxis
{
	private ConstraintStruct cs;

	private ArrayList KSs;

	private int KSpointer;

	public bool EmptyStack => KSpointer == 0;

	public int lastDepth
	{
		get
		{
			if (KSpointer != 0)
			{
				return ((KSStruct)KSs[KSpointer - 1]).depth;
			}
			return -1;
		}
	}

	public SelectorActiveAxis(Asttree axisTree, ConstraintStruct cs)
		: base(axisTree)
	{
		KSs = new ArrayList();
		this.cs = cs;
	}

	public override bool EndElement(string localname, string URN)
	{
		base.EndElement(localname, URN);
		if (KSpointer > 0 && base.CurrentDepth == lastDepth)
		{
			return true;
		}
		return false;
	}

	public int PushKS(int errline, int errcol)
	{
		KeySequence ks = new KeySequence(cs.TableDim, errline, errcol);
		KSStruct kSStruct;
		if (KSpointer < KSs.Count)
		{
			kSStruct = (KSStruct)KSs[KSpointer];
			kSStruct.ks = ks;
			for (int i = 0; i < cs.TableDim; i++)
			{
				kSStruct.fields[i].Reactivate(ks);
			}
		}
		else
		{
			kSStruct = new KSStruct(ks, cs.TableDim);
			for (int j = 0; j < cs.TableDim; j++)
			{
				kSStruct.fields[j] = new LocatedActiveAxis(cs.constraint.Fields[j], ks, j);
				cs.axisFields.Add(kSStruct.fields[j]);
			}
			KSs.Add(kSStruct);
		}
		kSStruct.depth = base.CurrentDepth - 1;
		return KSpointer++;
	}

	public KeySequence PopKS()
	{
		return ((KSStruct)KSs[--KSpointer]).ks;
	}
}
