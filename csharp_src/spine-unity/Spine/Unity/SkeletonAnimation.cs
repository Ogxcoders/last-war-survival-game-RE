using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[AddComponentMenu("Spine/SkeletonAnimation")]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonAnimation-Component")]
public class SkeletonAnimation : SkeletonRenderer, ISkeletonAnimation, ISpineComponent, IAnimationStateComponent
{
	public AnimationState state;

	private bool wasUpdatedAfterInit = true;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string _animationName;

	public bool loop;

	public float timeScale = 1f;

	public AnimationState AnimationState
	{
		get
		{
			Initialize(overwrite: false);
			return state;
		}
	}

	public string AnimationName
	{
		get
		{
			if (!valid)
			{
				return _animationName;
			}
			return state.GetCurrent(0)?.Animation.Name;
		}
		set
		{
			Initialize(overwrite: false);
			if (_animationName == value)
			{
				TrackEntry current = state.GetCurrent(0);
				if (current != null && current.Loop == loop)
				{
					return;
				}
			}
			_animationName = value;
			if (string.IsNullOrEmpty(value))
			{
				state.ClearTrack(0);
				return;
			}
			Animation animation = skeletonDataAsset.GetSkeletonData(quiet: false).FindAnimation(value);
			if (animation != null)
			{
				state.SetAnimation(0, animation, loop);
			}
		}
	}

	protected event UpdateBonesDelegate _BeforeApply;

	protected event UpdateBonesDelegate _UpdateLocal;

	protected event UpdateBonesDelegate _UpdateWorld;

	protected event UpdateBonesDelegate _UpdateComplete;

	public event UpdateBonesDelegate BeforeApply
	{
		add
		{
			_BeforeApply += value;
		}
		remove
		{
			_BeforeApply -= value;
		}
	}

	public event UpdateBonesDelegate UpdateLocal
	{
		add
		{
			_UpdateLocal += value;
		}
		remove
		{
			_UpdateLocal -= value;
		}
	}

	public event UpdateBonesDelegate UpdateWorld
	{
		add
		{
			_UpdateWorld += value;
		}
		remove
		{
			_UpdateWorld -= value;
		}
	}

	public event UpdateBonesDelegate UpdateComplete
	{
		add
		{
			_UpdateComplete += value;
		}
		remove
		{
			_UpdateComplete -= value;
		}
	}

	public static SkeletonAnimation AddToGameObject(GameObject gameObject, SkeletonDataAsset skeletonDataAsset, bool quiet = false)
	{
		return SkeletonRenderer.AddSpineComponent<SkeletonAnimation>(gameObject, skeletonDataAsset, quiet);
	}

	public static SkeletonAnimation NewSkeletonAnimationGameObject(SkeletonDataAsset skeletonDataAsset, bool quiet = false)
	{
		return SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(skeletonDataAsset, quiet);
	}

	public override void ClearState()
	{
		base.ClearState();
		if (state != null)
		{
			state.ClearTracks();
		}
	}

	public override void Initialize(bool overwrite, bool quiet = false)
	{
		if (valid && !overwrite)
		{
			return;
		}
		base.Initialize(overwrite, quiet);
		if (!valid)
		{
			return;
		}
		state = new AnimationState(skeletonDataAsset.GetAnimationStateData());
		wasUpdatedAfterInit = false;
		if (!string.IsNullOrEmpty(_animationName))
		{
			Animation animation = skeletonDataAsset.GetSkeletonData(quiet: false).FindAnimation(_animationName);
			if (animation != null)
			{
				state.SetAnimation(0, animation, loop);
			}
		}
	}

	private void Update()
	{
		Update(Time.deltaTime);
	}

	public void Update(float deltaTime)
	{
		if (!valid || state == null)
		{
			return;
		}
		wasUpdatedAfterInit = true;
		if (updateMode >= UpdateMode.OnlyAnimationStatus)
		{
			UpdateAnimationStatus(deltaTime);
			if (updateMode == UpdateMode.OnlyAnimationStatus)
			{
				state.ApplyEventTimelinesOnly(skeleton, issueEvents: false);
			}
			else
			{
				ApplyAnimation();
			}
		}
	}

	protected void UpdateAnimationStatus(float deltaTime)
	{
		deltaTime *= timeScale;
		skeleton.Update(deltaTime);
		state.Update(deltaTime);
	}

	protected void ApplyAnimation()
	{
		if (this._BeforeApply != null)
		{
			this._BeforeApply(this);
		}
		if (updateMode != UpdateMode.OnlyEventTimelines)
		{
			state.Apply(skeleton);
		}
		else
		{
			state.ApplyEventTimelinesOnly(skeleton);
		}
		if (this._UpdateLocal != null)
		{
			this._UpdateLocal(this);
		}
		skeleton.UpdateWorldTransform();
		if (this._UpdateWorld != null)
		{
			this._UpdateWorld(this);
			skeleton.UpdateWorldTransform();
		}
		if (this._UpdateComplete != null)
		{
			this._UpdateComplete(this);
		}
	}

	public override void LateUpdate()
	{
		if (!wasUpdatedAfterInit)
		{
			Update(0f);
		}
		base.LateUpdate();
	}

	public override void OnBecameVisible()
	{
		UpdateMode updateMode = base.updateMode;
		base.updateMode = UpdateMode.FullUpdate;
		if (updateMode != UpdateMode.FullUpdate && updateMode != UpdateMode.EverythingExceptMesh)
		{
			Update(0f);
		}
		if (updateMode != UpdateMode.FullUpdate)
		{
			LateUpdate();
		}
	}
}
