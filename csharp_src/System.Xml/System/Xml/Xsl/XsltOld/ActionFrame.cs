using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl.XsltOld.Debugger;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class ActionFrame : IStackFrame
{
	private class XPathSortArrayIterator : XPathArrayIterator
	{
		public override XPathNavigator Current => ((SortKey)list[index - 1]).Node;

		public XPathSortArrayIterator(List<SortKey> list)
			: base(list)
		{
		}

		public XPathSortArrayIterator(XPathSortArrayIterator it)
			: base(it)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new XPathSortArrayIterator(this);
		}
	}

	private int state;

	private int counter;

	private object[] variables;

	private Hashtable withParams;

	private Action action;

	private ActionFrame container;

	private int currentAction;

	private XPathNodeIterator nodeSet;

	private XPathNodeIterator newNodeSet;

	private PrefixQName calulatedName;

	private string storedOutput;

	internal PrefixQName CalulatedName
	{
		get
		{
			return calulatedName;
		}
		set
		{
			calulatedName = value;
		}
	}

	internal string StoredOutput
	{
		get
		{
			return storedOutput;
		}
		set
		{
			storedOutput = value;
		}
	}

	internal int State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}

	internal int Counter
	{
		get
		{
			return counter;
		}
		set
		{
			counter = value;
		}
	}

	internal ActionFrame Container => container;

	internal XPathNavigator Node
	{
		get
		{
			if (nodeSet != null)
			{
				return nodeSet.Current;
			}
			return null;
		}
	}

	internal XPathNodeIterator NodeSet => nodeSet;

	internal XPathNodeIterator NewNodeSet => newNodeSet;

	XPathNavigator IStackFrame.Instruction
	{
		get
		{
			if (action == null)
			{
				return null;
			}
			return action.GetDbgData(this).StyleSheet;
		}
	}

	XPathNodeIterator IStackFrame.NodeSet => nodeSet.Clone();

	internal int IncrementCounter()
	{
		return ++counter;
	}

	internal void AllocateVariables(int count)
	{
		if (0 < count)
		{
			variables = new object[count];
		}
		else
		{
			variables = null;
		}
	}

	internal object GetVariable(int index)
	{
		return variables[index];
	}

	internal void SetVariable(int index, object value)
	{
		variables[index] = value;
	}

	internal void SetParameter(XmlQualifiedName name, object value)
	{
		if (withParams == null)
		{
			withParams = new Hashtable();
		}
		withParams[name] = value;
	}

	internal void ResetParams()
	{
		if (withParams != null)
		{
			withParams.Clear();
		}
	}

	internal object GetParameter(XmlQualifiedName name)
	{
		if (withParams != null)
		{
			return withParams[name];
		}
		return null;
	}

	internal void InitNodeSet(XPathNodeIterator nodeSet)
	{
		this.nodeSet = nodeSet;
	}

	internal void InitNewNodeSet(XPathNodeIterator nodeSet)
	{
		newNodeSet = nodeSet;
	}

	internal void SortNewNodeSet(Processor proc, ArrayList sortarray)
	{
		int count = sortarray.Count;
		XPathSortComparer xPathSortComparer = new XPathSortComparer(count);
		for (int i = 0; i < count; i++)
		{
			Sort sort = (Sort)sortarray[i];
			Query compiledQuery = proc.GetCompiledQuery(sort.select);
			xPathSortComparer.AddSort(compiledQuery, new XPathComparerHelper(sort.order, sort.caseOrder, sort.lang, sort.dataType));
		}
		List<SortKey> list = new List<SortKey>();
		while (NewNextNode(proc))
		{
			XPathNodeIterator xPathNodeIterator = nodeSet;
			nodeSet = newNodeSet;
			SortKey sortKey = new SortKey(count, list.Count, newNodeSet.Current.Clone());
			for (int j = 0; j < count; j++)
			{
				sortKey[j] = xPathSortComparer.Expression(j).Evaluate(newNodeSet);
			}
			list.Add(sortKey);
			nodeSet = xPathNodeIterator;
		}
		list.Sort(xPathSortComparer);
		newNodeSet = new XPathSortArrayIterator(list);
	}

	internal void Finished()
	{
		State = -1;
	}

	internal void Inherit(ActionFrame parent)
	{
		variables = parent.variables;
	}

	private void Init(Action action, ActionFrame container, XPathNodeIterator nodeSet)
	{
		state = 0;
		this.action = action;
		this.container = container;
		currentAction = 0;
		this.nodeSet = nodeSet;
		newNodeSet = null;
	}

	internal void Init(Action action, XPathNodeIterator nodeSet)
	{
		Init(action, null, nodeSet);
	}

	internal void Init(ActionFrame containerFrame, XPathNodeIterator nodeSet)
	{
		Init(containerFrame.GetAction(0), containerFrame, nodeSet);
	}

	internal void SetAction(Action action)
	{
		SetAction(action, 0);
	}

	internal void SetAction(Action action, int state)
	{
		this.action = action;
		this.state = state;
	}

	private Action GetAction(int actionIndex)
	{
		return ((ContainerAction)action).GetAction(actionIndex);
	}

	internal void Exit()
	{
		Finished();
		container = null;
	}

	internal bool Execute(Processor processor)
	{
		if (action == null)
		{
			return true;
		}
		action.Execute(processor, this);
		if (State == -1)
		{
			if (container != null)
			{
				currentAction++;
				action = container.GetAction(currentAction);
				State = 0;
			}
			else
			{
				action = null;
			}
			return action == null;
		}
		return false;
	}

	internal bool NextNode(Processor proc)
	{
		bool flag = nodeSet.MoveNext();
		if (flag && proc.Stylesheet.Whitespace)
		{
			XPathNodeType nodeType = nodeSet.Current.NodeType;
			if (nodeType == XPathNodeType.Whitespace)
			{
				XPathNavigator xPathNavigator = nodeSet.Current.Clone();
				bool flag2;
				do
				{
					xPathNavigator.MoveTo(nodeSet.Current);
					xPathNavigator.MoveToParent();
					flag2 = !proc.Stylesheet.PreserveWhiteSpace(proc, xPathNavigator) && (flag = nodeSet.MoveNext());
					nodeType = nodeSet.Current.NodeType;
				}
				while (flag2 && nodeType == XPathNodeType.Whitespace);
			}
		}
		return flag;
	}

	internal bool NewNextNode(Processor proc)
	{
		bool flag = newNodeSet.MoveNext();
		if (flag && proc.Stylesheet.Whitespace)
		{
			XPathNodeType nodeType = newNodeSet.Current.NodeType;
			if (nodeType == XPathNodeType.Whitespace)
			{
				XPathNavigator xPathNavigator = newNodeSet.Current.Clone();
				bool flag2;
				do
				{
					xPathNavigator.MoveTo(newNodeSet.Current);
					xPathNavigator.MoveToParent();
					flag2 = !proc.Stylesheet.PreserveWhiteSpace(proc, xPathNavigator) && (flag = newNodeSet.MoveNext());
					nodeType = newNodeSet.Current.NodeType;
				}
				while (flag2 && nodeType == XPathNodeType.Whitespace);
			}
		}
		return flag;
	}

	int IStackFrame.GetVariablesCount()
	{
		if (action == null)
		{
			return 0;
		}
		return action.GetDbgData(this).Variables.Length;
	}

	XPathNavigator IStackFrame.GetVariable(int varIndex)
	{
		return action.GetDbgData(this).Variables[varIndex].GetDbgData(null).StyleSheet;
	}

	object IStackFrame.GetVariableValue(int varIndex)
	{
		return GetVariable(action.GetDbgData(this).Variables[varIndex].VarKey);
	}
}
