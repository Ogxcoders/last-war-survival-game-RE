using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class xLuaOptiUtils
{
	public static void Set_offsetMax(this RectTransform rt, float x, float y)
	{
		rt.offsetMax = new Vector2(x, y);
	}

	public static void Get_offsetMax(this RectTransform rt, out float x, out float y)
	{
		Vector2 offsetMax = rt.offsetMax;
		x = offsetMax.x;
		y = offsetMax.y;
	}

	public static void Set_offsetMin(this RectTransform rt, float x, float y)
	{
		rt.offsetMin = new Vector2(x, y);
	}

	public static void Get_offsetMin(this RectTransform rt, out float x, out float y)
	{
		Vector2 offsetMin = rt.offsetMin;
		x = offsetMin.x;
		y = offsetMin.y;
	}

	public static void Set_anchorMin(this RectTransform rt, float x, float y)
	{
		rt.anchorMin = new Vector2(x, y);
	}

	public static void Get_anchorMin(this RectTransform rt, out float x, out float y)
	{
		Vector2 anchorMin = rt.anchorMin;
		x = anchorMin.x;
		y = anchorMin.y;
	}

	public static void Set_anchorMax(this RectTransform rt, float x, float y)
	{
		rt.anchorMax = new Vector2(x, y);
	}

	public static void Get_anchorMax(this RectTransform rt, out float x, out float y)
	{
		Vector2 anchorMax = rt.anchorMax;
		x = anchorMax.x;
		y = anchorMax.y;
	}

	public static void Set_anchoredPosition(this RectTransform rt, float x, float y)
	{
		rt.anchoredPosition = new Vector2(x, y);
	}

	public static void Get_anchoredPosition(this RectTransform rt, out float x, out float y)
	{
		Vector2 anchoredPosition = rt.anchoredPosition;
		x = anchoredPosition.x;
		y = anchoredPosition.y;
	}

	public static void Set_pivot(this RectTransform rt, float x, float y)
	{
		rt.pivot = new Vector2(x, y);
	}

	public static void Get_pivot(this RectTransform rt, out float x, out float y)
	{
		Vector2 pivot = rt.pivot;
		x = pivot.x;
		y = pivot.y;
	}

	public static void Set_sizeDelta(this RectTransform rt, float x, float y)
	{
		rt.sizeDelta = new Vector2(x, y);
	}

	public static void Set_sizeDelta_x(this RectTransform rt, float x)
	{
		rt.sizeDelta = new Vector2(x, rt.sizeDelta.y);
	}

	public static void Set_sizeDelta_y(this RectTransform rt, float y)
	{
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, y);
	}

	public static void Get_sizeDelta(this RectTransform rt, out float x, out float y)
	{
		Vector2 sizeDelta = rt.sizeDelta;
		x = sizeDelta.x;
		y = sizeDelta.y;
	}

	public static void Get_sizeDelta_x(this RectTransform rt, out float x)
	{
		x = rt.sizeDelta.x;
	}

	public static void Get_sizeDelta_y(this RectTransform rt, out float y)
	{
		y = rt.sizeDelta.y;
	}

	public static void Get_worldCorners_x(this RectTransform rt, out float minX, out float maxX)
	{
		Vector3[] array = new Vector3[4];
		rt.GetWorldCorners(array);
		minX = array[0].x;
		maxX = array[2].x;
	}

	public static void Set_position(this Transform t, float x, float y, float z)
	{
		t.position = new Vector3(x, y, z);
	}

	public static void Set_positionX(this Transform t, float x)
	{
		Vector3 position = t.position;
		t.position = new Vector3(x, position.y, position.z);
	}

	public static void Set_positionY(this Transform t, float x, float y, float z)
	{
		Vector3 position = t.position;
		t.position = new Vector3(position.x, y, position.z);
	}

	public static void Set_positionZ(this Transform t, float x, float y, float z)
	{
		Vector3 position = t.position;
		t.position = new Vector3(position.x, position.y, z);
	}

	public static void Get_position(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 position = rt.position;
		x = position.x;
		y = position.y;
		z = position.z;
	}

	public static void Set_localPosition(this Transform t, float x, float y, float z)
	{
		t.localPosition = new Vector3(x, y, z);
	}

	public static void Get_localPosition(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 localPosition = rt.localPosition;
		x = localPosition.x;
		y = localPosition.y;
		z = localPosition.z;
	}

	public static void Set_localScale(this Transform t, float x, float y, float z)
	{
		t.localScale = new Vector3(x, y, z);
	}

	public static void Get_localScale(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 localScale = rt.localScale;
		x = localScale.x;
		y = localScale.y;
		z = localScale.z;
	}

	public static void Get_lossyScale(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 lossyScale = rt.lossyScale;
		x = lossyScale.x;
		y = lossyScale.y;
		z = lossyScale.z;
	}

	public static void Set_eulerAngles(this Transform t, float x, float y, float z)
	{
		t.eulerAngles = new Vector3(x, y, z);
	}

	public static void Get_eulerAngles(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 eulerAngles = rt.eulerAngles;
		x = eulerAngles.x;
		y = eulerAngles.y;
		z = eulerAngles.z;
	}

	public static void Set_localEulerAngles(this Transform t, float x, float y, float z)
	{
		t.localEulerAngles = new Vector3(x, y, z);
	}

	public static void Get_localEulerAngles(this Transform rt, out float x, out float y, out float z)
	{
		Vector3 localEulerAngles = rt.localEulerAngles;
		x = localEulerAngles.x;
		y = localEulerAngles.y;
		z = localEulerAngles.z;
	}

	public static void Get_rotation(this Transform t, out float x, out float y, out float z, out float w)
	{
		Quaternion rotation = t.rotation;
		x = rotation.x;
		y = rotation.y;
		z = rotation.z;
		w = rotation.w;
	}

	public static void Set_rotation(this Transform t, float x, float y, float z, float w)
	{
		t.rotation = new Quaternion(x, y, z, w);
	}

	public static void Get_localRotation(this Transform t, out float x, out float y, out float z, out float w)
	{
		Quaternion localRotation = t.localRotation;
		x = localRotation.x;
		y = localRotation.y;
		z = localRotation.z;
		w = localRotation.w;
	}

	public static void Set_localRotation(this Transform t, float x, float y, float z, float w)
	{
		t.localRotation = new Quaternion(x, y, z, w);
	}

	public static void Get_forward(this Transform t, out float x, out float y, out float z)
	{
		Vector3 forward = t.forward;
		x = forward.x;
		y = forward.y;
		z = forward.z;
	}

	public static void Set_forward(this Transform t, float x, float y, float z)
	{
		t.forward = new Vector3(x, y, z);
	}

	public static void Get_right(this Transform t, out float x, out float y, out float z)
	{
		Vector3 right = t.right;
		x = right.x;
		y = right.y;
		z = right.z;
	}

	public static void Set_right(this Transform t, float x, float y, float z)
	{
		t.right = new Vector3(x, y, z);
	}

	public static void Get_up(this Transform t, out float x, out float y, out float z)
	{
		Vector3 up = t.up;
		x = up.x;
		y = up.y;
		z = up.z;
	}

	public static void Set_up(this Transform t, float x, float y, float z)
	{
		t.up = new Vector3(x, y, z);
	}

	public static void Reset(this Transform t)
	{
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale = Vector3.one;
	}

	public static void Set_color(this Graphic graphic, float r, float g, float b, float a)
	{
		graphic.color = new Color(r, g, b, a);
	}

	public static void Set_color_r(this Graphic graphic, float r)
	{
		Color color = graphic.color;
		graphic.color = new Color(r, color.g, color.b, color.a);
	}

	public static void Set_color_g(this Graphic graphic, float g)
	{
		Color color = graphic.color;
		graphic.color = new Color(color.r, g, color.b, color.a);
	}

	public static void Set_color_b(this Graphic graphic, float b)
	{
		Color color = graphic.color;
		graphic.color = new Color(color.r, color.g, b, color.a);
	}

	public static void Set_color_a(this Graphic graphic, float a)
	{
		Color color = graphic.color;
		graphic.color = new Color(color.r, color.g, color.b, a);
	}

	public static void Get_color(this Graphic graphic, out float r, out float g, out float b, out float a)
	{
		Color color = graphic.color;
		r = color.r;
		g = color.g;
		b = color.b;
		a = color.a;
	}

	public static void Get_color_r(this Graphic graphic, out float r)
	{
		r = graphic.color.r;
	}

	public static void Get_color_g(this Graphic graphic, out float g)
	{
		g = graphic.color.g;
	}

	public static void Get_color_b(this Graphic graphic, out float b)
	{
		b = graphic.color.b;
	}

	public static void Get_color_a(this Graphic graphic, out float a)
	{
		a = graphic.color.a;
	}

	public static void Set_size(this SpriteRenderer r, float x, float y)
	{
		r.size = new Vector2(x, y);
	}

	public static void Get_size(this SpriteRenderer r, out float x, out float y)
	{
		Vector2 size = r.size;
		x = size.x;
		y = size.y;
	}

	public static void Set_color(this SpriteRenderer sr, float r, float g, float b, float a)
	{
		sr.color = new Color(r, g, b, a);
	}

	public static void Set_color_r(this SpriteRenderer sr, float r)
	{
		Color color = sr.color;
		sr.color = new Color(r, color.g, color.b, color.a);
	}

	public static void Set_color_g(this SpriteRenderer sr, float g)
	{
		Color color = sr.color;
		sr.color = new Color(color.r, g, color.b, color.a);
	}

	public static void Set_color_b(this SpriteRenderer sr, float b)
	{
		Color color = sr.color;
		sr.color = new Color(color.r, color.g, b, color.a);
	}

	public static void Set_color_a(this SpriteRenderer sr, float a)
	{
		Color color = sr.color;
		sr.color = new Color(color.r, color.g, color.b, a);
	}

	public static void Set_color_a(this SpriteMeshRenderer sr, float a)
	{
		Color color = sr.color;
		sr.color = new Color(color.r, color.g, color.b, a);
	}

	public static void Get_color(this SpriteRenderer sr, out float r, out float g, out float b, out float a)
	{
		Color color = sr.color;
		r = color.r;
		g = color.g;
		b = color.b;
		a = color.a;
	}

	public static void Get_color_r(this SpriteRenderer sr, out float r)
	{
		r = sr.color.r;
	}

	public static void Get_color_g(this SpriteRenderer sr, out float g)
	{
		g = sr.color.g;
	}

	public static void Get_color_b(this SpriteRenderer sr, out float b)
	{
		b = sr.color.b;
	}

	public static void Get_color_a(this SpriteRenderer sr, out float a)
	{
		a = sr.color.a;
	}

	public static void Set_color(this Shadow shadow, float r, float g, float b, float a)
	{
		shadow.effectColor = new Color(r, g, b, a);
	}

	public static void Native_SetText(this TextMeshProUGUI text, string value)
	{
		try
		{
			text.text = Regex.Unescape(value);
		}
		catch (ArgumentException)
		{
			text.text = value;
		}
	}

	public static void SetText_NotNative(this TextMeshProUGUI text, string value)
	{
		text.text = value;
	}
}
