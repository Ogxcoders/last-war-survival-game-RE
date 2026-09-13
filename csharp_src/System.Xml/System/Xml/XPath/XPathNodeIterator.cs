using System.Collections;
using System.Diagnostics;
using System.Text;

namespace System.Xml.XPath;

[DebuggerDisplay("Position={CurrentPosition}, Current={debuggerDisplayProxy}")]
public abstract class XPathNodeIterator : ICloneable, IEnumerable
{
	private class Enumerator : IEnumerator
	{
		private XPathNodeIterator original;

		private XPathNodeIterator current;

		private bool iterationStarted;

		public virtual object Current
		{
			get
			{
				if (iterationStarted)
				{
					if (current == null)
					{
						throw new InvalidOperationException(Res.GetString("Enumeration has already finished.", string.Empty));
					}
					return current.Current.Clone();
				}
				throw new InvalidOperationException(Res.GetString("Enumeration has not started. Call MoveNext.", string.Empty));
			}
		}

		public Enumerator(XPathNodeIterator original)
		{
			this.original = original.Clone();
		}

		public virtual bool MoveNext()
		{
			if (!iterationStarted)
			{
				current = original.Clone();
				iterationStarted = true;
			}
			if (current == null || !current.MoveNext())
			{
				current = null;
				return false;
			}
			return true;
		}

		public virtual void Reset()
		{
			iterationStarted = false;
		}
	}

	private struct DebuggerDisplayProxy
	{
		private XPathNodeIterator nodeIterator;

		public DebuggerDisplayProxy(XPathNodeIterator nodeIterator)
		{
			this.nodeIterator = nodeIterator;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Position=");
			stringBuilder.Append(nodeIterator.CurrentPosition);
			stringBuilder.Append(", Current=");
			if (nodeIterator.Current == null)
			{
				stringBuilder.Append("null");
			}
			else
			{
				stringBuilder.Append('{');
				stringBuilder.Append(new XPathNavigator.DebuggerDisplayProxy(nodeIterator.Current).ToString());
				stringBuilder.Append('}');
			}
			return stringBuilder.ToString();
		}
	}

	internal int count = -1;

	public abstract XPathNavigator Current { get; }

	public abstract int CurrentPosition { get; }

	public virtual int Count
	{
		get
		{
			if (count == -1)
			{
				XPathNodeIterator xPathNodeIterator = Clone();
				while (xPathNodeIterator.MoveNext())
				{
				}
				count = xPathNodeIterator.CurrentPosition;
			}
			return count;
		}
	}

	private object debuggerDisplayProxy
	{
		get
		{
			if (Current != null)
			{
				return new XPathNavigator.DebuggerDisplayProxy(Current);
			}
			return null;
		}
	}

	object ICloneable.Clone()
	{
		return Clone();
	}

	public abstract XPathNodeIterator Clone();

	public abstract bool MoveNext();

	public virtual IEnumerator GetEnumerator()
	{
		return new Enumerator(this);
	}
}
