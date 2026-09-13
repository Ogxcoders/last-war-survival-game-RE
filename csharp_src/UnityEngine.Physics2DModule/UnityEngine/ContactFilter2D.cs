using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[Serializable]
[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
[NativeHeader("Modules/Physics2D/Public/Collider2D.h")]
[NativeClass("ContactFilter", "struct ContactFilter;")]
public struct ContactFilter2D
{
	[NativeName("m_UseTriggers")]
	public bool useTriggers;

	[NativeName("m_UseLayerMask")]
	public bool useLayerMask;

	[NativeName("m_UseDepth")]
	public bool useDepth;

	[NativeName("m_UseOutsideDepth")]
	public bool useOutsideDepth;

	[NativeName("m_UseNormalAngle")]
	public bool useNormalAngle;

	[NativeName("m_UseOutsideNormalAngle")]
	public bool useOutsideNormalAngle;

	[NativeName("m_LayerMask")]
	public LayerMask layerMask;

	[NativeName("m_MinDepth")]
	public float minDepth;

	[NativeName("m_MaxDepth")]
	public float maxDepth;

	[NativeName("m_MinNormalAngle")]
	public float minNormalAngle;

	[NativeName("m_MaxNormalAngle")]
	public float maxNormalAngle;

	public const float NormalAngleUpperLimit = 359.9999f;

	public bool isFiltering => !useTriggers || useLayerMask || useDepth || useNormalAngle;

	public ContactFilter2D NoFilter()
	{
		useTriggers = true;
		useLayerMask = false;
		layerMask = -1;
		useDepth = false;
		useOutsideDepth = false;
		minDepth = float.NegativeInfinity;
		maxDepth = float.PositiveInfinity;
		useNormalAngle = false;
		useOutsideNormalAngle = false;
		minNormalAngle = 0f;
		maxNormalAngle = 359.9999f;
		return this;
	}

	private void CheckConsistency()
	{
		CheckConsistency_Injected(ref this);
	}

	public void ClearLayerMask()
	{
		useLayerMask = false;
	}

	public void SetLayerMask(LayerMask layerMask)
	{
		this.layerMask = layerMask;
		useLayerMask = true;
	}

	public void ClearDepth()
	{
		useDepth = false;
	}

	public void SetDepth(float minDepth, float maxDepth)
	{
		this.minDepth = minDepth;
		this.maxDepth = maxDepth;
		useDepth = true;
		CheckConsistency();
	}

	public void ClearNormalAngle()
	{
		useNormalAngle = false;
	}

	public void SetNormalAngle(float minNormalAngle, float maxNormalAngle)
	{
		this.minNormalAngle = minNormalAngle;
		this.maxNormalAngle = maxNormalAngle;
		useNormalAngle = true;
		CheckConsistency();
	}

	public bool IsFilteringTrigger([Writable] Collider2D collider)
	{
		return !useTriggers && collider.isTrigger;
	}

	public bool IsFilteringLayerMask(GameObject obj)
	{
		return useLayerMask && ((int)layerMask & (1 << obj.layer)) == 0;
	}

	public bool IsFilteringDepth(GameObject obj)
	{
		if (!useDepth)
		{
			return false;
		}
		if (minDepth > maxDepth)
		{
			float num = minDepth;
			minDepth = maxDepth;
			maxDepth = num;
		}
		float z = obj.transform.position.z;
		bool flag = z < minDepth || z > maxDepth;
		if (useOutsideDepth)
		{
			return !flag;
		}
		return flag;
	}

	public bool IsFilteringNormalAngle(Vector2 normal)
	{
		return IsFilteringNormalAngle_Injected(ref this, ref normal);
	}

	public bool IsFilteringNormalAngle(float angle)
	{
		return IsFilteringNormalAngleUsingAngle(angle);
	}

	private bool IsFilteringNormalAngleUsingAngle(float angle)
	{
		return IsFilteringNormalAngleUsingAngle_Injected(ref this, angle);
	}

	internal static ContactFilter2D CreateLegacyFilter(int layerMask, float minDepth, float maxDepth)
	{
		ContactFilter2D result = default(ContactFilter2D);
		result.useTriggers = Physics2D.queriesHitTriggers;
		result.SetLayerMask(layerMask);
		result.SetDepth(minDepth, maxDepth);
		return result;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void CheckConsistency_Injected(ref ContactFilter2D _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsFilteringNormalAngle_Injected(ref ContactFilter2D _unity_self, ref Vector2 normal);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsFilteringNormalAngleUsingAngle_Injected(ref ContactFilter2D _unity_self, float angle);
}
