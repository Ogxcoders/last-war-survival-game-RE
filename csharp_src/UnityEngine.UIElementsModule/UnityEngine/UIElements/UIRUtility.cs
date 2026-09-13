namespace UnityEngine.UIElements;

internal static class UIRUtility
{
	public static readonly string k_DefaultShaderName = "Hidden/Internal-UIRDefault";

	public const float k_ClearZ = 0.99f;

	public const float k_MeshPosZ = 0.995f;

	public const float k_MaskPosZ = -0.995f;

	public static Vector4 ToVector4(this Rect rc)
	{
		return new Vector4(rc.xMin, rc.yMin, rc.xMax, rc.yMax);
	}

	public static bool IsRoundRect(VisualElement ve)
	{
		IResolvedStyle resolvedStyle = ve.resolvedStyle;
		return !(resolvedStyle.borderTopLeftRadius < Mathf.Epsilon) || !(resolvedStyle.borderTopRightRadius < Mathf.Epsilon) || !(resolvedStyle.borderBottomLeftRadius < Mathf.Epsilon) || !(resolvedStyle.borderBottomRightRadius < Mathf.Epsilon);
	}

	public static bool IsVectorImageBackground(VisualElement ve)
	{
		return ve.computedStyle.backgroundImage.value.vectorImage != null;
	}

	public static void Destroy(Object obj)
	{
		if (!(obj == null))
		{
			if (Application.isPlaying)
			{
				Object.Destroy(obj);
			}
			else
			{
				Object.DestroyImmediate(obj);
			}
		}
	}
}
