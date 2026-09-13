using MS.Internal.Xml.XPath;

namespace System.Xml.Schema;

internal class DoubleLinkAxis : Axis
{
	internal Axis next;

	internal Axis Next
	{
		get
		{
			return next;
		}
		set
		{
			next = value;
		}
	}

	internal DoubleLinkAxis(Axis axis, DoubleLinkAxis inputaxis)
		: base(axis.TypeOfAxis, inputaxis, axis.Prefix, axis.Name, axis.NodeType)
	{
		next = null;
		base.Urn = axis.Urn;
		abbrAxis = axis.AbbrAxis;
		if (inputaxis != null)
		{
			inputaxis.Next = this;
		}
	}

	internal static DoubleLinkAxis ConvertTree(Axis axis)
	{
		if (axis == null)
		{
			return null;
		}
		return new DoubleLinkAxis(axis, ConvertTree((Axis)axis.Input));
	}
}
