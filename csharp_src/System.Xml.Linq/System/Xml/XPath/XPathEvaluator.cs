using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace System.Xml.XPath;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal readonly struct XPathEvaluator
{
	public object Evaluate<T>(XNode node, string expression, IXmlNamespaceResolver resolver) where T : class
	{
		object obj = node.CreateNavigator().Evaluate(expression, resolver);
		if (obj is XPathNodeIterator result)
		{
			return EvaluateIterator<T>(result);
		}
		if (!(obj is T))
		{
			throw new InvalidOperationException(global::SR.Format("The XPath expression evaluated to unexpected type {0}.", obj.GetType()));
		}
		return (T)obj;
	}

	private IEnumerable<T> EvaluateIterator<T>(XPathNodeIterator result)
	{
		foreach (XPathNavigator item in result)
		{
			object r = item.UnderlyingObject;
			if (!(r is T))
			{
				throw new InvalidOperationException(global::SR.Format("The XPath expression evaluated to unexpected type {0}.", r.GetType()));
			}
			yield return (T)r;
			XText t = r as XText;
			if (t == null || t.GetParent() == null)
			{
				continue;
			}
			do
			{
				t = t.NextNode as XText;
				if (t == null)
				{
					break;
				}
				yield return (T)(object)t;
			}
			while (t != t.GetParent().LastNode);
		}
	}
}
