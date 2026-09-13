using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParkourStage.Runtime.Util;

public static class PlaneMoveUtil
{
	public static void CalPingPongMoveHDeltaValue(ref float curMoveHDelta, ref int curMoveDirection, float lDelta, float rDelta, float moveSpeed, float deltaTime)
	{
		curMoveHDelta = (float)curMoveDirection * moveSpeed * deltaTime;
		if ((curMoveHDelta > 0f && curMoveHDelta > rDelta) || (curMoveHDelta < 0f && curMoveHDelta < lDelta))
		{
			curMoveDirection *= -1;
		}
	}

	public static void CalCircleParams(Vector2 startPos, Vector2 centerPos, float moveSpeed, float moveDirection, out float radius, out float startRadian, out float radianSpeed, out float circleTime)
	{
		Vector2 vector = startPos - centerPos;
		radius = Mathf.Max(vector.magnitude, 0.1f);
		startRadian = Mathf.Atan2(vector.y, vector.x);
		radianSpeed = moveSpeed / radius * moveDirection;
		circleTime = MathF.PI * 2f / radianSpeed;
	}

	public static Vector2 CalCircleMoveDeltaV2(ref float curRadian, float radius, float radianSpeed, float deltaTime)
	{
		curRadian += (float)((double)(radianSpeed * deltaTime) % 6.28318);
		return new Vector2(Mathf.Cos(curRadian) * radius, Mathf.Sin(curRadian) * radius);
	}

	public static void CalEllipseParams(Vector2 startPos, Vector2 centerPos, float vAxialScaleFactor, float moveSpeed, float moveDirection, out float a, out float b, out float statRadian, out float radianSpeed, out float circumference)
	{
		Vector2 vector = startPos - centerPos;
		statRadian = Mathf.Atan2(vector.y, vector.x * vAxialScaleFactor);
		float num = Mathf.Sin(statRadian);
		float num2 = Mathf.Cos(statRadian);
		b = Mathf.Sqrt(vector.sqrMagnitude / (vAxialScaleFactor * vAxialScaleFactor * num * num + num2 * num2));
		a = vAxialScaleFactor * b;
		circumference = MathF.PI * (3f * (a + b) - Mathf.Sqrt((3f * a + b) * (a + 3f * b)));
		radianSpeed = moveSpeed / circumference * 2f * MathF.PI * moveDirection;
	}

	public static Vector2 CalEllipseMoveDeltaV2(ref float curRadian, float a, float b, float radianSpeed, float deltaTime)
	{
		curRadian = (curRadian + radianSpeed * deltaTime) % 6.28318f;
		float x = b * Mathf.Cos(curRadian);
		float y = a * Mathf.Sin(curRadian);
		return new Vector2(x, y);
	}

	public static void GetPointsToSimulateEllipse(int totalPointsNum, in List<Vector3> totalPointsList, Vector2 startPos, Vector2 centerPos, float vAxialScaleFactor)
	{
		totalPointsList.Clear();
		CalEllipseParams(startPos, centerPos, vAxialScaleFactor, 1f, 1f, out var a, out var b, out var _, out var _, out var _);
		float num = 6.28318f / (float)totalPointsNum;
		for (int i = 0; i < totalPointsNum; i++)
		{
			float f = num * (float)i;
			float num2 = b * Mathf.Cos(f);
			float num3 = a * Mathf.Sin(f);
			totalPointsList.Add(new Vector3(centerPos.x + num2, 0f, centerPos.y + num3));
		}
	}
}
