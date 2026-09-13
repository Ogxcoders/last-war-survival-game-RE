using Box2DSharp.Common;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame;

public static class FixedMathExt
{
	public static FloatVector2 ToCSharpVector2(this in FVector2 vector2)
	{
		return new FloatVector2((float)vector2.X, (float)vector2.Y);
	}

	public static FloatVector3 ToCSharpVector3(this in FVector2 vector3)
	{
		return new FloatVector3((float)vector3.X, (float)vector3.Y, 0f);
	}

	public static FloatVector3 ToCSharpVector3(this in FVector3 vector3)
	{
		return new FloatVector3((float)vector3.X, (float)vector3.Y, (float)vector3.Z);
	}

	public static FVector2 ToFVector2(this Vector3 v3)
	{
		return new FVector2(v3.x, v3.y);
	}

	public static FVector2 ToFVector2(this in Vector2 vector2)
	{
		return new FVector2(vector2.x, vector2.y);
	}

	public static FVector3 ToFVector3(this Vector3 v3)
	{
		return new FVector3(v3.x, v3.y, v3.z);
	}

	public static Vector2 ToUnityVector2(this FVector2 vector2)
	{
		return new Vector2(vector2.X.AsFloat, vector2.Y.AsFloat);
	}

	public static Vector3 ToUnityVector3(this FVector2 v2, float z = 0f)
	{
		return new Vector3((float)v2.X, (float)v2.Y, z);
	}

	public static Vector3 ToUnityVector3(this in Vector2 vector2)
	{
		return new Vector3(vector2.x, vector2.y, 0f);
	}

	public static Vector3 ToUnityVector3(this FVector3 v2)
	{
		return new Vector3((float)v2.X, (float)v2.Y, (float)v2.Z);
	}
}
