using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CenterOnChild : MonoBehaviour, IEndDragHandler, IEventSystemHandler, IDragHandler
{
	public delegate void OnCenterHandler(GameObject centerChild);

	public float centerSpeed = 9f;

	private ScrollRect _scrollView;

	private Transform _container;

	private List<float> _childrenPos = new List<float>();

	private float _targetPos;

	private bool _centering;

	public event OnCenterHandler onCenter;

	private void Awake()
	{
		_scrollView = GetComponent<ScrollRect>();
		if (_scrollView == null)
		{
			Debug.LogError("CenterOnChild: No ScrollRect");
			return;
		}
		_container = _scrollView.content;
		GridLayoutGroup component = _container.GetComponent<GridLayoutGroup>();
		if (component == null)
		{
			Debug.LogError("CenterOnChild: No GridLayoutGroup on the ScrollRect's content");
			return;
		}
		_scrollView.movementType = ScrollRect.MovementType.Unrestricted;
		float num = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - component.cellSize.x * 0.5f;
		_childrenPos.Add(num);
		for (int i = 0; i < _container.childCount - 1; i++)
		{
			num -= component.cellSize.x + component.spacing.x;
			_childrenPos.Add(num);
		}
	}

	public void Reset()
	{
		_scrollView = GetComponent<ScrollRect>();
		if (_scrollView == null)
		{
			Debug.LogError("CenterOnChild: No ScrollRect");
			return;
		}
		_container = _scrollView.content;
		GridLayoutGroup component = _container.GetComponent<GridLayoutGroup>();
		if (component == null)
		{
			Debug.LogError("CenterOnChild: No GridLayoutGroup on the ScrollRect's content");
			return;
		}
		_scrollView.movementType = ScrollRect.MovementType.Unrestricted;
		_childrenPos.Clear();
		float num = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - component.cellSize.x * 0.5f;
		_childrenPos.Add(num);
		for (int i = 0; i < _container.childCount - 1; i++)
		{
			num -= component.cellSize.x + component.spacing.x;
			_childrenPos.Add(num);
		}
	}

	private void Update()
	{
		if (_centering)
		{
			Vector3 localPosition = _container.localPosition;
			localPosition.x = Mathf.Lerp(_container.localPosition.x, _targetPos, centerSpeed * Time.deltaTime);
			_container.localPosition = localPosition;
			if (Mathf.Abs(_container.localPosition.x - _targetPos) < 0.01f)
			{
				_centering = false;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_centering = true;
		_targetPos = FindClosestPos(_container.localPosition.x);
	}

	public void OnDrag(PointerEventData eventData)
	{
		_centering = false;
	}

	private float FindClosestPos(float currentPos)
	{
		int index = 0;
		float result = 0f;
		float num = float.PositiveInfinity;
		for (int i = 0; i < _childrenPos.Count; i++)
		{
			float num2 = _childrenPos[i];
			float num3 = Mathf.Abs(num2 - currentPos);
			if (num3 < num)
			{
				num = num3;
				result = num2;
				index = i;
			}
		}
		GameObject centerChild = _container.GetChild(index).gameObject;
		if (this.onCenter != null)
		{
			this.onCenter(centerChild);
		}
		return result;
	}
}
