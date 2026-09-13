using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using BitBenderGames;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using RiverGame.PerformanceAnalysis;
using Spine;
using Spine.Unity;
using TimelineScript;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

public class CSUtils
{
	private const double Mebibyte = 1048576.0;

	private static Vector3 topVec = new Vector3(0f, 0f, 0f);

	private static Vector3 downVec = new Vector3(0f, 0f, 0f);

	private static Collider[] colliders = new Collider[200];

	public static void SetPositionFromInput(Transform tf)
	{
		Vector3 position = GameEntry.UICamera.ScreenToWorldPoint(Input.mousePosition);
		tf.position = position;
	}

	public static Vector3 WorldPositionToUISpacePosition(Vector3 worldPosition)
	{
		Vector3 position = Camera.main.WorldToScreenPoint(worldPosition);
		return GameEntry.UICamera.ScreenToWorldPoint(position);
	}

	public static void WorldPositionToUITransform(Transform worldTransform, Camera mainCamera, RectTransform uiRectTransform, float offsetY = 0f, float offsetZ = 0f)
	{
		Vector3 position = mainCamera.WorldToScreenPoint(worldTransform.position + Vector3.up * offsetY + Vector3.forward * offsetZ);
		position = GameEntry.UICamera.ScreenToWorldPoint(position);
		uiRectTransform.position = position;
	}

