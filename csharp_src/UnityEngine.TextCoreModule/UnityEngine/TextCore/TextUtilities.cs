namespace UnityEngine.TextCore;

internal static class TextUtilities
{
	private struct LineSegment
	{
		public Vector3 Point1;

		public Vector3 Point2;

		public LineSegment(Vector3 p1, Vector3 p2)
		{
			Point1 = p1;
			Point2 = p2;
		}
	}

	private static Vector3[] s_RectWorldCorners = new Vector3[4];

	private const string k_LookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

	private const string k_LookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

	public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
	{
		ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
		rectTransform.GetWorldCorners(s_RectWorldCorners);
		if (PointIntersectRectangle(position, s_RectWorldCorners[0], s_RectWorldCorners[1], s_RectWorldCorners[2], s_RectWorldCorners[3]))
		{
			return true;
		}
		return false;
	}

	private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		Vector3 vector = b - a;
		Vector3 rhs = m - a;
		Vector3 vector2 = c - b;
		Vector3 rhs2 = m - b;
		float num = Vector3.Dot(vector, rhs);
		float num2 = Vector3.Dot(vector2, rhs2);
		return 0f <= num && num <= Vector3.Dot(vector, vector) && 0f <= num2 && num2 <= Vector3.Dot(vector2, vector2);
	}

	public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
	{
		worldPoint = Vector3.zero;
		Ray ray = cam.ScreenPointToRay(screenPoint);
		if (!new Plane(transform.rotation * Vector3.back, transform.position).Raycast(ray, out var enter))
		{
			return false;
		}
		worldPoint = ray.GetPoint(enter);
		return true;
	}

	private static bool IntersectLinePlane(LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
	{
		intersectingPoint = Vector3.zero;
		Vector3 vector = line.Point2 - line.Point1;
		Vector3 rhs = line.Point1 - point;
		float num = Vector3.Dot(normal, vector);
		float num2 = 0f - Vector3.Dot(normal, rhs);
		if (Mathf.Abs(num) < Mathf.Epsilon)
		{
			if (num2 == 0f)
			{
				return true;
			}
			return false;
		}
		float num3 = num2 / num;
		if (num3 < 0f || num3 > 1f)
		{
			return false;
		}
		intersectingPoint = line.Point1 + num3 * vector;
		return true;
	}

	public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = a - point;
		float num = Vector3.Dot(vector, vector2);
		if (num > 0f)
		{
			return Vector3.Dot(vector2, vector2);
		}
		Vector3 vector3 = point - b;
		if (Vector3.Dot(vector, vector3) > 0f)
		{
			return Vector3.Dot(vector3, vector3);
		}
		Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
		return Vector3.Dot(vector4, vector4);
	}

	public static char ToLowerFast(char c)
	{
		if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
		{
			return c;
		}
		return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[c];
	}

	public static char ToUpperFast(char c)
	{
		if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
		{
			return c;
		}
		return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[c];
	}

	public static uint ToUpperASCIIFast(uint c)
	{
		if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
		{
			return c;
		}
		return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
	}

	public static uint ToLowerASCIIFast(uint c)
	{
		if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
		{
			return c;
		}
		return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
	}

	public static int GetHashCodeCaseSensitive(string s)
	{
		int num = 0;
		for (int i = 0; i < s.Length; i++)
		{
			num = ((num << 5) + num) ^ s[i];
		}
		return num;
	}

	public static int GetHashCodeCaseInSensitive(string s)
	{
		int num = 0;
		for (int i = 0; i < s.Length; i++)
		{
			num = ((num << 5) + num) ^ (int)ToUpperASCIIFast(s[i]);
		}
		return num;
	}

	public static uint GetSimpleHashCodeLowercase(string s)
	{
		uint num = 5381u;
		for (int i = 0; i < s.Length; i++)
		{
			num = ((num << 5) + num) ^ ToLowerFast(s[i]);
		}
		return num;
	}

	public static int HexToInt(char hex)
	{
		return hex switch
		{
			'0' => 0, 
			'1' => 1, 
			'2' => 2, 
			'3' => 3, 
			'4' => 4, 
			'5' => 5, 
			'6' => 6, 
			'7' => 7, 
			'8' => 8, 
			'9' => 9, 
			'A' => 10, 
			'B' => 11, 
			'C' => 12, 
			'D' => 13, 
			'E' => 14, 
			'F' => 15, 
			'a' => 10, 
			'b' => 11, 
			'c' => 12, 
			'd' => 13, 
			'e' => 14, 
			'f' => 15, 
			_ => 15, 
		};
	}

	public static int StringHexToInt(string s)
	{
		int num = 0;
		for (int i = 0; i < s.Length; i++)
		{
			num += HexToInt(s[i]) * (int)Mathf.Pow(16f, s.Length - 1 - i);
		}
		return num;
	}
}
