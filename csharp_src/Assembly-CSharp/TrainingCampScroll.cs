using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainingCampScroll : MonoBehaviour, IEndDragHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler
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

	private bool _onDray;

	private bool _playMove;

	public float MoveToCenterSpeed = 10f;

	public OnCenterComplete OnCenterCallBack;

	private ScrollRect _scrollView;

	private Transform _content;

	private List<float> _childrenPos = new List<float>();

	private List<Image> _childImages = new List<Image>();

	private float _startTime;

	private int _curCenterChildIndex;

	private Vector3 _startPos;

	private Bounds m_ContentBounds;

	private Vector2 center;

	private bool m_isMoveStop = true;

	private Vector2 _targetPos;

	public GameObject CurCenterChildItem
	{
		get
		{
			GameObject result = null;
			if (_content != null && _curCenterChildIndex >= 0)
			{
				result = ((_curCenterChildIndex >= _content.childCount) ? _content.GetChild(_content.childCount - 1).gameObject : _content.GetChild((_curCenterChildIndex != 0) ? (_curCenterChildIndex - 1) : 0).gameObject);
			}
			return result;
		}
	}

	public int CurCenterChildIndex => _curCenterChildIndex;

	public void Start()
	{
	}

	public void Init()
	{
		_scrollView = GetComponent<ScrollRect>();
		if (_scrollView == null)
		{
			Log.Error("ScrollRect is null");
			return;
		}
		_content = _scrollView.content;
		_childrenPos.Clear();
		_childImages.Clear();
		LayoutGroup layoutGroup = null;
		layoutGroup = _content.GetComponent<LayoutGroup>();
		if (layoutGroup == null)
		{
			Log.Error("LayoutGroup component is null");
		}
		_scrollView.movementType = ScrollRect.MovementType.Unrestricted;
		float num = 0f;
		switch (Dir)
		{
		case ScrollDir.Horizontal:
			if (layoutGroup is HorizontalLayoutGroup || layoutGroup is BidirectionalHorizontalLayoutGroup)
			{
				float childItemWidth = GetChildItemWidth(0);
				for (int k = 0; k < _content.childCount; k++)
				{
					_childrenPos.Add((0f - childItemWidth) * ((float)k + 0.5f));
				}
				for (int l = 0; l < _childrenPos.Count; l++)
				{
				}
			}
			else if (layoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup gridLayoutGroup2 = layoutGroup as GridLayoutGroup;
				float num4 = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - gridLayoutGroup2.cellSize.x * 0.5f;
				_childrenPos.Add(num4);
				for (int m = 0; m < _content.childCount - 1; m++)
				{
					num4 -= gridLayoutGroup2.cellSize.x + gridLayoutGroup2.spacing.x;
					_childrenPos.Add(num4);
				}
			}
			else
			{
				Log.Error("Horizontal ScrollView is using VerticalLayoutGroup");
			}
			break;
		case ScrollDir.Vertical:
			if (layoutGroup is VerticalLayoutGroup)
			{
				float num2 = (0f - _scrollView.GetComponent<RectTransform>().rect.height) * 0.5f + GetChildItemHeight(0) * 0.5f;
				num = (layoutGroup as VerticalLayoutGroup).spacing;
				_childrenPos.Add(num2);
				for (int i = 1; i < _content.childCount; i++)
				{
					num2 += GetChildItemHeight(i) * 0.5f + GetChildItemHeight(i - 1) * 0.5f + num;
					_childrenPos.Add(num2);
				}
			}
			else if (layoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup gridLayoutGroup = layoutGroup as GridLayoutGroup;
				float num3 = (0f - _scrollView.GetComponent<RectTransform>().rect.height) * 0.5f + gridLayoutGroup.cellSize.y * 0.5f;
				_childrenPos.Add(num3);
				for (int j = 1; j < _content.childCount; j++)
				{
					num3 += gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
					_childrenPos.Add(num3);
				}
			}
			break;
		}
	}

	private float GetChildItemWidth(int index)
	{
		return (_content.GetChild(index) as RectTransform).sizeDelta.x;
	}

	private float GetChildItemHeight(int index)
	{
		return (_content.GetChild(index) as RectTransform).sizeDelta.y;
	}

	private void OnScrollMoveStop()
	{
		if (!m_isMoveStop)
		{
			m_isMoveStop = true;
		}
		SetPageIndex(FindClosestChildIndex());
		_startTime = 0f;
		_startPos = _scrollView.content.anchoredPosition;
		_targetPos = FindClosestChildPos(_curCenterChildIndex);
		_playMove = true;
	}

	private void Update()
	{
		ClampBounds();
		if (!_scrollView || m_isMoveStop)
		{
			return;
		}
		if (_scrollView.velocity == Vector2.zero)
		{
			OnScrollMoveStop();
			return;
		}
		int num = FindClosestChildIndex();
		if (num != _curCenterChildIndex)
		{
			SetPageIndex(num);
			UpdateSelected();
		}
	}

	private void LateUpdate()
	{
		if (m_isMoveStop && _playMove && !_onDray)
		{
			_startTime += Time.deltaTime * 5f;
			if (Vector2.Distance(_scrollView.content.anchoredPosition, _targetPos) < 0.02f)
			{
				SetAnchoredPosition(_targetPos);
				_playMove = false;
				OnMoveDone();
			}
			else
			{
				Vector2 anchoredPosition = Vector2.Lerp(_startPos, _targetPos, _startTime);
				SetAnchoredPosition(anchoredPosition);
			}
		}
	}

	private void UpdateSelected()
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_onDray = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		int num = FindClosestChildIndex();
		if (num != _curCenterChildIndex)
		{
			SetPageIndex(num);
			UpdateSelected();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_isMoveStop = false;
		_playMove = false;
		_onDray = false;
		_ = _curCenterChildIndex;
		if (Dir != ScrollDir.Horizontal)
		{
			_ = 1;
		}
	}

	public void CenterOn(int index)
	{
		if (m_isMoveStop)
		{
			_playMove = false;
		}
		else
		{
			_scrollView.StopMovement();
		}
		GoToItem(index);
	}

	private void OnMoveDone()
	{
	}

	private int FindClosestChildIndex()
	{
		float x = _scrollView.content.anchoredPosition.x;
		int num = 0;
		float num2 = Mathf.Abs(_childrenPos[num] - x);
		for (int i = 1; i < _childrenPos.Count; i++)
		{
			float num3 = Mathf.Abs(_childrenPos[i] - x);
			if (num3 < num2)
			{
				num = i;
				num2 = num3;
			}
		}
		return num;
	}

	private Vector2 FindClosestChildPos(int curCenterChildIndex)
	{
		return new Vector2(_childrenPos[curCenterChildIndex], 0f);
	}

	public void ClampBounds()
	{
		if (_scrollView != null)
		{
			Vector2 anchoredPosition = _scrollView.content.anchoredPosition;
			if (anchoredPosition.x > _childrenPos[0])
			{
				_scrollView.StopMovement();
				_scrollView.content.anchoredPosition = new Vector2(_childrenPos[0], 0f);
			}
			if (anchoredPosition.x < _childrenPos[_childrenPos.Count - 1])
			{
				_scrollView.StopMovement();
				_scrollView.content.anchoredPosition = new Vector2(_childrenPos[_childrenPos.Count - 1], 0f);
			}
		}
	}

	public void AotuGotoItem(int index)
	{
		if (!_playMove)
		{
			_startTime = 0f;
			_startPos = _scrollView.content.anchoredPosition;
			SetPageIndex(index);
			_targetPos = FindClosestChildPos(index);
			_playMove = true;
		}
	}

	public void GoToItem(int index)
	{
		float x = _childrenPos[index];
		Vector2 anchoredPosition = new Vector2(x, 0f);
		SetAnchoredPosition(anchoredPosition);
		SetPageIndex(index);
	}

	private void SetPageIndex(int index)
	{
		_curCenterChildIndex = index;
	}

	private void SetAnchoredPosition(Vector2 pos)
	{
		_scrollView.content.anchoredPosition = pos;
	}
}
