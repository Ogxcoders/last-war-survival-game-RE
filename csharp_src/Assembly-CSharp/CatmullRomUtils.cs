using System.Collections.Generic;
using UnityEngine;

public class CatmullRomUtils
{
	private const int SIMPLE_POINT_PRE_UNIT = 50;

	public static List<Vector3> CalcCatmullRomCurve(List<Vector3> ctrlPoints, bool loop)
	{
		if (ctrlPoints == null || ctrlPoints.Count <= 2)
		{
			return null;
		}
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < ctrlPoints.Count - 1; i++)
		{
			if (i == 0)
			{
				GetCatmullRomSplinePos(list, ctrlPoints[0], ctrlPoints[0], ctrlPoints[1], ctrlPoints[2]);
			}
			else if (i == ctrlPoints.Count - 2)
			{
				GetCatmullRomSplinePos(list, ctrlPoints[i - 1], ctrlPoints[i], ctrlPoints[i + 1], ctrlPoints[i + 1]);
			}
			else
			{
				GetCatmullRomSplinePos(list, ctrlPoints[i - 1], ctrlPoints[i], ctrlPoints[i + 1], ctrlPoints[i + 2]);
			}
		}
		if (loop)
		{
			int i = ctrlPoints.Count - 1;
			GetCatmullRomSplinePos(list, ctrlPoints[i - 1], ctrlPoints[i], ctrlPoints[0], ctrlPoints[1]);
		}
		return list;
	}

	private static void GetCatmullRomSplinePos(List<Vector3> pos, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		int num = Mathf.CeilToInt(Mathf.Abs(p1.x - p2.x) * 50f);
		for (int i = 0; i <= num; i++)
		{
			pos.Add(GetCatmullRomPosition((float)i / (float)num, p0, p1, p2, p3));
		}
	}

	private static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 vector = 2f * p1;
		Vector3 vector2 = p2 - p0;
		Vector3 vector3 = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 vector4 = -p0 + 3f * p1 - 3f * p2 + p3;
		return 0.5f * (vector + vector2 * t + vector3 * t * t + vector4 * t * t * t);
	}

	public static List<Vector3> CalcCurve(Vector3 start, Vector3 end, float hwRatio)
	{
		List<Vector3> list = new List<Vector3>(2);
		list.Add(start);
		list.Add(end);
		float num = Vector3.Distance(start, end);
		Vector3 item = (start + end) / 2f + new Vector3(0f, num * hwRatio, 0f);
		list.Insert(1, item);
		return CalcCatmullRomCurve(list, 10);
	}

	public static List<Vector3> CalcCurveXZPlane(Vector3 start, Vector3 end, float ctrPointRatio, bool isLeftSide)
	{
		List<Vector3> obj = new List<Vector3>(2) { start, end };
		Vector3 vector = end - start;
		int num = (isLeftSide ? (-90) : 90);
		Vector3 vector2 = Quaternion.Euler(0f, num, 0f) * vector;
		float num2 = Vector3.Distance(start, end) * ctrPointRatio;
		Vector3 item = (start + end) / 2f + vector2.normalized * num2;
		obj.Insert(1, item);
		return CalcCatmullRomCurve(obj, 10);
	}

	public static List<Vector3> CalcCurveByPoints(Vector3 start, Vector3 end, Vector3 center)
	{
		return CalcCatmullRomCurve(new List<Vector3>(3) { start, center, end }, 10);
	}

	public static List<Vector3> CalcCatmullRomCurve(List<Vector3> ctrlPoints, int frameCount)
	{
		if (ctrlPoints == null || ctrlPoints.Count <= 2)
		{
			return null;
		}
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<float> list3 = new List<float>();
		float num = 0f;
		for (int i = 0; i < ctrlPoints.Count; i++)
		{
			if (i < ctrlPoints.Count - 1)
			{
				float num2 = Vector3.Distance(ctrlPoints[i], ctrlPoints[i + 1]);
				num += num2;
				list3.Add(num2);
			}
		}
		for (int j = 0; j < list3.Count; j++)
		{
			list2.Add((int)((float)frameCount * (list3[j] / num)));
		}
		for (int k = 0; k < ctrlPoints.Count - 1; k++)
		{
			if (k == 0)
			{
				GetCatmullRomSplinePos(list, ctrlPoints[0], ctrlPoints[0], ctrlPoints[1], ctrlPoints[2], list2[k]);
			}
			else if (k == ctrlPoints.Count - 2)
			{
				GetCatmullRomSplinePos(list, ctrlPoints[k - 1], ctrlPoints[k], ctrlPoints[k + 1], ctrlPoints[k + 1], list2[k]);
			}
			else
			{
				GetCatmullRomSplinePos(list, ctrlPoints[k - 1], ctrlPoints[k], ctrlPoints[k + 1], ctrlPoints[k + 2], list2[k]);
			}
		}
		return list;
	}

	private static void GetCatmullRomSplinePos(List<Vector3> pos, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int frame)
	{
		if (frame == 0)
		{
			pos.Add(p1);
			return;
		}
		for (int i = 0; i <= frame; i++)
		{
			pos.Add(GetCatmullRomPosition((float)i / (float)frame, p0, p1, p2, p3));
		}
	}
}
