using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Utils.UnityEx;

public class ScreenToucher : MonoBehaviour
{
	private static ScreenToucher _instance;

	private const float CLICK_JUDGE = 200f;

	public readonly Dictionary<int, Vector2> _pointerDownPosi = new Dictionary<int, Vector2>();

	public readonly Dictionary<int, Vector2> _pointerDownPosiIgnoreUI = new Dictionary<int, Vector2>();

	public readonly Dictionary<int, Vector2> _tempMovePosi = new Dictionary<int, Vector2>();

	public EventSystem eventSystem;

	private float lastClickTime;

	private Rect interestedScreenRect;

	private bool hasInterestedScreenRect;

	private StringBuilder debugSb = new StringBuilder();

	public static ScreenToucher Instance => _instance;

	private void Awake()
	{
		_instance = this;
	}

	public void Clear()
	{
		_pointerDownPosi.Clear();
		_pointerDownPosiIgnoreUI.Clear();
		ClearInterestedRect();
	}

	private bool AddPointerDownRecord(int pointerId, Vector2 pointerPosi)
	{
		bool num = _pointerDownPosi.ContainsKey(pointerId);
		if (!num)
		{
			_pointerDownPosi.Add(pointerId, pointerPosi);
		}
		return !num;
	}

	private bool AddPointerDownIgnoreUIRecord(int pointerId, Vector2 pointerPosi)
	{
		bool num = _pointerDownPosiIgnoreUI.ContainsKey(pointerId);
		if (!num)
		{
			_pointerDownPosiIgnoreUI.Add(pointerId, pointerPosi);
		}
		return !num;
	}

	private void AddTempMoveRecord(int pointerId, Vector2 pointerPosi)
	{
		if (_tempMovePosi.ContainsKey(pointerId))
		{
			_tempMovePosi[pointerId] = pointerPosi;
		}
		else
		{
			_tempMovePosi.Add(pointerId, pointerPosi);
		}
	}

	private void RemoveTempMoveRecord(int pointerId)
	{
		if (_tempMovePosi.ContainsKey(pointerId))
		{
			_tempMovePosi.Remove(pointerId);
		}
	}

	private void RemovePointerDownRecord(int pointerId)
	{
		if (_pointerDownPosi.ContainsKey(pointerId))
		{
			_pointerDownPosi.Remove(pointerId);
		}
		if (_pointerDownPosiIgnoreUI.ContainsKey(pointerId))
		{
			_pointerDownPosiIgnoreUI.Remove(pointerId);
		}
	}

	private void CallTouchDownEvent(int pointerId, Vector2 pointerPosi)
	{
		TouchInfo touchInfo = new TouchInfo(pointerId, pointerPosi, Vector2.zero);
		GameEntry.Event.Fire(EventId.SCREEN_TOUCH_DOWN, touchInfo);
	}

	private void CallTouchDownIgnoreUIEvent(int pointerId, Vector2 pointerPosi)
	{
		TouchInfo touchInfo = new TouchInfo(pointerId, pointerPosi, Vector2.zero);
		GameEntry.Event.Fire(EventId.SCREEN_TOUCH_DOWN_IGNORE_UI, touchInfo);
	}

	private void CallTouchUpEvent(int pointerId, Vector2 pointerPosi)
	{
		TouchInfo touchInfo = new TouchInfo(pointerId, pointerPosi, Vector2.zero);
		GameEntry.Event.Fire(EventId.SCREEN_TOUCH_UP_IGNORE_UI, touchInfo);
		if (!IsPointerOverUIObject(pointerId, pointerPosi))
		{
			GameEntry.Event.Fire(EventId.SCREEN_TOUCH_UP, touchInfo);
		}
	}

