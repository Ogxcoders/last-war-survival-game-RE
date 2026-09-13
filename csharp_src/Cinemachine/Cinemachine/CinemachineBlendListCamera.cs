using System;
using System.Collections.Generic;
using System.Text;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[DisallowMultipleComponent]
[ExecuteAlways]
[AddComponentMenu("Cinemachine/CinemachineBlendListCamera")]
public class CinemachineBlendListCamera : CinemachineVirtualCameraBase
{
	[Serializable]
	public struct Instruction
	{
		[Tooltip("The virtual camera to activate when this instruction becomes active")]
		public CinemachineVirtualCameraBase m_VirtualCamera;

		[Tooltip("How long to wait (in seconds) before activating the next virtual camera in the list (if any)")]
		public float m_Hold;

		[CinemachineBlendDefinitionProperty]
		[Tooltip("How to blend to the next virtual camera in the list (if any)")]
		public CinemachineBlendDefinition m_Blend;
	}

	[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
	public bool m_ShowDebugText;

	[SerializeField]
	[HideInInspector]
	[NoSaveDuringPlay]
	internal CinemachineVirtualCameraBase[] m_ChildCameras;

	[Tooltip("The set of instructions for enabling child cameras.")]
	public Instruction[] m_Instructions;

	private CameraState m_State = CameraState.Default;

	private float mActivationTime = -1f;

	private int mCurrentInstruction;

	private CinemachineBlend mActiveBlend;

	public override string Description
	{
		get
		{
			if (mActiveBlend != null)
			{
				return mActiveBlend.Description;
			}
			ICinemachineCamera liveChild = LiveChild;
			if (liveChild == null)
			{
				return "(none)";
			}
			StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
			stringBuilder.Append("[");
			stringBuilder.Append(liveChild.Name);
			stringBuilder.Append("]");
			string result = stringBuilder.ToString();
			CinemachineDebug.ReturnToPool(stringBuilder);
			return result;
		}
	}

	public ICinemachineCamera LiveChild { get; set; }

	public override CameraState State => m_State;

	public override Transform LookAt
	{
		get
		{
			return ResolveLookAt(m_LookAt);
		}
		set
		{
			m_LookAt = value;
		}
	}

	public override Transform Follow
	{
		get
		{
			return ResolveFollow(m_Follow);
		}
		set
		{
			m_Follow = value;
		}
	}

	private ICinemachineCamera TransitioningFrom { get; set; }

	public CinemachineVirtualCameraBase[] ChildCameras
	{
		get
		{
			UpdateListOfChildren();
			return m_ChildCameras;
		}
	}

	public bool IsBlending => mActiveBlend != null;

	public override bool IsLiveChild(ICinemachineCamera vcam)
	{
		if (vcam != LiveChild)
		{
			if (mActiveBlend != null)
			{
				if (vcam != mActiveBlend.CamA)
				{
					return vcam == mActiveBlend.CamB;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
		UpdateListOfChildren();
		CinemachineVirtualCameraBase[] childCameras = m_ChildCameras;
		for (int i = 0; i < childCameras.Length; i++)
		{
			childCameras[i].OnTargetObjectWarped(target, positionDelta);
		}
		base.OnTargetObjectWarped(target, positionDelta);
	}

	public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		mActivationTime = Time.time;
		mCurrentInstruction = -1;
		LiveChild = null;
		mActiveBlend = null;
		TransitioningFrom = fromCam;
		InternalUpdateCameraState(worldUp, deltaTime);
	}

	public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!base.PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateListOfChildren();
		AdvanceCurrentInstruction();
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = null;
		if (mCurrentInstruction >= 0 && mCurrentInstruction < m_Instructions.Length)
		{
			cinemachineVirtualCameraBase = m_Instructions[mCurrentInstruction].m_VirtualCamera;
		}
		if (cinemachineVirtualCameraBase != null)
		{
			if (!cinemachineVirtualCameraBase.gameObject.activeInHierarchy)
			{
				cinemachineVirtualCameraBase.gameObject.SetActive(value: true);
				cinemachineVirtualCameraBase.UpdateCameraState(worldUp, deltaTime);
			}
			ICinemachineCamera liveChild = LiveChild;
			LiveChild = cinemachineVirtualCameraBase;
			if (liveChild != LiveChild && LiveChild != null)
			{
				LiveChild.OnTransitionFromCamera(liveChild, worldUp, deltaTime);
				CinemachineCore.Instance.GenerateCameraActivationEvent(LiveChild, liveChild);
				if (liveChild != null && mCurrentInstruction > 0)
				{
					mActiveBlend = CreateBlend(liveChild, LiveChild, m_Instructions[mCurrentInstruction].m_Blend, mActiveBlend);
					if (mActiveBlend == null)
					{
						CinemachineCore.Instance.GenerateCameraCutEvent(LiveChild);
					}
				}
			}
		}
		if (mActiveBlend != null)
		{
			mActiveBlend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : mActiveBlend.Duration);
			if (mActiveBlend.IsComplete)
			{
				mActiveBlend = null;
			}
		}
		if (mActiveBlend != null)
		{
			mActiveBlend.UpdateCameraState(worldUp, deltaTime);
			m_State = mActiveBlend.State;
		}
		else if (LiveChild != null)
		{
			if (TransitioningFrom != null)
			{
				LiveChild.OnTransitionFromCamera(TransitioningFrom, worldUp, deltaTime);
			}
			m_State = LiveChild.State;
		}
		TransitioningFrom = null;
		InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref m_State, deltaTime);
		base.PreviousStateIsValid = true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		InvalidateListOfChildren();
		LiveChild = null;
		mActiveBlend = null;
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Combine(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		CinemachineDebug.OnGUIHandlers = (CinemachineDebug.OnGUIDelegate)Delegate.Remove(CinemachineDebug.OnGUIHandlers, new CinemachineDebug.OnGUIDelegate(OnGuiHandler));
	}

	private void OnTransformChildrenChanged()
	{
		InvalidateListOfChildren();
	}

	private void OnGuiHandler()
	{
		if (!m_ShowDebugText)
		{
			CinemachineDebug.ReleaseScreenPos(this);
			return;
		}
		StringBuilder stringBuilder = CinemachineDebug.SBFromPool();
		stringBuilder.Append(base.Name);
		stringBuilder.Append(": ");
		stringBuilder.Append(Description);
		string text = stringBuilder.ToString();
		GUI.Label(CinemachineDebug.GetScreenPos(this, text, GUI.skin.box), text, GUI.skin.box);
		CinemachineDebug.ReturnToPool(stringBuilder);
	}

	private void InvalidateListOfChildren()
	{
		m_ChildCameras = null;
		LiveChild = null;
	}

	private void UpdateListOfChildren()
	{
		if (m_ChildCameras != null)
		{
			return;
		}
		List<CinemachineVirtualCameraBase> list = new List<CinemachineVirtualCameraBase>();
		CinemachineVirtualCameraBase[] componentsInChildren = GetComponentsInChildren<CinemachineVirtualCameraBase>(includeInactive: true);
		foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in componentsInChildren)
		{
			if (cinemachineVirtualCameraBase.transform.parent == base.transform)
			{
				list.Add(cinemachineVirtualCameraBase);
			}
		}
		m_ChildCameras = list.ToArray();
		ValidateInstructions();
	}