	public static Vector2 WorldTransformToUIPosition(Transform worldTransform, Camera mainCamera, RectTransform parentRectTransform, float offsetY = 0f, float offsetZ = 0f)
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldTransform.position + Vector3.up * offsetY + Vector3.forward * offsetZ);
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPoint, GameEntry.UICamera, out var localPoint))
		{
			return localPoint;
		}
		return Vector2.zero;
	}

	public static Vector2 UIWorldToUIPos(Vector3 worldPos, RectTransform rect)
	{
		Vector2 localPoint = RectTransformUtility.WorldToScreenPoint(GameEntry.UICamera, worldPos);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, localPoint, GameEntry.UICamera, out localPoint);
		return localPoint;
	}

	public static Vector2 WorldToUIPos(Vector3 worldPos, RectTransform rect)
	{
		if (GameEntry.UICamera == null)
		{
			return Vector3.zero;
		}
		worldPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, worldPos, GameEntry.UICamera, out var localPoint))
		{
			return localPoint;
		}
		return Vector3.zero;
	}

	public static bool IsPointerOverUIObject()
	{
		if (TouchWrapper.TouchCount > 0)
		{
			foreach (WrappedTouch touch in TouchWrapper.Touches)
			{
				if (EventSystem.current.IsPointerOverGameObject(touch.FingerId))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void SetTransformUISpacePosition(Transform targetTransform, float posX, float posY, float posZ)
	{
		if (!(targetTransform == null))
		{
			Vector3 position = WorldPositionToUISpacePosition(new Vector3(posX, posY, posZ));
			targetTransform.position = position;
		}
	}

	public static void GetPositionAndEulerAngleY(Transform transform, out float x, out float y, out float z, out float angle)
	{
		x = 0f;
		y = 0f;
		z = 0f;
		angle = 0f;
		if (!(transform == null))
		{
			Vector3 position = transform.position;
			x = position.x;
			y = position.y;
			z = position.z;
			angle = transform.eulerAngles.y;
		}
	}

	public static void DOTweenTo_RectTransformPos_X(RectTransform _rectTransform, float endX, float time, Action callback = null)
	{
		float oldY = _rectTransform.anchoredPosition.y;
		DOTween.To(() => _rectTransform.anchoredPosition.x, delegate(float pos)
		{
			_rectTransform.anchoredPosition = new Vector2(pos, oldY);
		}, endX, time).onComplete = delegate
		{
			callback?.Invoke();
		};
	}

	public static void DOTweenTo_RectTransformPos_Y(RectTransform _rectTransform, float endY, float time, Action callback = null)
	{
		float oldX = _rectTransform.anchoredPosition.x;
		DOTween.To(() => _rectTransform.anchoredPosition.y, delegate(float pos)
		{
			_rectTransform.anchoredPosition = new Vector2(oldX, pos);
		}, endY, time).onComplete = delegate
		{
			callback?.Invoke();
		};
	}

	public static void DOTweenTo_MinWidth(LayoutElement _rectTransform, float endValue, float time, Action callback = null)
	{
		DOTween.To(() => _rectTransform.minWidth, delegate(float pos)
		{
			_rectTransform.minWidth = pos;
		}, endValue, time).onComplete = delegate
		{
			callback?.Invoke();
		};
	}

	public static void DOTweenTo_ScrollRect_Horizontal(ScrollRect _scrollRect, float endValue, float time, Action callback = null)
	{
		DOTween.To(() => _scrollRect.horizontalNormalizedPosition, delegate(float pos)
		{
			_scrollRect.horizontalNormalizedPosition = pos;
		}, endValue, time).onComplete = delegate
		{
			callback?.Invoke();
		};
	}

	public static int GetTriggerIds(float tx, float ty, float tz, float dx, float dy, float dz, float radius, GameObject srcObj, LuaTable outTable, string layerName)
	{
		if (srcObj == null)
		{
			return -1;
		}
		LayerMask layerMask = LayerMask.GetMask(layerName);
		Vector3 position = srcObj.transform.position;
		int num = 1;
		topVec.Set(tx, ty, tz);
		downVec.Set(dx, dy, dz);
		int num2 = Physics.OverlapCapsuleNonAlloc(topVec, downVec, radius, colliders, layerMask);
		int num3 = ((num2 > colliders.Length) ? colliders.Length : num2);
		int num4 = 0;
		for (int i = 0; i < num3; i++)
		{
			Collider collider = colliders[i];
			CitySpaceManTrigger componentInParent = collider.transform.GetComponentInParent<CitySpaceManTrigger>();
			if (componentInParent != null)
			{
				Vector3 position2 = collider.transform.position;
				Debug.DrawLine(position2, position, Color.red, 200f);
				Vector2 lhs = new Vector2(position2.x - position.x, position2.z - position.z);
				Vector2 rhs = new Vector2(srcObj.transform.forward.x, srcObj.transform.forward.z);
				if (Vector2.Dot(lhs, rhs) > 0f)
				{
					num4++;
					outTable.Set(num++, componentInParent.ObjectId);
				}
			}
		}
		return num4;
	}

	public static bool Hit(Transform transform, float radius, float speed, out Vector3 oritation)
	{
		float radius2 = radius;
		radius = 0.54f;
		speed *= 3f;
		LayerMask layerMask = LayerMask.GetMask("Default");
		Vector3 position = transform.position;
		Vector3 point = transform.position + new Vector3(0f, 1f, 0f);
		if (Physics.CapsuleCast(position, point, radius, transform.forward, out var hitInfo, speed, layerMask))
		{
			if (Physics.CapsuleCast(position, point, radius2, transform.forward, out var _, speed, layerMask))
			{
				oritation = Vector3.zero;
				return true;
			}
			Vector3 vector = Vector3.Cross(hitInfo.normal, Vector3.up);
			if (Vector3.Dot(transform.forward, vector) > 0f)
			{
				oritation = vector;
			}
			else
			{
				oritation = -vector;
			}
			if (Physics.CapsuleCast(position, point, radius, oritation, out var _, speed, layerMask))
			{
				oritation = Vector3.zero;
				return true;
			}
			return true;
		}
		oritation = Vector3.zero;
		return false;
	}

	public static bool Hit2(Vector3 pos, Vector3 forward, float radius, float speed, out Vector3 oritation)
	{
		LayerMask layerMask = LayerMask.GetMask("Default");
		Vector3 point = pos + new Vector3(0f, 1f, 0f);
		if (Physics.CapsuleCast(pos, point, radius, forward, out var hitInfo, speed, layerMask))
		{
			Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 0.5f);
			oritation = Vector3.zero;
			return true;
		}
		oritation = Vector3.zero;
		return false;
	}

	public static Vector2[] PoissonDiscSampler(float radius, float width, float height)
	{
		int num = 6;
		float radius2 = radius * radius;
		float num2 = radius / Mathf.Sqrt(2f);
		int num3 = Mathf.CeilToInt(width / num2);
		int num4 = Mathf.CeilToInt(height / num2);
		Vector2[] array = new Vector2[num3 * num4];
		List<Vector2> list = new List<Vector2>();
		Sample(width / 2f, height / 2f, array, num2, num3, list);
		while (list.Count > 0)
		{
			int num5 = Mathf.FloorToInt(UnityEngine.Random.value * (float)list.Count);
			Vector2 vector = list[num5];
			float value = UnityEngine.Random.value;
			float num6 = 1E-07f;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				float f = MathF.PI * 2f * (value + (float)i * 1f / (float)num);
				float num7 = radius + num6;
				float num8 = vector.x + num7 * Mathf.Cos(f);
				float num9 = vector.y + num7 * Mathf.Sin(f);
				if (num8 >= 0f && num8 < width && num9 >= 0f && num9 < height && Far(num8, num9, array, num2, num3, num4, radius2))
				{
					Sample(num8, num9, array, num2, num3, list);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (num5 < list.Count - 1)
				{
					Vector2 value2 = list[list.Count - 1];
					list[num5] = value2;
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					list.RemoveAt(list.Count - 1);
				}
			}
		}
		List<Vector2> list2 = new List<Vector2>();
		int j = 0;
		for (int num10 = array.Length; j < num10; j++)
		{
			Vector2 vector2 = array[j];
			if (vector2 != Vector2.zero)
			{
				list2.Add(vector2);
			}
		}
		list2.Sort(GridSorter);
		return list2.ToArray();
	}

	private static int GridSorter(Vector2 x, Vector2 y)
	{
		float x2 = x.x;
		float x3 = y.x;
		if (!Mathf.Approximately(x2, x3))
		{
			if (!(x3 - x2 > 0f))
			{
				return -1;
			}
			return 1;
		}
		float y2 = x.y;
		float y3 = y.y;
		if (!Mathf.Approximately(y2, y3))
		{
			if (!(y3 - y2 > 0f))
			{
				return -1;
			}
			return 1;
		}
		return 0;
	}

	private static Vector2 Sample(float x, float y, Vector2[] grid, float cellSize, int gridWidth, List<Vector2> queue)
	{
		Vector2 vector = new Vector2(x, y);
		int num = gridWidth * Mathf.FloorToInt(y / cellSize) + Mathf.FloorToInt(x / cellSize);
		grid[num] = vector;
		queue.Add(vector);
		return vector;
	}

	private static bool Far(float x, float y, Vector2[] grid, float cellSize, int gridWidth, int gridHeight, float radius2)
	{
		int num = Mathf.FloorToInt(x / cellSize);
		int num2 = Mathf.FloorToInt(y / cellSize);
		int num3 = Mathf.Max(num - 2, 0);
		int num4 = Mathf.Max(num2 - 2, 0);
		int num5 = Mathf.Min(num + 3, gridWidth);
		int num6 = Mathf.Min(num2 + 3, gridHeight);
		for (int i = num4; i < num6; i++)
		{
			int num7 = i * gridWidth;
			for (int j = num3; j < num5; j++)
			{
				int num8 = num7 + j;
				if (num8 < grid.Length)
				{
					Vector2 vector = grid[num8];
					float num9 = vector.x - x;
					float num10 = vector.y - y;
					if (num9 * num9 + num10 * num10 < radius2)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public static bool GetCameraTouchWorldPos(MobileTouchCamera touchCamera, out float hitX, out float hitZ)
	{
		hitX = 0f;
		hitZ = 0f;
		if ((object)touchCamera == null)
		{
			return false;
		}
		if (TouchWrapper.TouchCount > 0)
		{
			WrappedTouch wrappedTouch = TouchWrapper.Touches[0];
			if (wrappedTouch == null)
			{
				return false;
			}
			Vector3 position = wrappedTouch.Position;
			Ray ray = touchCamera.ScreenPointToRay(position);
			if (new Plane(Vector3.up, 0f).Raycast(ray, out var enter))
			{
				Vector3 point = ray.GetPoint(enter);
				hitX = point.x;
				hitZ = point.z;
				return true;
			}
		}
		return false;
	}

	public static bool GetCameraTouchScreenPos(MobileTouchCamera touchCamera, out float screenX, out float screenY)
	{
		screenX = 0f;
		screenY = 0f;
		if ((object)touchCamera == null)
		{
			return false;
		}
		if (TouchWrapper.TouchCount > 0)
		{
			WrappedTouch wrappedTouch = TouchWrapper.Touches[0];
			if (wrappedTouch == null)
			{
				return false;
			}
			Vector3 position = wrappedTouch.Position;
			screenX = position.x;
			screenY = position.y;
			return true;
		}
		return false;
	}

	public static void DoCommonVibration(float intensity, float sharpness, float duration)
	{
		MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration);
	}

	public static AnimationCurve StringToCurve(string str)
	{
		return BulletMotionEditor.StringToCurve(str);
	}

	public static void UpdatePVEStaticMgr(MobileTouchCamera touchCamera, PVEDecorationManagerBase pveStaticManager)
	{
		Vector2Int vector2Int = TileCoord.WorldToTile(touchCamera.GetCameraTargetPos());
		pveStaticManager.OnUpdate(vector2Int.x, vector2Int.y);
	}

	public static void UpdatePVEStaticMgrWithRealPos(MobileTouchCamera touchCamera, PVEDecorationManagerBase pveStaticManager, float offsetZ, float renderOffsetZ)
	{
		Vector3 cameraPos = touchCamera.GetCameraPos();
		Vector2Int vector2Int = TileCoord.WorldToTile(cameraPos + Vector3.forward * offsetZ);
		Vector2Int vector2Int2 = TileCoord.WorldToTile(cameraPos + Vector3.forward * (offsetZ - renderOffsetZ));
		pveStaticManager.OnUpdateData(vector2Int.x, vector2Int.y, vector2Int2.x, vector2Int2.y);
	}

	public static void UpdatePVEStaticMgrDrawMesh(PVEDecorationManagerBase pveStaticManager)
	{
		if (pveStaticManager is PVEStaticDecorationManager pVEStaticDecorationManager)
		{
			pVEStaticDecorationManager.UpdateDrawBatch();
		}
	}

	public static void ModifySaveGirlAnimationStartPos(SkeletonGraphic skeletonGraphic, string animationName, string boneName, float startX, float startY)
	{
		Skeleton skeleton = skeletonGraphic.Skeleton;
		if (skeleton == null)
		{
			return;
		}
		Spine.Animation animation = skeleton.Data.FindAnimation(animationName);
		if (animation == null)
		{
			return;
		}
		ExposedList<Timeline> timelines = animation.Timelines;
		if (timelines == null)
		{
			return;
		}
		int boneIndex = GetBoneIndex(skeleton.Data, boneName);
		if (boneIndex < 0)
		{
			return;
		}
		foreach (Timeline item in timelines)
		{
			if (item is TranslateTimeline translateTimeline && translateTimeline.BoneIndex == boneIndex)
			{
				int frameEntries = translateTimeline.FrameEntries;
				int num = translateTimeline.Frames.Length / frameEntries;
				float time = translateTimeline.Frames[0];
				translateTimeline.SetFrame(0, time, startX, startY);
				float time2 = translateTimeline.Frames[frameEntries];
				translateTimeline.SetFrame(1, time2, startX, startY);
				float time3 = translateTimeline.Frames[(num - 1) * frameEntries];
				translateTimeline.SetFrame(num - 1, time3, startX, startY);
				break;
			}
		}
	}

	private static int GetBoneIndex(SkeletonData skeletonData, string boneName)
	{
		for (int i = 0; i < skeletonData.Bones.Count; i++)
		{
			if (skeletonData.Bones.Items[i].Name == boneName)
			{
				return i;
			}
		}
		return -1;
	}

	public static void GetMemRecord(out double nrsv, out double nuse, out double mrsv, out double muse)
	{
		nrsv = (double)PerformanceMetrics.MemoryNativeReservedLong / 1048576.0;
		nuse = (double)PerformanceMetrics.MemoryNativeUsedLong / 1048576.0;
		mrsv = (double)PerformanceMetrics.MemoryMonoReservedLong / 1048576.0;
		muse = (double)PerformanceMetrics.MemoryMonoUsedLong / 1048576.0;
	}

	public static bool CheckiOSSystemVersion(int majorVersion)
	{
		if (!SDKManager.IS_IPhonePlayer())
		{
			return false;
		}
		if (int.TryParse(Regex.Match(SystemInfo.operatingSystem, "\\d+").Value, out var result) && result > 0 && result < majorVersion)
		{
			return true;
		}
		return false;
	}

	public static bool ClickInUIRect(RectTransform rectTransform, float screenPointX, float screenPointY)
	{
		if (GameEntry.UICamera == null)
		{
			return false;
		}
		if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, new Vector2(screenPointX, screenPointY), GameEntry.UICamera))
		{
			return true;
		}
		return false;
	}

	public static void LoadPveShotTex(string dirPath)
	{
		string path = (string.IsNullOrEmpty(dirPath) ? "C:\\Users\\admin\\Downloads\\Screenshots" : dirPath);
		if (!Directory.Exists(path))
		{
			return;
		}
		string text = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path) + "_Restored");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string[] files = Directory.GetFiles(path, "*.frame", SearchOption.TopDirectoryOnly);
		if (files.Length == 0)
		{
			return;
		}
		int num = 0;
		string[] array = files;
		foreach (string text2 in array)
		{
			try
			{
				int width;
				int height;
				byte[] compressed;
				using (FileStream input = new FileStream(text2, FileMode.Open, FileAccess.Read))
				{
					using BinaryReader binaryReader = new BinaryReader(input);
					byte[] array2 = binaryReader.ReadBytes(4);
					if (array2[0] != 70 || array2[1] != 82 || array2[2] != 77)
					{
						break;
					}
					binaryReader.ReadInt32();
					width = binaryReader.ReadInt32();
					height = binaryReader.ReadInt32();
					binaryReader.ReadSingle();
					int count = binaryReader.ReadInt32();
					compressed = binaryReader.ReadBytes(count);
				}
				byte[] data = DecompressData(compressed);
				Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
				texture2D.LoadRawTextureData(data);
				texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
				string path2 = Path.Combine(text, fileNameWithoutExtension + ".jpg");
				byte[] bytes = texture2D.EncodeToJPG(90);
				File.WriteAllBytes(path2, bytes);
				UnityEngine.Object.DestroyImmediate(texture2D);
				num++;
			}
			catch (Exception arg)
			{
				Debug.LogError($"[CSUtils. LoadPveShotTex] 还原失败: {text2}\n{arg}");
			}
		}
	}

	private static byte[] DecompressData(byte[] compressed)
	{
		using MemoryStream stream = new MemoryStream(compressed);
		using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
		using MemoryStream memoryStream = new MemoryStream();
		deflateStream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	public static void SetTimelineExtOpen(bool isOpen)
	{
		TimelineInteractionManager.Inst.SetTimelineExtOpen(isOpen);
	}

	public static void OnTimelineInteractionPlotDone(int plotId)
	{
		TimelineInteractionManager.Inst.OnPlotDone(plotId);
	}

	public static void OnTimelineInteractionQTE1Done()
	{
		TimelineInteractionManager.Inst.OnQTE1Done();
	}
}