	private void CallTouchClickEvent(int pointerId, Vector2 pointerPosi)
	{
		if (hasInterestedScreenRect && interestedScreenRect.Contains(pointerPosi))
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup - lastClickTime <= 2f)
			{
				lastClickTime = 0f;
				GameObject firstUIObject = GetFirstUIObject(pointerPosi);
				firstUIObject = firstUIObject ?? eventSystem?.currentSelectedGameObject;
				PostGameObjectClickedEvent(firstUIObject);
			}
			else
			{
				lastClickTime = realtimeSinceStartup;
			}
		}
		if (_pointerDownPosiIgnoreUI.ContainsKey(pointerId) && !((pointerPosi - _pointerDownPosiIgnoreUI[pointerId]).sqrMagnitude > 200f))
		{
			TouchInfo touchInfo = new TouchInfo(pointerId, pointerPosi, Vector2.zero);
			GameEntry.Event.Fire(EventId.SCREEN_TOUCH_CLICK_IGNORE_UI, touchInfo);
			if (_pointerDownPosi.ContainsKey(pointerId) && !IsPointerOverUIObject(pointerId, pointerPosi) && _pointerDownPosiIgnoreUI.ContainsKey(pointerId))
			{
				GameEntry.Event.Fire(EventId.SCREEN_TOUCH_CLICK, touchInfo);
			}
		}
	}

	private void CallTouchMoveEvent(int pointerId, Vector2 pointerPosi, Vector2 deltaPosi)
	{
		TouchInfo touchInfo = new TouchInfo(pointerId, pointerPosi, deltaPosi);
		GameEntry.Event.Fire(EventId.SCREEN_TOUCH_MOVE, touchInfo);
	}

	private void Update()
	{
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			switch (touch.phase)
			{
			case TouchPhase.Stationary:
				if (AddPointerDownIgnoreUIRecord(touch.fingerId, touch.position))
				{
					AddTempMoveRecord(touch.fingerId, touch.position);
					CallTouchDownIgnoreUIEvent(touch.fingerId, touch.position);
					if (!IsPointerOverUIObject(touch.fingerId, touch.position) && AddPointerDownRecord(touch.fingerId, touch.position))
					{
						CallTouchDownEvent(touch.fingerId, touch.position);
					}
				}
				break;
			case TouchPhase.Ended:
				CallTouchUpEvent(touch.fingerId, touch.position);
				CallTouchClickEvent(touch.fingerId, touch.position);
				RemovePointerDownRecord(touch.fingerId);
				RemoveTempMoveRecord(touch.fingerId);
				break;
			case TouchPhase.Moved:
				if (AddPointerDownIgnoreUIRecord(touch.fingerId, touch.position))
				{
					AddTempMoveRecord(touch.fingerId, touch.position);
					CallTouchDownIgnoreUIEvent(touch.fingerId, touch.position);
					if (!IsPointerOverUIObject(touch.fingerId, touch.position) && AddPointerDownRecord(touch.fingerId, touch.position))
					{
						CallTouchDownEvent(touch.fingerId, touch.position);
					}
				}
				if (_tempMovePosi.ContainsKey(touch.fingerId))
				{
					CallTouchMoveEvent(touch.fingerId, touch.position, touch.position - _tempMovePosi[touch.fingerId]);
					AddTempMoveRecord(touch.fingerId, touch.position);
				}
				break;
			}
		}
	}

	public bool IsPointerOverUIObject(int fingerID, Vector2 screenPosition)
	{
		return ((eventSystem == null) ? EventSystem.current : eventSystem).IsPointerOverGameObject(fingerID);
	}

	public bool IsPointerOverUIObject(Vector2 screenPosition)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = new Vector2(screenPosition.x, screenPosition.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		if (list.Count > 0)
		{
			Debug.LogWarning("EventSystem.IsPointerOverGameObject Is Not Working.");
		}
		return list.Count > 0;
	}

	private GameObject GetFirstUIObject(Vector2 screenPosition)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = new Vector2(screenPosition.x, screenPosition.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		if (list.Count > 0)
		{
			return list[0].gameObject;
		}
		return null;
	}

	public static void RegisterScreenRect(RectTransform rectTransform)
	{
		if (_instance == null)
		{
			return;
		}
		if (rectTransform == null)
		{
			ClearInterestedRect();
			return;
		}
		if (GameEntry.UICamera == null)
		{
			ClearInterestedRect();
			return;
		}
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < 4; i++)
		{
			Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(GameEntry.UICamera, array[i]);
			vector.x = Mathf.Min(vector.x, vector3.x);
			vector.y = Mathf.Min(vector.y, vector3.y);
			vector2.x = Mathf.Max(vector2.x, vector3.x);
			vector2.y = Mathf.Max(vector2.y, vector3.y);
		}
		_instance.interestedScreenRect = new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
		_instance.hasInterestedScreenRect = true;
	}

	public static void ClearInterestedRect()
	{
		if (!(_instance == null))
		{
			_instance.interestedScreenRect.Set(0f, 0f, 0f, 0f);
			_instance.hasInterestedScreenRect = false;
		}
	}

	private void PostGameObjectClickedEvent(GameObject gameObject)
	{
		if (!(gameObject == null))
		{
			string text = _GetName(debugSb, gameObject.transform);
			Log.Info("[ScreenToucher] " + text + "]");
		}
		string _GetName(StringBuilder sb, Transform t)
		{
			debugSb.Length = 0;
			Transform transform = t;
			while (transform != null)
			{
				sb.Insert(0, transform.name);
				transform = transform.parent;
				if (transform != null)
				{
					sb.Insert(0, "/");
				}
			}
			string result = sb.ToString();
			debugSb.Length = 0;
			return result;
		}
	}
}