	internal void ValidateInstructions()
	{
		if (m_Instructions == null)
		{
			m_Instructions = new Instruction[0];
		}
		for (int i = 0; i < m_Instructions.Length; i++)
		{
			if (m_Instructions[i].m_VirtualCamera != null && m_Instructions[i].m_VirtualCamera.transform.parent != base.transform)
			{
				m_Instructions[i].m_VirtualCamera = null;
			}
		}
		mActiveBlend = null;
	}

	private void AdvanceCurrentInstruction()
	{
		if (m_ChildCameras == null || m_ChildCameras.Length == 0 || mActivationTime < 0f || m_Instructions.Length == 0)
		{
			mActivationTime = -1f;
			mCurrentInstruction = -1;
			mActiveBlend = null;
			return;
		}
		if (mCurrentInstruction >= m_Instructions.Length - 1)
		{
			mCurrentInstruction = m_Instructions.Length - 1;
			return;
		}
		float time = Time.time;
		if (mCurrentInstruction < 0)
		{
			mActivationTime = time;
			mCurrentInstruction = 0;
		}
		else if (time - mActivationTime > Mathf.Max(0f, m_Instructions[mCurrentInstruction].m_Hold))
		{
			mActivationTime = time;
			mCurrentInstruction++;
		}
	}
}
