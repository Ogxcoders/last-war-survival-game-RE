using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
	private bool mNeedCaculate;

	private bool mIsScollV;

	public float m_Sensitive;

	private ScrollRect rect;

	private List<float> posList = new List<float>();

	private float m_HorizontalLength;

	private float mPageWidth;

	private ScrollRect m_CurTransform;

	private float targethorizontal;

	private bool isDrag;

	private bool stopMove = true;

	public float smooting = 3f;

	private int m_Num;

	private Vector3 mOldPosition;

	private float startTime;

	private float startDragHorizontal;

	private int m_CurPage;

	[SerializeField]
	private InputControl inputControl;

	private Dictionary<int, ScrollRect> catchScrollRect = new Dictionary<int, ScrollRect>();

	public int AllNum => m_Num;

	public int CurPage => m_CurPage;

	private void Awake()
	{
		rect = base.transform.GetComponent<ScrollRect>();
	}

	public void Refresh()
	{
		rect = base.transform.GetComponent<ScrollRect>();
		m_Num = rect.content.transform.childCount;
		m_Num = ((m_Num <= 0) ? 1 : m_Num);
		mPageWidth = GetComponent<RectTransform>().rect.width;
		m_HorizontalLength = rect.content.rect.width - mPageWidth;
		m_CurTransform = GetTransformByIndex(GetCurrentIndex());
		m_CurPage = GetCurrentIndex();
		posList.Clear();
		catchScrollRect.Clear();
		int childCount = rect.content.transform.childCount;
		float num = 1f / ((float)childCount - 1f);
		for (int i = 0; i < childCount; i++)
		{
			posList.Add((float)i * num);
		}
		targethorizontal = posList[0];
	}

	private void OnPointerDown(Vector2 mousePosition)
	{
		mNeedCaculate = true;
		isDrag = true;
		startDragHorizontal = rect.horizontalNormalizedPosition;
		mOldPosition = Input.mousePosition;
	}

	private void OnDrag(Vector2 mousePosition)
	{
		Vector2 pDragVector = Input.mousePosition - mOldPosition;
		if (Mathf.Abs(pDragVector.x) < 6f && Mathf.Abs(pDragVector.y) < 6f)
		{
			return;
		}
		if (mNeedCaculate)
		{
			mNeedCaculate = false;
			if (Mathf.Abs(pDragVector.x) > Mathf.Abs(pDragVector.y))
			{
				mIsScollV = false;
			}
			else
			{
				mIsScollV = true;
			}
		}
		DragScreen(pDragVector);
		mOldPosition = Input.mousePosition;
	}

	private void OnPointerUp(Vector2 mousePosition)
	{
		if (!mIsScollV)
		{
			int index = (m_CurPage = GetCurrentIndex());
			targethorizontal = posList[index];
			isDrag = false;
			startTime = 0f;
			stopMove = false;
		}
	}

	private int GetCurrentIndex()
	{
		if (posList.Count < 2)
		{
			return 0;
		}
		float horizontalNormalizedPosition = rect.horizontalNormalizedPosition;
		for (int i = 0; i < posList.Count; i++)
		{
			if (posList[i] == horizontalNormalizedPosition)
			{
				return i;
			}
			if (i + 1 == posList.Count)
			{
				return i;
			}
			if (posList[i] < horizontalNormalizedPosition && posList[i + 1] > horizontalNormalizedPosition)
			{
				if (horizontalNormalizedPosition >= posList[i] + (posList[i + 1] - posList[i]) / 2f)
				{
					return i + 1;
				}
				return i;
			}
		}
		return 0;
	}

	private ScrollRect GetTransformByIndex(int index)
	{
		if (index > rect.content.transform.childCount - 1)
		{
			return null;
		}
		if (!catchScrollRect.ContainsKey(index))
		{
			catchScrollRect.Add(index, rect.content.transform.GetChild(index).GetComponent<ScrollRect>());
		}
		return catchScrollRect[index];
	}

	private void DragScreen(Vector2 pDragVector)
	{
		if (!mIsScollV)
		{
			rect.horizontalNormalizedPosition -= pDragVector.x / (float)Screen.width * m_Sensitive / (float)posList.Count;
			if (rect.horizontalNormalizedPosition < 0f)
			{
				rect.horizontalNormalizedPosition = 0f;
			}
			else if (rect.horizontalNormalizedPosition > 1f)
			{
				rect.horizontalNormalizedPosition = 1f;
			}
		}
	}

	private void Start()
	{
		inputControl.EVENT_MOUSE_DOWN += OnPointerDown;
		inputControl.EVENT_MOUSE_UP += OnPointerUp;
		inputControl.EVENT_MOUSE_DRAG += OnDrag;
	}

	private void OnDestory()
	{
		inputControl.EVENT_MOUSE_DOWN -= OnPointerDown;
		inputControl.EVENT_MOUSE_UP -= OnPointerUp;
		inputControl.EVENT_MOUSE_DRAG -= OnDrag;
	}

	private void Update()
	{
		if (!isDrag && !stopMove && !mIsScollV)
		{
			startTime += Time.deltaTime;
			float num = startTime * smooting;
			rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, num);
			if (num >= 1f)
			{
				m_CurTransform = GetTransformByIndex(GetCurrentIndex());
				stopMove = true;
			}
		}
	}
}
