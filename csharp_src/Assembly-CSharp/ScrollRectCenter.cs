using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectCenter : MonoBehaviour, IEndDragHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler
{
	public enum ScrollType
	{
		Normal,
		ForGiftPack,
		ForHeroInfo,
		ForEditTroop,
		ForEditTrain
	}

	public delegate void OnCenterComplete(int index);

	public ScrollDir Dir;

	private bool _isCentering;

	public float MoveToCenterSpeed = 10f;

	public OnCenterComplete OnCenterCallBack;

	private ScrollRect _scrollView;

	private Transform _content;

	private List<float> _childrenPos = new List<float>();

	private List<Image> _childImages = new List<Image>();

	private float _targetPos;

	private float _tempTime;

	private bool _inited;

	private int _curCenterChildIndex;

	private Vector3 _startPrefix = Vector3.zero;

	private ScrollType _type;

	private Vector3 _startPos;

	public GameObject CurCenterChildItem
	{
		get
		{
			GameObject result = null;
			if (_type == ScrollType.ForEditTrain)
			{
				if (_content != null && _curCenterChildIndex >= 0)
				{
					result = ((_curCenterChildIndex >= _content.childCount) ? _content.GetChild(_content.childCount - 1).gameObject : _content.GetChild((_curCenterChildIndex != 0) ? (_curCenterChildIndex - 1) : 0).gameObject);
				}
			}
			else if (_content != null && _curCenterChildIndex >= 0 && _curCenterChildIndex < _content.childCount)
			{
				result = _content.GetChild(_curCenterChildIndex).gameObject;
			}
			return result;
		}
	}

	public int CurCenterChildIndex => _curCenterChildIndex;

	public void Init(ScrollType type)
	{
		_scrollView = GetComponent<ScrollRect>();
		if (_scrollView == null)
		{
			Log.Error("ScrollRect is null");
			return;
		}
		_type = type;
		_content = _scrollView.content;
		_childrenPos.Clear();
		_childImages.Clear();
		if (_type == ScrollType.ForHeroInfo)
		{
			for (int i = 0; i < _content.childCount; i++)
			{
			}
		}
		else if (_type == ScrollType.ForEditTroop || _type == ScrollType.ForEditTrain)
		{
			for (int j = 0; j < _content.childCount; j++)
			{
				_childImages.Add(_content.GetChild(j).GetComponentInChildren<Image>());
			}
		}
		LayoutGroup layoutGroup = null;
		layoutGroup = _content.GetComponent<LayoutGroup>();
		_ = layoutGroup == null;
		float num = 0f;
		switch (Dir)
		{
		case ScrollDir.Horizontal:
			if (layoutGroup is HorizontalLayoutGroup || layoutGroup is BidirectionalHorizontalLayoutGroup)
			{
				num = (layoutGroup as HorizontalOrVerticalLayoutGroup).spacing;
				float num4 = 0f;
				if (_type == ScrollType.Normal || _type == ScrollType.ForHeroInfo || _type == ScrollType.ForGiftPack)
				{
					_childrenPos.Add(num4);
					for (int m = 1; m < _content.childCount; m++)
					{
						num4 -= GetChildItemWidth(m) * 0.5f + GetChildItemWidth(m - 1) * 0.5f + num;
						_childrenPos.Add(num4);
					}
				}
				else if (_type == ScrollType.ForEditTroop)
				{
					num4 = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - GetChildItemWidth(0) * 0.5f;
					num = (layoutGroup as HorizontalOrVerticalLayoutGroup).spacing;
					_childrenPos.Add(num4);
					for (int n = 1; n < _content.childCount; n++)
					{
						num4 -= GetChildItemWidth(n) * 0.5f + GetChildItemWidth(n - 1) * 0.5f + num;
						_childrenPos.Add(num4);
					}
				}
				else
				{
					if (_type != ScrollType.ForEditTrain)
					{
						break;
					}
					num4 = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - GetChildItemWidth(0) * 0.5f;
					num = (layoutGroup as HorizontalOrVerticalLayoutGroup).spacing;
					_childrenPos.Add(num4);
					for (int num5 = 0; num5 < _content.childCount + 1; num5++)
					{
						if (num5 == _content.childCount)
						{
							num4 = _childrenPos[num5];
						}
						else if (num5 != 0)
						{
							num4 -= GetChildItemWidth(num5) * 0.5f + GetChildItemWidth(num5 - 1) * 0.5f + num;
						}
						_childrenPos.Add(num4);
					}
				}
			}
			else if (layoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup gridLayoutGroup2 = layoutGroup as GridLayoutGroup;
				float num6 = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - gridLayoutGroup2.cellSize.x * 0.5f;
				_childrenPos.Add(num6);
				for (int num7 = 0; num7 < _content.childCount - 1; num7++)
				{
					num6 -= gridLayoutGroup2.cellSize.x + gridLayoutGroup2.spacing.x;
					_childrenPos.Add(num6);
				}
			}
			break;
		case ScrollDir.Vertical:
			if (layoutGroup is VerticalLayoutGroup)
			{
				float num2 = (0f - _scrollView.GetComponent<RectTransform>().rect.height) * 0.5f + GetChildItemHeight(0) * 0.5f;
				num = (layoutGroup as VerticalLayoutGroup).spacing;
				_childrenPos.Add(num2);
				for (int k = 1; k < _content.childCount; k++)
				{
					num2 += GetChildItemHeight(k) * 0.5f + GetChildItemHeight(k - 1) * 0.5f + num;
					_childrenPos.Add(num2);
				}
			}
			else if (layoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup gridLayoutGroup = layoutGroup as GridLayoutGroup;
				float num3 = (0f - _scrollView.GetComponent<RectTransform>().rect.height) * 0.5f + gridLayoutGroup.cellSize.y * 0.5f;
				_childrenPos.Add(num3);
				for (int l = 1; l < _content.childCount; l++)
				{
					num3 += gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
					_childrenPos.Add(num3);
				}
			}
			break;
		}
		_tempTime = 0f;
		_inited = true;
	}

	private float GetChildItemWidth(int index)
	{
		return (_content.GetChild(index) as RectTransform).sizeDelta.x;
	}

	private float GetChildItemHeight(int index)
	{
		return (_content.GetChild(index) as RectTransform).sizeDelta.y;
	}

	private void Update()
	{
		if (!_isCentering || !_inited)
		{
			return;
		}
		Vector3 startPrefix = _startPrefix;
		_tempTime += MoveToCenterSpeed * Time.deltaTime;
		switch (Dir)
		{
		case ScrollDir.Horizontal:
			startPrefix.x = Mathf.Lerp(_startPrefix.x, _targetPos, _tempTime);
			_content.localPosition = startPrefix;
			break;
		case ScrollDir.Vertical:
			startPrefix.y = Mathf.Lerp(_startPrefix.y, _targetPos, _tempTime);
			_content.localPosition = startPrefix;
			break;
		}
		if (_type == ScrollType.Normal || _type == ScrollType.ForHeroInfo || _type == ScrollType.ForGiftPack)
		{
			for (int i = _curCenterChildIndex - 1; i <= _curCenterChildIndex + 1; i++)
			{
				if (i >= 0)
				{
					_ = _childImages.Count;
				}
			}
		}
		if (_tempTime >= 1f)
		{
			_isCentering = false;
			if (OnCenterCallBack != null)
			{
				OnCenterCallBack(_curCenterChildIndex);
			}
		}
	}

	public void AsyncPos()
	{
		_content.localPosition = Vector3.zero;
	}

	public void OnDrag(PointerEventData eventData)
	{
		for (int i = _curCenterChildIndex - 1; i <= _curCenterChildIndex + 1; i++)
		{
			if (i >= 0)
			{
				_ = _childImages.Count;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		int curCenterChildIndex = _curCenterChildIndex;
		switch (Dir)
		{
		case ScrollDir.Horizontal:
		{
			_targetPos = FindClosestChildPos(_content.localPosition.x, out _curCenterChildIndex);
			if (_type != ScrollType.ForEditTrain || curCenterChildIndex != _curCenterChildIndex)
			{
				break;
			}
			float num = _startPos.x - eventData.position.x;
			if (Mathf.Abs(num) > 30f)
			{
				if (num < 0f)
				{
					_curCenterChildIndex--;
				}
				else if (num > 0f)
				{
					_curCenterChildIndex++;
				}
				_targetPos = FindClosestChildPos(_curCenterChildIndex);
			}
			break;
		}
		case ScrollDir.Vertical:
			_targetPos = FindClosestChildPos(_content.localPosition.y, out _curCenterChildIndex);
			break;
		}
		_isCentering = true;
		_startPrefix = _content.localPosition;
		_ = _type;
		_ = 4;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_isCentering = false;
		_tempTime = 0f;
		_startPrefix = Vector3.zero;
		_startPos = eventData.position;
	}

	public void CenterOn(int index)
	{
		if (index >= _childrenPos.Count)
		{
			index = _childrenPos.Count - 1;
		}
		if (index < 0)
		{
			index = 0;
		}
		switch (Dir)
		{
		case ScrollDir.Horizontal:
		{
			Vector3 localPosition = _content.localPosition;
			localPosition.x = _childrenPos[index];
			_content.localPosition = localPosition;
			break;
		}
		case ScrollDir.Vertical:
		{
			Vector3 localPosition = _content.localPosition;
			localPosition.y = _childrenPos[index];
			_content.localPosition = localPosition;
			break;
		}
		}
		_curCenterChildIndex = index;
	}

	private float FindClosestChildPos(float currentPos, out int curCenterChildIndex)
	{
		if (currentPos - _childrenPos[_curCenterChildIndex] > 50f)
		{
			if (_type == ScrollType.ForGiftPack)
			{
				curCenterChildIndex = ((_curCenterChildIndex <= 0) ? (_childrenPos.Count - 1) : (_curCenterChildIndex - 1));
			}
			else
			{
				curCenterChildIndex = ((_curCenterChildIndex > 0) ? (_curCenterChildIndex - 1) : 0);
			}
			return _childrenPos[curCenterChildIndex];
		}
		if (currentPos - _childrenPos[_curCenterChildIndex] < -50f)
		{
			if (_type == ScrollType.ForGiftPack)
			{
				curCenterChildIndex = ((_curCenterChildIndex + 1 < _childrenPos.Count) ? (_curCenterChildIndex + 1) : 0);
			}
			else
			{
				curCenterChildIndex = ((_curCenterChildIndex + 1 >= _childrenPos.Count) ? (_childrenPos.Count - 1) : (_curCenterChildIndex + 1));
			}
			return _childrenPos[curCenterChildIndex];
		}
		curCenterChildIndex = _curCenterChildIndex;
		return _childrenPos[curCenterChildIndex];
	}

	private float FindClosestChildPos(int curCenterChildIndex)
	{
		return _childrenPos[curCenterChildIndex];
	}
}
