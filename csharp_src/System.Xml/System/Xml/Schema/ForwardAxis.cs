namespace System.Xml.Schema;

internal class ForwardAxis
{
	private DoubleLinkAxis _topNode;

	private DoubleLinkAxis _rootNode;

	private bool _isAttribute;

	private bool _isDss;

	private bool _isSelfAxis;

	internal DoubleLinkAxis RootNode => _rootNode;

	internal DoubleLinkAxis TopNode => _topNode;

	internal bool IsAttribute => _isAttribute;

	internal bool IsDss => _isDss;

	internal bool IsSelfAxis => _isSelfAxis;

	public ForwardAxis(DoubleLinkAxis axis, bool isdesorself)
	{
		_isDss = isdesorself;
		_isAttribute = Asttree.IsAttribute(axis);
		_topNode = axis;
		_rootNode = axis;
		while (_rootNode.Input != null)
		{
			_rootNode = (DoubleLinkAxis)_rootNode.Input;
		}
		_isSelfAxis = Asttree.IsSelf(_topNode);
	}
}
