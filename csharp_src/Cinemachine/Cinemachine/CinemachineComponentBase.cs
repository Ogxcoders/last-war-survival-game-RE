using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.API)]
public abstract class CinemachineComponentBase : MonoBehaviour
{
	protected const float Epsilon = 0.0001f;

	private CinemachineVirtualCameraBase m_vcamOwner;

	private Transform mCachedFollowTarget;

	private CinemachineVirtualCameraBase mCachedFollowTargetVcam;

	private CinemachineTargetGroup mCachedFollowTargetGroup;

	private Transform mCachedLookAtTarget;

	private CinemachineVirtualCameraBase mCachedLookAtTargetVcam;

	private CinemachineTargetGroup mCachedLookAtTargetGroup;

	public CinemachineVirtualCameraBase VirtualCamera
	{
		get
		{
			if (m_vcamOwner == null)
			{
				m_vcamOwner = GetComponent<CinemachineVirtualCameraBase>();
			}
			if (m_vcamOwner == null && base.transform.parent != null)
			{
				m_vcamOwner = base.transform.parent.GetComponent<CinemachineVirtualCameraBase>();
			}
			return m_vcamOwner;
		}
	}

	public Transform FollowTarget
	{
		get
		{
			CinemachineVirtualCameraBase virtualCamera = VirtualCamera;
			if (!(virtualCamera == null))
			{
				return virtualCamera.Follow;
			}
			return null;
		}
	}

	public Transform LookAtTarget
	{
		get
		{
			CinemachineVirtualCameraBase virtualCamera = VirtualCamera;
			if (!(virtualCamera == null))
			{
				return virtualCamera.LookAt;
			}
			return null;
		}
	}

	public CinemachineTargetGroup FollowTargetGroup
	{
		get
		{
			if (FollowTarget != mCachedFollowTarget)
			{
				UpdateFollowTargetCache();
			}
			return mCachedFollowTargetGroup;
		}
	}

	public Vector3 FollowTargetPosition
	{
		get
		{
			Transform followTarget = FollowTarget;
			if (followTarget != mCachedFollowTarget)
			{
				UpdateFollowTargetCache();
			}
			if (mCachedFollowTargetVcam != null)
			{
				return mCachedFollowTargetVcam.State.FinalPosition;
			}
			if (followTarget != null)
			{
				return followTarget.position;
			}
			return Vector3.zero;
		}
	}

	public Quaternion FollowTargetRotation
	{
		get
		{
			Transform followTarget = FollowTarget;
			if (followTarget != mCachedFollowTarget)
			{
				mCachedFollowTargetVcam = null;
				mCachedFollowTarget = followTarget;
				if (followTarget != null)
				{
					mCachedFollowTargetVcam = followTarget.GetComponent<CinemachineVirtualCameraBase>();
				}
			}
			if (mCachedFollowTargetVcam != null)
			{
				return mCachedFollowTargetVcam.State.FinalOrientation;
			}
			if (followTarget != null)
			{
				return followTarget.rotation;
			}
			return Quaternion.identity;
		}
	}

	public CinemachineTargetGroup LookAtTargetGroup
	{
		get
		{
			if (LookAtTarget != mCachedLookAtTarget)
			{
				UpdateLookAtTargetCache();
			}
			return mCachedLookAtTargetGroup;
		}
	}

	public Vector3 LookAtTargetPosition
	{
		get
		{
			Transform lookAtTarget = LookAtTarget;
			if (lookAtTarget != mCachedLookAtTarget)
			{
				UpdateLookAtTargetCache();
			}
			if (mCachedLookAtTargetVcam != null)
			{
				return mCachedLookAtTargetVcam.State.FinalPosition;
			}
			if (lookAtTarget != null)
			{
				return lookAtTarget.position;
			}
			return Vector3.zero;
		}
	}

	public Quaternion LookAtTargetRotation
	{
		get
		{
			Transform lookAtTarget = LookAtTarget;
			if (lookAtTarget != mCachedLookAtTarget)
			{
				UpdateLookAtTargetCache();
			}
			if (mCachedLookAtTargetVcam != null)
			{
				return mCachedLookAtTargetVcam.State.FinalOrientation;
			}
			if (lookAtTarget != null)
			{
				return lookAtTarget.rotation;
			}
			return Quaternion.identity;
		}
	}

	public CameraState VcamState
	{
		get
		{
			CinemachineVirtualCameraBase virtualCamera = VirtualCamera;
			if (!(virtualCamera == null))
			{
				return virtualCamera.State;
			}
			return CameraState.Default;
		}
	}

	public abstract bool IsValid { get; }

	public abstract CinemachineCore.Stage Stage { get; }

	private void UpdateFollowTargetCache()
	{
		mCachedFollowTargetVcam = null;
		mCachedFollowTargetGroup = null;
		mCachedFollowTarget = FollowTarget;
		if (mCachedFollowTarget != null)
		{
			mCachedFollowTargetVcam = mCachedFollowTarget.GetComponent<CinemachineVirtualCameraBase>();
			mCachedFollowTargetGroup = mCachedFollowTarget.GetComponent<CinemachineTargetGroup>();
		}
	}

	private void UpdateLookAtTargetCache()
	{
		mCachedLookAtTargetVcam = null;
		mCachedLookAtTargetGroup = null;
		mCachedLookAtTarget = LookAtTarget;
		if (mCachedLookAtTarget != null)
		{
			mCachedLookAtTargetVcam = mCachedLookAtTarget.GetComponent<CinemachineVirtualCameraBase>();
			mCachedLookAtTargetGroup = mCachedLookAtTarget.GetComponent<CinemachineTargetGroup>();
		}
	}

	public virtual void PrePipelineMutateCameraState(ref CameraState curState)
	{
	}

	public abstract void MutateCameraState(ref CameraState curState, float deltaTime);

	public virtual bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
	{
		return false;
	}

	public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
	{
	}
}
