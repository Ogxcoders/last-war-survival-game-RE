using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIUtils
{
	public delegate void SkeletonDelegate(SkeletonDataAsset asset);

	public static float tmpWidth = -100f;

	public static Vector2 leftTopScreenPoint = default(Vector2);

	public static Vector2 leftTopRectPoint = default(Vector2);

	public static float GetSpecialScreenWidth()
	{
		if ((double)tmpWidth < -99.0)
		{
			int width = Screen.width;
			float width2 = ScreenSafeArea.GetSafeArea().width;
			tmpWidth = ((float)width - width2) / 2f;
		}
		return tmpWidth;
	}

	public static float GetDescentHeightFromScreenSafeArea(RectTransform panel)
	{
		if (panel == null)
		{
			return 0f;
		}
		Rect safeArea = ScreenSafeArea.GetSafeArea();
		leftTopScreenPoint.x = 0f;
		leftTopScreenPoint.y = safeArea.height;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, leftTopScreenPoint, GameEntry.UICamera, out leftTopRectPoint);
		float num = panel.rect.y + panel.rect.height;
		if (num > leftTopRectPoint.y)
		{
			return num - leftTopRectPoint.y;
		}
		return 0f;
	}

	public static bool IsPhone()
	{
		return 1f * (float)Screen.width / (float)Screen.height > 1.5f;
	}

	public static void AddChildManually(Transform parent, Transform child)
	{
		child.SetParent(parent);
		child.localScale = Vector3.one;
		child.localPosition = Vector3.zero;
	}

	public static bool CheckGuiRaycastObjects()
	{
		if (EventSystem.current == null)
		{
			return false;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		if (Input.touchCount > 0)
		{
			pointerEventData.pressPosition = Input.GetTouch(0).position;
			pointerEventData.position = Input.GetTouch(0).position;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public static List<RaycastResult> GetGuiRaycastObjects(Vector2 mousePos)
	{
		List<RaycastResult> list = new List<RaycastResult>();
		if (EventSystem.current == null)
		{
			return list;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.pressPosition = mousePos;
		pointerEventData.position = mousePos;
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list;
	}

	public static bool CheckGuiRaycastObjects(Vector2 mousePos)
	{
		if (EventSystem.current == null)
		{
			return false;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.pressPosition = mousePos;
		pointerEventData.position = mousePos;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public static void ShowMessage(string message, int buttonCount = 1, string confirmText = "110006", string cancelText = "110106", Action confirmAction = null, Action cancelAction = null, bool isChangeImg = false)
	{
		GameEntry.Lua.ShowMessage(message, buttonCount, confirmText, cancelText, confirmAction, cancelAction, cancelAction, "", isChangeImg);
	}

	public static void ShowMessages(string message, int buttonCount, string cancelText, string confirmText, Action confirmAction = null, Action cancelAction = null, bool isChangeImg = true)
	{
		GameEntry.Lua.ShowMessage(message, buttonCount, cancelText, confirmText, cancelAction, confirmAction, cancelAction, "", isChangeImg);
	}

	public static void ShowMessage(string message, Action confirmAction, Action cancelAction = null, bool isChangeImg = false)
	{
		GameEntry.Lua.ShowMessage(message, (cancelAction == null) ? 1 : 2, "110006", "110106", confirmAction, cancelAction, cancelAction, "", isChangeImg);
	}

	public static void NewShowMessage(string message, Action confirmAction, Action cancelAction = null, bool isChangeImg = false)
	{
		GameEntry.Lua.ShowMessage(message, (cancelAction == null) ? 2 : 3, "110006", "110106", confirmAction, cancelAction, cancelAction, "", isChangeImg);
	}

	public static void ShowMessage3Action(string message, string leftBtnText, string rightBtnText, Action action1, Action action2, Action closeAction, bool isChangeImg)
	{
		GameEntry.Lua.ShowMessage(message, 3, leftBtnText, rightBtnText, action1, action2, closeAction, "", isChangeImg);
	}

	public static void ShowReloadMessage(string message, Action confirmAction, Action cancelAction, string rTxt = "", float countTime = 0f)
	{
		GameEntry.Lua.ShowMessage(message, 2, "110006", rTxt, confirmAction, cancelAction, cancelAction, "", isChangeImg: false);
	}

	public static void ShowLoadingMask()
	{
	}

	public static void HideLoadingMask()
	{
	}

	public static void ShowLoadingMask(bool animation, Color color, bool isAutoTimer = true)
	{
	}

	public static void ShowTips(string msgKey, float closeTime = 3f, params object[] args)
	{
		GameEntry.Lua.ShowTips(GameEntry.Localization.GetString(msgKey, args), "", "");
	}

	public static void ShowLackResource(string tipContent, Dictionary<ResourceType, long> resources, string actionName, Action actionCallBack, bool tipContentIsKey = true, bool actionNameIsKey = true)
	{
	}

	public static void ShowRankView()
	{
		int num = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigNum", "ranking", "k3");
		if (GameEntry.Data.Building.GetMainLv() < num)
		{
			ShowTips("108111", 3f, num);
		}
	}

	public static Vector2 WordToScenePoint(Vector3 wordPosition)
	{
		CanvasScaler component = GameEntry.UIContainer.GetComponent<CanvasScaler>();
		_ = component.referenceResolution;
		_ = component.referenceResolution;
		float num = (float)Screen.width / component.referenceResolution.x * (1f - component.matchWidthOrHeight) + (float)Screen.height / component.referenceResolution.y * component.matchWidthOrHeight;
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(Camera.main, wordPosition);
		return new Vector2(vector.x / num, vector.y / num);
	}

	public static Vector2 ScreenPointToLocalPointInRectangle(Transform transform, Vector2 screenPos)
	{
		RectTransform component = transform.GetComponent<RectTransform>();
		if (component == null)
		{
			return Vector2.zero;
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle(component, screenPos, null, out var localPoint);
		return localPoint;
	}

	public static void BuildSkeletonDataAsset(string skeletonPath, SkeletonDelegate handler)
	{
	}

	public static bool checkUserName(string userName)
	{
		bool result = false;
		if (string.IsNullOrEmpty(userName))
		{
			ShowTips("280122", 3f);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool checkPwd(string pwd)
	{
		bool result = false;
		if (string.IsNullOrEmpty(pwd))
		{
			ShowTips("280121", 3f);
		}
		else if (pwd.Length < 8 || pwd.Length > 15)
		{
			ShowTips("280119", 3f);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool checkPwd(string pwd1, string pwd2)
	{
		if (checkPwd(pwd1) && !string.IsNullOrEmpty(pwd1) && !string.IsNullOrEmpty(pwd2))
		{
			if (pwd1.Equals(pwd2))
			{
				return true;
			}
			ShowTips("280126", 3f);
			return false;
		}
		return false;
	}

	public static T GetAncestorComponent<T>(GameObject origin) where T : Component
	{
		if (origin == null)
		{
			return null;
		}
		Transform transform = origin.transform;
		while (transform != null)
		{
			T component = transform.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
			transform = transform.parent;
		}
		return null;
	}

	public static Transform GetFirstChild(Transform parent, string name)
	{
		if (parent == null)
		{
			return null;
		}
		Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name == name)
			{
				return componentsInChildren[i];
			}
		}
		return null;
	}

	public static void SetTextWithImage(Text text, string des, Image image, int imageWidth = 0)
	{
		string format = "<color=#00000000><quad size={0} x=0 y=0 width=1 height=1 /></color>";
		if (imageWidth <= 0)
		{
			imageWidth = (int)image.rectTransform.sizeDelta.x;
		}
		format = string.Format(format, imageWidth);
		text.text = des.Replace("<quad/>", format);
		image.transform.position = GetPosAtText(GameEntry.UIContainer.GetComponent<Canvas>(), text, format);
		float num = text.fontSize;
		float num2 = ((num > (float)imageWidth) ? 0f : ((float)imageWidth - num));
		image.rectTransform.anchoredPosition += new Vector2(0f, num2 + (float)text.fontSize * 0.25f);
	}

	public static Vector3 GetPosAtText(Canvas canvas, Text text, string strFragment)
	{
		int num = text.text.IndexOf(strFragment);
		Vector3 zero = Vector3.zero;
		if (num > -1)
		{
			Vector3 posAtText = GetPosAtText(canvas, text, num + 1);
			Vector3 posAtText2 = GetPosAtText(canvas, text, num + strFragment.Length);
			return (posAtText + posAtText2) * 0.5f;
		}
		return GetPosAtText(canvas, text, num);
	}

	public static Vector3 GetPosAtText(Canvas canvas, Text text, int charIndex)
	{
		string text2 = text.text;
		Vector3 position = Vector3.zero;
		if (charIndex <= text2.Length && charIndex > 0)
		{
			TextGenerator textGenerator = new TextGenerator(text2.Length);
			Vector2 size = text.gameObject.GetComponent<RectTransform>().rect.size;
			textGenerator.Populate(text2, text.GetGenerationSettings(size));
			int num = text2.Substring(0, charIndex).Split(new char[1] { '\n' }).Length - 1;
			_ = text2.Substring(0, charIndex).Split(new char[1] { ' ' }).Length;
			int num2 = charIndex * 4 + num * 4 - 4;
			if (num2 < textGenerator.vertexCount)
			{
				position = (textGenerator.verts[num2].position + textGenerator.verts[num2 + 1].position + textGenerator.verts[num2 + 2].position + textGenerator.verts[num2 + 3].position) / 4f;
			}
		}
		position /= canvas.scaleFactor;
		return text.transform.TransformPoint(position);
	}

	public static float PlayAnimationReturnTime(SimpleAnimation anim, string animName)
	{
		SimpleAnimation.State state = anim.GetState(animName);
		if (state != null)
		{
			anim.Stop();
			anim.Play(animName);
			return state.length;
		}
		return -1f;
	}

	public static float PlayCrossFadeAnimationReturnTime(SimpleAnimation anim, string animName, float corssTime)
	{
		SimpleAnimation.State state = anim.GetState(animName);
		if (state != null)
		{
			anim.CrossFade(animName, corssTime);
			return state.length;
		}
		return -1f;
	}

	public static float PlayAnimationReturnTime(Animator anim, string animName)
	{
		AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++)
		{
			if (animationClips[i].name.EndsWith(animName))
			{
				anim.Play(animName, 0, 0f);
				return animationClips[i].length;
			}
		}
		return -1f;
	}

	public static float PlayAnimationReturnTime(GPUSkinningAnimator anim, string animName)
	{
		anim.Play(animName);
		return anim.GetClipLength(animName);
	}

	public static Vector2Int GetCurScreenMaxRadiusSize()
	{
		Vector2Int vector2Int = SceneManager.World.WorldToTile(SceneManager.World.ScreenPointToWorld(Vector3.zero));
		return SceneManager.World.CurTilePos - vector2Int;
	}

	public static int GetPointByMeteoriteHitPlane()
	{
		int num = 3;
		int num2 = 2;
		bool flag = true;
		Vector3 worldPos = new Vector3(0f, 0f, 0f);
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		while (num > 0)
		{
			worldPos.x = UnityEngine.Random.Range(Screen.width / 6, Screen.width - Screen.width / 6);
			worldPos.y = UnityEngine.Random.Range(Screen.height / 4, Screen.height - Screen.height / 4);
			int num3 = SceneManager.World.WorldToTileIndex(SceneManager.World.ScreenPointToWorld(worldPos));
			int indexByOffset = SceneManager.World.GetIndexByOffset(num3, num2, num2);
			flag = true;
			for (int i = 0; i <= num2; i++)
			{
				for (int j = 0; j <= num2; j++)
				{
					int indexByOffset2 = SceneManager.World.GetIndexByOffset(indexByOffset, -i, -j);
					if (SceneManager.World.GetPointInfo(indexByOffset2) != null || !SceneManager.World.IsTileWalkable(SceneManager.World.TileIndexToWorld(indexByOffset2, curServerId)) || !SceneManager.World.IsOutCityByPoint(indexByOffset2))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				return num3;
			}
			num--;
		}
		return 0;
	}

	public static BuildPointInfo GetBuildPointByMeteoriteHitGlass()
	{
		return SceneManager.World.GetBaseMainByScreen();
	}

	public static string FormatServerAllianceName(int serverId, string abbr, string name)
	{
		if (serverId <= 0)
		{
			return FormatAllianceAndName(abbr, name);
		}
		return $"#{serverId} {FormatAllianceAndName(abbr, name)}";
	}

	public static string FormatAllianceAndName(string abbr, string name)
	{
		if (string.IsNullOrEmpty(abbr))
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			return name;
		}
		return "[" + abbr + "]" + name;
	}
}
