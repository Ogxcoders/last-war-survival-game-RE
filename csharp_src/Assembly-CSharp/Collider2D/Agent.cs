using UnityEngine;

namespace Collider2D;

public struct Agent
{
	public int Id;

	public int ColliderId;

	public Collider2DType ColliderType;

	private float ColliderCenterX;

	private float ColliderCenterY;

	private float ColliderExtendX;

	private float ColliderExtendY;

	public int Layer;

	public float PosX;

	public float PosY;

	public float TransformedColliderCenterX;

	public float TransformedColliderCenterY;

	public float TransformedColliderExtendX;

	public float TransformedColliderExtendY;

	public float TransformedAABBMinX;

	public float TransformedAABBMinY;

	public float TransformedAABBMaxX;

	public float TransformedAABBMaxY;

	public void SetColliderData(Collider2DType collider2DType, float colliderCenterX, float colliderCenterY, float colliderExtendX, float colliderExtendY, int layer)
	{
		ColliderType = collider2DType;
		ColliderCenterX = colliderCenterX;
		ColliderCenterY = colliderCenterY;
		ColliderExtendX = colliderExtendX;
		ColliderExtendY = colliderExtendY;
		Layer = 1 << layer;
	}

	public void ResetByTransform(Transform transform)
	{
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.position;
		float x = position.x;
		float z = position.z;
		PosX = x;
		PosY = z;
		Vector3 right = transform.right;
		float x2 = right.x;
		float z2 = right.z;
		Vector3 forward = transform.forward;
		float x3 = forward.x;
		float z3 = forward.z;
		Vector3 lossyScale = transform.lossyScale;
		float x4 = lossyScale.x;
		float z4 = lossyScale.z;
		float num = ColliderCenterX * x4;
		float num2 = ColliderCenterY * z4;
		float num3 = num * x2 + num2 * x3 + x;
		float num4 = num * z2 + num2 * z3 + z;
		if (ColliderType == Collider2DType.Cube)
		{
			float num5 = ColliderExtendX * x4;
			float num6 = ColliderExtendY * z4;
			float num7 = num5 * x2;
			float num8 = num6 * x3;
			float num9 = num5 * z2;
			float num10 = num6 * z3;
			float num11 = num3 + num7 + num8;
			float num12 = num4 + num9 + num10;
			float num13 = num3 + num7 - num8;
			float num14 = num4 + num9 - num10;
			float num15 = num3 - num7 - num8;
			float num16 = num4 - num9 - num10;
			float num17 = num3 - num7 + num8;
			float num18 = num4 - num9 + num10;
			float num19 = num11;
			if (num13 < num19)
			{
				num19 = num13;
			}
			if (num15 < num19)
			{
				num19 = num15;
			}
			if (num17 < num19)
			{
				num19 = num17;
			}
			float num20 = num11;
			if (num13 > num20)
			{
				num20 = num13;
			}
			if (num15 > num20)
			{
				num20 = num15;
			}
			if (num17 > num20)
			{
				num20 = num17;
			}
			float num21 = num12;
			if (num14 < num21)
			{
				num21 = num14;
			}
			if (num16 < num21)
			{
				num21 = num16;
			}
			if (num18 < num21)
			{
				num21 = num18;
			}
			float num22 = num12;
			if (num14 > num22)
			{
				num22 = num14;
			}
			if (num16 > num22)
			{
				num22 = num16;
			}
			if (num18 > num22)
			{
				num22 = num18;
			}
			TransformedAABBMinX = num19;
			TransformedAABBMinY = num21;
			TransformedAABBMaxX = num20;
			TransformedAABBMaxY = num22;
			TransformedColliderExtendX = num5;
			TransformedColliderExtendY = num6;
		}
		else if (ColliderType == Collider2DType.Circle)
		{
			float num23 = ((x4 > z4) ? x4 : z4);
			float num24 = ColliderExtendX * num23;
			TransformedColliderExtendX = (TransformedColliderExtendY = num24);
			TransformedAABBMinX = num3 - num24;
			TransformedAABBMinY = num4 - num24;
			TransformedAABBMaxX = num3 + num24;
			TransformedAABBMaxY = num4 + num24;
		}
		TransformedColliderCenterX = num3;
		TransformedColliderCenterY = num4;
	}
}
