using System.Reflection;
using SuperScrollView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

[RequireComponent(typeof(Image))]
[LuaCallCSharp(GenFlag.No)]
public class UITestInputEvent : MonoBehaviour, ICanvasRaycastFilter
{
	private static StandaloneInputModule s_inputModule;

	private static FieldInfo s_fieldInfo;

	public LoopListView2 loopListView2;

	private RectTransform _rectTransform;

	private Transform _parent;

	private int _raycastFrame;

	private bool isMouseScroll;

	private void Awake()
	{
		if (s_fieldInfo == null)
		{
			s_inputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
			if (s_inputModule != null)
			{
				s_fieldInfo = typeof(StandaloneInputModule).GetField("m_InputPointerEvent", BindingFlags.Instance | BindingFlags.NonPublic);
			}
		}
		_rectTransform = GetComponent<RectTransform>();
		SetEnable(enable: false);
		_parent = base.transform.parent;
		GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
	}

	public void SetEnable(bool enable)
	{
		base.gameObject.SetActive(enable);
		if (enable)
		{
			_raycastFrame = 0;
			isMouseScroll = false;
		}
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		_raycastFrame = Time.frameCount;
		return false;
	}

	public void Update()
	{
	}

	public void LateUpdate()
	{
		if (isMouseScroll)
		{
			SetEnable(enable: false);
			loopListView2.EnableLoadingGoTail(enable: false);
		}
		else
		{
			if (_raycastFrame != Time.frameCount)
			{
				return;
			}
			if (s_fieldInfo == null)
			{
				SetEnable(enable: false);
				loopListView2.EnableLoadingGoTail(enable: false);
			}
			else if (s_fieldInfo.GetValue(s_inputModule) is PointerEventData pointerEventData && pointerEventData.rawPointerPress != null)
			{
				RectTransform component = pointerEventData.rawPointerPress.GetComponent<RectTransform>();
				if (IsRectTransformOverlap(component, _rectTransform) && IsSameParent(pointerEventData.rawPointerPress))
				{
					SetEnable(enable: false);
					loopListView2.EnableLoadingGoTail(enable: false);
				}
			}
		}
	}

	private bool IsRectTransformOverlap(RectTransform rectA, RectTransform rectB)
	{
		Rect rect = rectA.rect;
		Vector2 vector = rectA.TransformPoint(rect.min);
		Vector2 vector2 = rectA.TransformPoint(rect.max);
		Rect rect2 = rectB.rect;
		Vector2 vector3 = rectB.TransformPoint(rect2.min);
		Vector2 vector4 = rectB.TransformPoint(rect2.max);
		if (vector2.x > vector3.x && vector.x < vector4.x && vector2.y > vector3.y)
		{
			return vector.y < vector4.y;
		}
		return false;
	}

	private bool IsSameParent(GameObject obj)
	{
		if (_parent == null)
		{
			return true;
		}
		bool result = false;
		Transform parent = obj.transform;
		while (parent != null)
		{
			if (parent == _parent)
			{
				result = true;
				break;
			}
			parent = parent.parent;
		}
		return result;
	}
}
