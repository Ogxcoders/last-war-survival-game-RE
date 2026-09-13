using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Yoga;

internal class YogaNode : IEnumerable<YogaNode>, IEnumerable
{
	internal IntPtr _ygNode;

	private readonly YogaConfig _config;

	private WeakReference _parent;

	private List<YogaNode> _children;

	private MeasureFunction _measureFunction;

	private BaselineFunction _baselineFunction;

	private object _data;

	public bool IsDirty => Native.YGNodeIsDirty(_ygNode);

	public bool HasNewLayout => Native.YGNodeGetHasNewLayout(_ygNode);

	public YogaNode Parent => (_parent != null) ? (_parent.Target as YogaNode) : null;

	public bool IsMeasureDefined => _measureFunction != null;

	public bool IsBaselineDefined => _baselineFunction != null;

	public YogaDirection StyleDirection
	{
		get
		{
			return Native.YGNodeStyleGetDirection(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetDirection(_ygNode, value);
		}
	}

	public YogaFlexDirection FlexDirection
	{
		get
		{
			return Native.YGNodeStyleGetFlexDirection(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetFlexDirection(_ygNode, value);
		}
	}

	public YogaJustify JustifyContent
	{
		get
		{
			return Native.YGNodeStyleGetJustifyContent(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetJustifyContent(_ygNode, value);
		}
	}

	public YogaDisplay Display
	{
		get
		{
			return Native.YGNodeStyleGetDisplay(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetDisplay(_ygNode, value);
		}
	}

	public YogaAlign AlignItems
	{
		get
		{
			return Native.YGNodeStyleGetAlignItems(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetAlignItems(_ygNode, value);
		}
	}

	public YogaAlign AlignSelf
	{
		get
		{
			return Native.YGNodeStyleGetAlignSelf(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetAlignSelf(_ygNode, value);
		}
	}

	public YogaAlign AlignContent
	{
		get
		{
			return Native.YGNodeStyleGetAlignContent(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetAlignContent(_ygNode, value);
		}
	}

	public YogaPositionType PositionType
	{
		get
		{
			return Native.YGNodeStyleGetPositionType(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetPositionType(_ygNode, value);
		}
	}

	public YogaWrap Wrap
	{
		get
		{
			return Native.YGNodeStyleGetFlexWrap(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetFlexWrap(_ygNode, value);
		}
	}

	public float Flex
	{
		set
		{
			Native.YGNodeStyleSetFlex(_ygNode, value);
		}
	}

	public float FlexGrow
	{
		get
		{
			return Native.YGNodeStyleGetFlexGrow(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetFlexGrow(_ygNode, value);
		}
	}

	public float FlexShrink
	{
		get
		{
			return Native.YGNodeStyleGetFlexShrink(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetFlexShrink(_ygNode, value);
		}
	}

	public YogaValue FlexBasis
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetFlexBasis(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetFlexBasisPercent(_ygNode, value.Value);
			}
			else if (value.Unit == YogaUnit.Auto)
			{
				Native.YGNodeStyleSetFlexBasisAuto(_ygNode);
			}
			else
			{
				Native.YGNodeStyleSetFlexBasis(_ygNode, value.Value);
			}
		}
	}

	public YogaValue Width
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetWidth(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetWidthPercent(_ygNode, value.Value);
			}
			else if (value.Unit == YogaUnit.Auto)
			{
				Native.YGNodeStyleSetWidthAuto(_ygNode);
			}
			else
			{
				Native.YGNodeStyleSetWidth(_ygNode, value.Value);
			}
		}
	}

	public YogaValue Height
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetHeight(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetHeightPercent(_ygNode, value.Value);
			}
			else if (value.Unit == YogaUnit.Auto)
			{
				Native.YGNodeStyleSetHeightAuto(_ygNode);
			}
			else
			{
				Native.YGNodeStyleSetHeight(_ygNode, value.Value);
			}
		}
	}

	public YogaValue MaxWidth
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMaxWidth(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetMaxWidthPercent(_ygNode, value.Value);
			}
			else
			{
				Native.YGNodeStyleSetMaxWidth(_ygNode, value.Value);
			}
		}
	}

	public YogaValue MaxHeight
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMaxHeight(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetMaxHeightPercent(_ygNode, value.Value);
			}
			else
			{
				Native.YGNodeStyleSetMaxHeight(_ygNode, value.Value);
			}
		}
	}

	public YogaValue MinWidth
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMinWidth(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetMinWidthPercent(_ygNode, value.Value);
			}
			else
			{
				Native.YGNodeStyleSetMinWidth(_ygNode, value.Value);
			}
		}
	}

	public YogaValue MinHeight
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMinHeight(_ygNode));
		}
		set
		{
			if (value.Unit == YogaUnit.Percent)
			{
				Native.YGNodeStyleSetMinHeightPercent(_ygNode, value.Value);
			}
			else
			{
				Native.YGNodeStyleSetMinHeight(_ygNode, value.Value);
			}
		}
	}

	public float AspectRatio
	{
		get
		{
			return Native.YGNodeStyleGetAspectRatio(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetAspectRatio(_ygNode, value);
		}
	}

	public float LayoutX => Native.YGNodeLayoutGetLeft(_ygNode);

	public float LayoutY => Native.YGNodeLayoutGetTop(_ygNode);

	public float LayoutRight => Native.YGNodeLayoutGetRight(_ygNode);

	public float LayoutBottom => Native.YGNodeLayoutGetBottom(_ygNode);

	public float LayoutWidth => Native.YGNodeLayoutGetWidth(_ygNode);

	public float LayoutHeight => Native.YGNodeLayoutGetHeight(_ygNode);

	public YogaDirection LayoutDirection => Native.YGNodeLayoutGetDirection(_ygNode);

	public YogaOverflow Overflow
	{
		get
		{
			return Native.YGNodeStyleGetOverflow(_ygNode);
		}
		set
		{
			Native.YGNodeStyleSetOverflow(_ygNode, value);
		}
	}

	public object Data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public YogaNode this[int index] => _children[index];

	public int Count => (_children != null) ? _children.Count : 0;

	public YogaValue Left
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.Left));
		}
		set
		{
			SetStylePosition(YogaEdge.Left, value);
		}
	}

	public YogaValue Top
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.Top));
		}
		set
		{
			SetStylePosition(YogaEdge.Top, value);
		}
	}

	public YogaValue Right
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.Right));
		}
		set
		{
			SetStylePosition(YogaEdge.Right, value);
		}
	}

	public YogaValue Bottom
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.Bottom));
		}
		set
		{
			SetStylePosition(YogaEdge.Bottom, value);
		}
	}

	public YogaValue Start
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.Start));
		}
		set
		{
			SetStylePosition(YogaEdge.Start, value);
		}
	}

	public YogaValue End
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPosition(_ygNode, YogaEdge.End));
		}
		set
		{
			SetStylePosition(YogaEdge.End, value);
		}
	}

	public YogaValue MarginLeft
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Left));
		}
		set
		{
			SetStyleMargin(YogaEdge.Left, value);
		}
	}

	public YogaValue MarginTop
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Top));
		}
		set
		{
			SetStyleMargin(YogaEdge.Top, value);
		}
	}

	public YogaValue MarginRight
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Right));
		}
		set
		{
			SetStyleMargin(YogaEdge.Right, value);
		}
	}

	public YogaValue MarginBottom
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Bottom));
		}
		set
		{
			SetStyleMargin(YogaEdge.Bottom, value);
		}
	}

	public YogaValue MarginStart
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Start));
		}
		set
		{
			SetStyleMargin(YogaEdge.Start, value);
		}
	}

	public YogaValue MarginEnd
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.End));
		}
		set
		{
			SetStyleMargin(YogaEdge.End, value);
		}
	}

	public YogaValue MarginHorizontal
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Horizontal));
		}
		set
		{
			SetStyleMargin(YogaEdge.Horizontal, value);
		}
	}

	public YogaValue MarginVertical
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.Vertical));
		}
		set
		{
			SetStyleMargin(YogaEdge.Vertical, value);
		}
	}

	public YogaValue Margin
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetMargin(_ygNode, YogaEdge.All));
		}
		set
		{
			SetStyleMargin(YogaEdge.All, value);
		}
	}

	public YogaValue PaddingLeft
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Left));
		}
		set
		{
			SetStylePadding(YogaEdge.Left, value);
		}
	}

	public YogaValue PaddingTop
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Top));
		}
		set
		{
			SetStylePadding(YogaEdge.Top, value);
		}
	}

	public YogaValue PaddingRight
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Right));
		}
		set
		{
			SetStylePadding(YogaEdge.Right, value);
		}
	}

	public YogaValue PaddingBottom
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Bottom));
		}
		set
		{
			SetStylePadding(YogaEdge.Bottom, value);
		}
	}

	public YogaValue PaddingStart
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Start));
		}
		set
		{
			SetStylePadding(YogaEdge.Start, value);
		}
	}

	public YogaValue PaddingEnd
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.End));
		}
		set
		{
			SetStylePadding(YogaEdge.End, value);
		}
	}

	public YogaValue PaddingHorizontal
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Horizontal));
		}
		set
		{
			SetStylePadding(YogaEdge.Horizontal, value);
		}
	}

	public YogaValue PaddingVertical
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.Vertical));
		}
		set
		{
			SetStylePadding(YogaEdge.Vertical, value);
		}
	}

	public YogaValue Padding
	{
		get
		{
			return YogaValue.MarshalValue(Native.YGNodeStyleGetPadding(_ygNode, YogaEdge.All));
		}
		set
		{
			SetStylePadding(YogaEdge.All, value);
		}
	}

	public float BorderLeftWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.Left);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.Left, value);
		}
	}

	public float BorderTopWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.Top);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.Top, value);
		}
	}

	public float BorderRightWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.Right);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.Right, value);
		}
	}

	public float BorderBottomWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.Bottom);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.Bottom, value);
		}
	}

	public float BorderStartWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.Start);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.Start, value);
		}
	}

	public float BorderEndWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.End);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.End, value);
		}
	}

	public float BorderWidth
	{
		get
		{
			return Native.YGNodeStyleGetBorder(_ygNode, YogaEdge.All);
		}
		set
		{
			Native.YGNodeStyleSetBorder(_ygNode, YogaEdge.All, value);
		}
	}

	public float LayoutMarginLeft => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.Left);

	public float LayoutMarginTop => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.Top);

	public float LayoutMarginRight => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.Right);

	public float LayoutMarginBottom => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.Bottom);

	public float LayoutMarginStart => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.Start);

	public float LayoutMarginEnd => Native.YGNodeLayoutGetMargin(_ygNode, YogaEdge.End);

	public float LayoutPaddingLeft => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.Left);

	public float LayoutPaddingTop => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.Top);

	public float LayoutPaddingRight => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.Right);

	public float LayoutPaddingBottom => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.Bottom);

	public float LayoutPaddingStart => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.Start);

	public float LayoutPaddingEnd => Native.YGNodeLayoutGetPadding(_ygNode, YogaEdge.End);

	public float ComputedFlexBasis => Native.YGNodeGetComputedFlexBasis(_ygNode);

	public YogaNode(YogaConfig config = null)
	{
		_config = ((config == null) ? YogaConfig.Default : config);
		_ygNode = Native.YGNodeNewWithConfig(_config.Handle);
		if (_ygNode == IntPtr.Zero)
		{
			throw new InvalidOperationException("Failed to allocate native memory");
		}
	}

	public YogaNode(YogaNode srcNode)
		: this(srcNode._config)
	{
		CopyStyle(srcNode);
	}

	~YogaNode()
	{
		Native.YGNodeFree(_ygNode);
	}

	public void Reset()
	{
		_measureFunction = null;
		_baselineFunction = null;
		_data = null;
		Native.YGSetManagedObject(_ygNode, null);
		Native.YGNodeReset(_ygNode);
	}

	public virtual void MarkDirty()
	{
		Native.YGNodeMarkDirty(_ygNode);
	}

	public void MarkHasNewLayout()
	{
		Native.YGNodeSetHasNewLayout(_ygNode, hasNewLayout: true);
	}

	public void CopyStyle(YogaNode srcNode)
	{
		Native.YGNodeCopyStyle(_ygNode, srcNode._ygNode);
	}

	public void MarkLayoutSeen()
	{
		Native.YGNodeSetHasNewLayout(_ygNode, hasNewLayout: false);
	}

	public bool ValuesEqual(float f1, float f2)
	{
		if (float.IsNaN(f1) || float.IsNaN(f2))
		{
			return float.IsNaN(f1) && float.IsNaN(f2);
		}
		return Math.Abs(f2 - f1) < float.Epsilon;
	}

	public void Insert(int index, YogaNode node)
	{
		if (_children == null)
		{
			_children = new List<YogaNode>(4);
		}
		_children.Insert(index, node);
		node._parent = new WeakReference(this);
		Native.YGNodeInsertChild(_ygNode, node._ygNode, (uint)index);
	}

	public void RemoveAt(int index)
	{
		YogaNode yogaNode = _children[index];
		yogaNode._parent = null;
		_children.RemoveAt(index);
		Native.YGNodeRemoveChild(_ygNode, yogaNode._ygNode);
	}

	public void AddChild(YogaNode child)
	{
		Insert(Count, child);
	}

	public void RemoveChild(YogaNode child)
	{
		int num = IndexOf(child);
		if (num >= 0)
		{
			RemoveAt(num);
		}
	}

	public void Clear()
	{
		if (_children != null)
		{
			while (_children.Count > 0)
			{
				RemoveAt(_children.Count - 1);
			}
		}
	}

	public int IndexOf(YogaNode node)
	{
		return (_children != null) ? _children.IndexOf(node) : (-1);
	}

	public void SetMeasureFunction(MeasureFunction measureFunction)
	{
		_measureFunction = measureFunction;
		if (measureFunction == null)
		{
			if (!IsBaselineDefined)
			{
				Native.YGSetManagedObject(_ygNode, null);
			}
			Native.YGNodeRemoveMeasureFunc(_ygNode);
		}
		else
		{
			Native.YGSetManagedObject(_ygNode, this);
			Native.YGNodeSetMeasureFunc(_ygNode);
		}
	}

	public void SetBaselineFunction(BaselineFunction baselineFunction)
	{
		_baselineFunction = baselineFunction;
		if (baselineFunction == null)
		{
			if (!IsMeasureDefined)
			{
				Native.YGSetManagedObject(_ygNode, null);
			}
			Native.YGNodeRemoveBaselineFunc(_ygNode);
		}
		else
		{
			Native.YGSetManagedObject(_ygNode, this);
			Native.YGNodeSetBaselineFunc(_ygNode);
		}
	}

	public void CalculateLayout(float width = float.NaN, float height = float.NaN)
	{
		Native.YGNodeCalculateLayout(_ygNode, width, height, Native.YGNodeStyleGetDirection(_ygNode));
	}

	public static YogaSize MeasureInternal(YogaNode node, float width, YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
	{
		if (node == null || node._measureFunction == null)
		{
			throw new InvalidOperationException("Measure function is not defined.");
		}
		return node._measureFunction(node, width, widthMode, height, heightMode);
	}

	public static float BaselineInternal(YogaNode node, float width, float height)
	{
		if (node == null || node._baselineFunction == null)
		{
			throw new InvalidOperationException("Baseline function is not defined.");
		}
		return node._baselineFunction(node, width, height);
	}

	public string Print(YogaPrintOptions options = YogaPrintOptions.Layout | YogaPrintOptions.Style | YogaPrintOptions.Children)
	{
		StringBuilder sb = new StringBuilder();
		Logger logger = _config.Logger;
		_config.Logger = delegate(YogaConfig config, YogaNode node, YogaLogLevel level, string message)
		{
			sb.Append(message);
		};
		Native.YGNodePrint(_ygNode, options);
		_config.Logger = logger;
		return sb.ToString();
	}

	public IEnumerator<YogaNode> GetEnumerator()
	{
		return (_children != null) ? ((IEnumerable<YogaNode>)_children).GetEnumerator() : Enumerable.Empty<YogaNode>().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return (_children != null) ? ((IEnumerable<YogaNode>)_children).GetEnumerator() : Enumerable.Empty<YogaNode>().GetEnumerator();
	}

	public static int GetInstanceCount()
	{
		return Native.YGNodeGetInstanceCount();
	}

	private void SetStylePosition(YogaEdge edge, YogaValue value)
	{
		if (value.Unit == YogaUnit.Percent)
		{
			Native.YGNodeStyleSetPositionPercent(_ygNode, edge, value.Value);
		}
		else
		{
			Native.YGNodeStyleSetPosition(_ygNode, edge, value.Value);
		}
	}

	private void SetStyleMargin(YogaEdge edge, YogaValue value)
	{
		if (value.Unit == YogaUnit.Percent)
		{
			Native.YGNodeStyleSetMarginPercent(_ygNode, edge, value.Value);
		}
		else if (value.Unit == YogaUnit.Auto)
		{
			Native.YGNodeStyleSetMarginAuto(_ygNode, edge);
		}
		else
		{
			Native.YGNodeStyleSetMargin(_ygNode, edge, value.Value);
		}
	}

	private void SetStylePadding(YogaEdge edge, YogaValue value)
	{
		if (value.Unit == YogaUnit.Percent)
		{
			Native.YGNodeStyleSetPaddingPercent(_ygNode, edge, value.Value);
		}
		else
		{
			Native.YGNodeStyleSetPadding(_ygNode, edge, value.Value);
		}
	}
}
