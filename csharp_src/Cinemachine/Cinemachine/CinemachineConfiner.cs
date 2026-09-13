using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[SaveDuringPlay]
[ExecuteAlways]
public class CinemachineConfiner : CinemachineExtension
{
	public enum Mode
	{
		Confine2D,
		Confine3D
	}

	private class VcamExtraState
	{
		public Vector3 m_previousDisplacement;

		public float confinerDisplacement;
	}

	[Tooltip("The confiner can operate using a 2D bounding shape or a 3D bounding volume")]
	public Mode m_ConfineMode;

	[Tooltip("The volume within which the camera is to be contained")]
	public Collider m_BoundingVolume;

	[Tooltip("The 2D shape within which the camera is to be contained")]
	public Collider2D m_BoundingShape2D;

	[Tooltip("If camera is orthographic, screen edges will be confined to the volume.  If not checked, then only the camera center will be confined")]
	public bool m_ConfineScreenEdges = true;

	[Tooltip("How gradually to return the camera to the bounding volume if it goes beyond the borders.  Higher numbers are more gradual.")]
	[Range(0f, 10f)]
	public float m_Damping;

	private List<List<Vector2>> m_pathCache;

	public bool IsValid
	{
		get
		{
			if (m_ConfineMode != Mode.Confine3D || !(m_BoundingVolume != null))
			{
				if (m_ConfineMode == Mode.Confine2D)
				{
					return m_BoundingShape2D != null;
				}
				return false;
			}
			return true;
		}
	}

	public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
	{
		return GetExtraState<VcamExtraState>(vcam).confinerDisplacement > 0f;
	}

	private void OnValidate()
	{
		m_Damping = Mathf.Max(0f, m_Damping);
	}

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (IsValid && stage == CinemachineCore.Stage.Body)
		{
			Vector3 vector = ((!m_ConfineScreenEdges || !state.Lens.Orthographic) ? ConfinePoint(state.CorrectedPosition) : ConfineScreenEdges(vcam, ref state));
			VcamExtraState extraState = GetExtraState<VcamExtraState>(vcam);
			if (m_Damping > 0f && deltaTime >= 0f)
			{
				Vector3 initial = vector - extraState.m_previousDisplacement;
				initial = Damper.Damp(initial, m_Damping, deltaTime);
				vector = extraState.m_previousDisplacement + initial;
			}
			extraState.m_previousDisplacement = vector;
			state.PositionCorrection += vector;
			extraState.confinerDisplacement = vector.magnitude;
		}
	}

	public void InvalidatePathCache()
	{
		m_pathCache = null;
	}

	private bool ValidatePathCache()
	{
		Type type = ((m_BoundingShape2D == null) ? null : m_BoundingShape2D.GetType());
		if (type == typeof(PolygonCollider2D))
		{
			PolygonCollider2D polygonCollider2D = m_BoundingShape2D as PolygonCollider2D;
			if (m_pathCache == null || m_pathCache.Count != polygonCollider2D.pathCount)
			{
				m_pathCache = new List<List<Vector2>>();
				for (int i = 0; i < polygonCollider2D.pathCount; i++)
				{
					Vector2[] path = polygonCollider2D.GetPath(i);
					List<Vector2> list = new List<Vector2>();
					for (int j = 0; j < path.Length; j++)
					{
						list.Add(path[j]);
					}
					m_pathCache.Add(list);
				}
			}
			return true;
		}
		if (type == typeof(CompositeCollider2D))
		{
			CompositeCollider2D compositeCollider2D = m_BoundingShape2D as CompositeCollider2D;
			if (m_pathCache == null || m_pathCache.Count != compositeCollider2D.pathCount)
			{
				m_pathCache = new List<List<Vector2>>();
				Vector2[] array = new Vector2[compositeCollider2D.pointCount];
				for (int k = 0; k < compositeCollider2D.pathCount; k++)
				{
					int path2 = compositeCollider2D.GetPath(k, array);
					List<Vector2> list2 = new List<Vector2>();
					for (int l = 0; l < path2; l++)
					{
						list2.Add(array[l]);
					}
					m_pathCache.Add(list2);
				}
			}
			return true;
		}
		InvalidatePathCache();
		return false;
	}

	private Vector3 ConfinePoint(Vector3 camPos)
	{
		if (m_ConfineMode == Mode.Confine3D)
		{
			return m_BoundingVolume.ClosestPoint(camPos) - camPos;
		}
		if (m_BoundingShape2D.OverlapPoint(camPos))
		{
			return Vector3.zero;
		}
		if (!ValidatePathCache())
		{
			return Vector3.zero;
		}
		Vector2 vector = camPos;
		Vector2 vector2 = vector;
		float num = float.MaxValue;
		for (int i = 0; i < m_pathCache.Count; i++)
		{
			int count = m_pathCache[i].Count;
			if (count <= 0)
			{
				continue;
			}
			Vector2 vector3 = m_BoundingShape2D.transform.TransformPoint(m_pathCache[i][count - 1]);
			for (int j = 0; j < count; j++)
			{
				Vector2 vector4 = m_BoundingShape2D.transform.TransformPoint(m_pathCache[i][j]);
				Vector2 vector5 = Vector2.Lerp(vector3, vector4, vector.ClosestPointOnSegment(vector3, vector4));
				float num2 = Vector2.SqrMagnitude(vector - vector5);
				if (num2 < num)
				{
					num = num2;
					vector2 = vector5;
				}
				vector3 = vector4;
			}
		}
		return vector2 - vector;
	}

	private Vector3 ConfineScreenEdges(CinemachineVirtualCameraBase vcam, ref CameraState state)
	{
		Quaternion quaternion = Quaternion.Inverse(state.CorrectedOrientation);
		float orthographicSize = state.Lens.OrthographicSize;
		float num = orthographicSize * state.Lens.Aspect;
		Vector3 vector = quaternion * Vector3.right * num;
		Vector3 vector2 = quaternion * Vector3.up * orthographicSize;
		Vector3 zero = Vector3.zero;
		Vector3 correctedPosition = state.CorrectedPosition;
		for (int i = 0; i < 12; i++)
		{
			Vector3 vector3 = ConfinePoint(correctedPosition - vector2 - vector);
			if (vector3.AlmostZero())
			{
				vector3 = ConfinePoint(correctedPosition - vector2 + vector);
			}
			if (vector3.AlmostZero())
			{
				vector3 = ConfinePoint(correctedPosition + vector2 - vector);
			}
			if (vector3.AlmostZero())
			{
				vector3 = ConfinePoint(correctedPosition + vector2 + vector);
			}
			if (vector3.AlmostZero())
			{
				break;
			}
			zero += vector3;
			correctedPosition += vector3;
		}
		return zero;
	}
}
