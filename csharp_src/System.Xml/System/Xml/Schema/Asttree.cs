using System.Collections;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Schema;

internal class Asttree
{
	private ArrayList _fAxisArray;

	private string _xpathexpr;

	private bool _isField;

	private XmlNamespaceManager _nsmgr;

	internal ArrayList SubtreeArray => _fAxisArray;

	public Asttree(string xPath, bool isField, XmlNamespaceManager nsmgr)
	{
		_xpathexpr = xPath;
		_isField = isField;
		_nsmgr = nsmgr;
		CompileXPath(xPath, isField, nsmgr);
	}

	private static bool IsNameTest(Axis ast)
	{
		if (ast.TypeOfAxis == Axis.AxisType.Child)
		{
			return ast.NodeType == XPathNodeType.Element;
		}
		return false;
	}

	internal static bool IsAttribute(Axis ast)
	{
		if (ast.TypeOfAxis == Axis.AxisType.Attribute)
		{
			return ast.NodeType == XPathNodeType.Attribute;
		}
		return false;
	}

	private static bool IsDescendantOrSelf(Axis ast)
	{
		if (ast.TypeOfAxis == Axis.AxisType.DescendantOrSelf && ast.NodeType == XPathNodeType.All)
		{
			return ast.AbbrAxis;
		}
		return false;
	}

	internal static bool IsSelf(Axis ast)
	{
		if (ast.TypeOfAxis == Axis.AxisType.Self && ast.NodeType == XPathNodeType.All)
		{
			return ast.AbbrAxis;
		}
		return false;
	}

	public void CompileXPath(string xPath, bool isField, XmlNamespaceManager nsmgr)
	{
		if (xPath == null || xPath.Length == 0)
		{
			throw new XmlSchemaException("The XPath for selector or field cannot be empty.", string.Empty);
		}
		string[] array = xPath.Split('|');
		ArrayList arrayList = new ArrayList(array.Length);
		_fAxisArray = new ArrayList(array.Length);
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				Axis value = (Axis)XPathParser.ParseXPathExpression(array[i]);
				arrayList.Add(value);
			}
		}
		catch
		{
			throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Axis axis = (Axis)arrayList[j];
			Axis axis2;
			if ((axis2 = axis) == null)
			{
				throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
			}
			Axis axis3 = axis2;
			if (IsAttribute(axis2))
			{
				if (!isField)
				{
					throw new XmlSchemaException("'{0}' is an invalid XPath for selector. Selector cannot have an XPath selection with an attribute node.", xPath);
				}
				SetURN(axis2, nsmgr);
				try
				{
					axis2 = (Axis)axis2.Input;
				}
				catch
				{
					throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
				}
			}
			while (axis2 != null && (IsNameTest(axis2) || IsSelf(axis2)))
			{
				if (IsSelf(axis2) && axis != axis2)
				{
					axis3.Input = axis2.Input;
				}
				else
				{
					axis3 = axis2;
					if (IsNameTest(axis2))
					{
						SetURN(axis2, nsmgr);
					}
				}
				try
				{
					axis2 = (Axis)axis2.Input;
				}
				catch
				{
					throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
				}
			}
			axis3.Input = null;
			if (axis2 == null)
			{
				if (IsSelf(axis) && axis.Input != null)
				{
					_fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis.Input), isdesorself: false));
				}
				else
				{
					_fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis), isdesorself: false));
				}
				continue;
			}
			if (!IsDescendantOrSelf(axis2))
			{
				throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
			}
			try
			{
				axis2 = (Axis)axis2.Input;
			}
			catch
			{
				throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
			}
			if (axis2 == null || !IsSelf(axis2) || axis2.Input != null)
			{
				throw new XmlSchemaException("'{0}' is an invalid XPath for selector or field.", xPath);
			}
			if (IsSelf(axis) && axis.Input != null)
			{
				_fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis.Input), isdesorself: true));
			}
			else
			{
				_fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis), isdesorself: true));
			}
		}
	}

	private void SetURN(Axis axis, XmlNamespaceManager nsmgr)
	{
		if (axis.Prefix.Length != 0)
		{
			axis.Urn = nsmgr.LookupNamespace(axis.Prefix);
			if (axis.Urn == null)
			{
				throw new XmlSchemaException("The prefix '{0}' in XPath cannot be resolved.", axis.Prefix);
			}
		}
		else if (axis.Name.Length != 0)
		{
			axis.Urn = null;
		}
		else
		{
			axis.Urn = "";
		}
	}
}
