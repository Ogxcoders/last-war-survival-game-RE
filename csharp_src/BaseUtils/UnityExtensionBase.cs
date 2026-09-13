using System;
using System.Collections.Generic;
using BaseUtils;
using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensionBase
{
	public static void TryAddElement<T>(this Dictionary<long, List<T>> dic, long key, T t)
	{
		if (dic.ContainsKey(key))
		{
			dic[key].Add(t);
			return;
		}
		dic[key] = new List<T>();
		dic[key].Add(t);
	}

	public static List<string> ToStrList(this string str, char splitChar)
	{
		List<string> list = new List<string>(5);
		if (!string.IsNullOrEmpty(str))
		{
			string[] array = str.Split(new char[1] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
		}
		return list;
	}

	public static List<int> ToIntList(this string str, char splitChar)
	{
		List<int> list = new List<int>(5);
		if (!string.IsNullOrEmpty(str))
		{
			string[] array = str.Split(new char[1] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(StringUtils.TryParseInt(array[i]));
			}
		}
		return list;
	}

	public static int ToInt(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return 0;
		}
		if (str.Equals(" "))
		{
			return 0;
		}
		if (str.Length == 1 && str[0] >= '0' && str[0] <= '9')
		{
			return str[0] - 48;
		}
		int result = 0;
		int.TryParse(str, out result);
		return result;
	}

	public static int ToInt(this object obj)
	{
		if (obj is string)
		{
			return ((string)obj).ToInt();
		}
		int num = 0;
		try
		{
			return Convert.ToInt32(obj);
		}
		catch (Exception)
		{
			return obj.ToString().ToInt();
		}
	}

	public static int ToInt(this ReadOnlySpan<char> str)
	{
		int num = 1;
		int num2 = 0;
		int i;
		for (i = 0; str[i] == ' '; i++)
		{
		}
		if (str[i] == '-' || str[i] == '+')
		{
			num = 1 - 2 * ((str[i++] == '-') ? 1 : 0);
		}
		while (i < str.Length && str[i] >= '0' && str[i] <= '9')
		{
			if (num2 > 214748364 || (num2 == 214748364 && str[i] - 48 > 7))
			{
				if (num == 1)
				{
					return int.MaxValue;
				}
				return int.MinValue;
			}
			num2 = 10 * num2 + (str[i++] - 48);
		}
		return num2 * num;
	}

	public static float ToFloat(this string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return 0f;
		}
		float result = 0f;
		try
		{
			result = Convert.ToSingle(str);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static float ToFloat(this ReadOnlySpan<char> str)
	{
		return (float)Strtod_CSharp.strtod(str);
	}

	public static ulong ToULong(this ReadOnlySpan<char> str)
	{
		return Strtoul_CSharp.strtoul(str);
	}

	public static float ToFloat(this object obj)
	{
		if (obj is string)
		{
			return ((string)obj).ToFloat();
		}
		float num = 0f;
		try
		{
			return Convert.ToSingle(obj);
		}
		catch (Exception)
		{
			return obj.ToString().ToFloat();
		}
	}

	public static long ToLong(this string str)
	{
		long result = 0L;
		if (str.Contains("."))
		{
			List<string> list = new List<string>();
			StringUtils.SplitString(str, '.', ref list);
			long.TryParse(list[0], out result);
		}
		else
		{
			long.TryParse(str, out result);
		}
		return result;
	}

	public static GameObject Instantiate(this GameObject go)
	{
		if (go != null)
		{
			return UnityEngine.Object.Instantiate(go);
		}
		return go;
	}

	public static void Destroy(this GameObject go)
	{
		if (go != null)
		{
			UnityEngine.Object.Destroy(go);
		}
	}

	public static bool InScene(this GameObject gameObject)
	{
		return gameObject.scene.name != null;
	}

	public static void SetLayerRecursively(this GameObject gameObject, int layer)
	{
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layer;
		}
	}

	public static Vector2 ToVector2(this Vector3 vector3)
	{
		return new Vector2(vector3.x, vector3.z);
	}

	public static Vector3 ToVector3(this Vector2 vector2)
	{
		return new Vector3(vector2.x, 0f, vector2.y);
	}

	public static void DestroyEx(this GameObject obj)
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(obj);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(obj, allowDestroyingAssets: false);
		}
	}

	public static void SetPositionX(this Transform transform, float newValue)
	{
		Vector3 position = transform.position;
		position.x = newValue;
		transform.position = position;
	}

	public static void SetPositionY(this Transform transform, float newValue)
	{
		Vector3 position = transform.position;
		position.y = newValue;
		transform.position = position;
	}

	public static void SetPositionZ(this Transform transform, float newValue)
	{
		Vector3 position = transform.position;
		position.z = newValue;
		transform.position = position;
	}

	public static void AddPositionX(this Transform transform, float deltaValue)
	{
		Vector3 position = transform.position;
		position.x += deltaValue;
		transform.position = position;
	}

	public static void AddPositionY(this Transform transform, float deltaValue)
	{
		Vector3 position = transform.position;
		position.y += deltaValue;
		transform.position = position;
	}

	public static void AddPositionZ(this Transform transform, float deltaValue)
	{
		Vector3 position = transform.position;
		position.z += deltaValue;
		transform.position = position;
	}

	public static void SetLocalPositionX(this Transform transform, float newValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x = newValue;
		transform.localPosition = localPosition;
	}

	public static void SetLocalPositionY(this Transform transform, float newValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.y = newValue;
		transform.localPosition = localPosition;
	}

	public static void SetLocalPositionZ(this Transform transform, float newValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.z = newValue;
		transform.localPosition = localPosition;
	}

	public static void AddLocalPositionX(this Transform transform, float deltaValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x += deltaValue;
		transform.localPosition = localPosition;
	}

	public static void AddLocalPositionY(this Transform transform, float deltaValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.y += deltaValue;
		transform.localPosition = localPosition;
	}

	public static void AddLocalPositionZ(this Transform transform, float deltaValue)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.z += deltaValue;
		transform.localPosition = localPosition;
	}

	public static void SetLocalScaleX(this Transform transform, float newValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.x = newValue;
		transform.localScale = localScale;
	}

	public static void SetLocalScaleY(this Transform transform, float newValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.y = newValue;
		transform.localScale = localScale;
	}

	public static void SetLocalScaleZ(this Transform transform, float newValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.z = newValue;
		transform.localScale = localScale;
	}

	public static void AddLocalScaleX(this Transform transform, float deltaValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.x += deltaValue;
		transform.localScale = localScale;
	}

	public static void AddLocalScaleY(this Transform transform, float deltaValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.y += deltaValue;
		transform.localScale = localScale;
	}

	public static void AddLocalScaleZ(this Transform transform, float deltaValue)
	{
		Vector3 localScale = transform.localScale;
		localScale.z += deltaValue;
		transform.localScale = localScale;
	}

	public static Transform FindChildByName(this Transform parent, string name, int maxFirstLevelChildren = 5, int maxDepth = 10)
	{
		return parent.FindChildByName(name, maxFirstLevelChildren, maxDepth, 0);
	}

	private static Transform FindChildByName(this Transform parent, string name, int maxFirstLevelChildren, int maxDepth, int currentDepth)
	{
		if (currentDepth > maxDepth)
		{
			return null;
		}
		if (parent.name == name)
		{
			return parent;
		}
		int num = 0;
		foreach (Transform item in parent)
		{
			if (currentDepth == 0 && num >= maxFirstLevelChildren)
			{
				break;
			}
			Transform transform = item.FindChildByName(name, maxDepth, maxFirstLevelChildren, currentDepth + 1);
			if (transform != null)
			{
				return transform;
			}
			num++;
		}
		return null;
	}

	public static Component FindAndGetComponent(this Transform tran, string path, Type type)
	{
		Transform transform = tran.Find(path);
		if (transform == null)
		{
			return null;
		}
		return transform.GetComponent(type);
	}

	public static Component GetComponent_RectTransform(this Transform tran)
	{
		return tran.GetComponent<RectTransform>();
	}

	public static Component GetComponent_RectTransform(this GameObject obj)
	{
		return obj.GetComponent<RectTransform>();
	}

	public static Component GetComponent_Text(this Transform tran)
	{
		return tran.GetComponent<Text>();
	}

	public static Component GetComponent_Text(this GameObject obj)
	{
		return obj.GetComponent<Text>();
	}

	public static Component GetComponent_Image(this Transform tran)
	{
		return tran.GetComponent<Image>();
	}

	public static Component GetComponent_Image(this GameObject obj)
	{
		return obj.GetComponent<Image>();
	}

	public static Component GetComponent_Button(this Transform tran)
	{
		return tran.GetComponent<Button>();
	}

	public static Component GetComponent_Button(this GameObject obj)
	{
		return obj.GetComponent<Button>();
	}
}
