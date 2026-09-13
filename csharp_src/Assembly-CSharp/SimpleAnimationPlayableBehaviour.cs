using UnityEngine;
using UnityEngine.Playables;

public class SimpleAnimationPlayableBehaviour : PlayableBehaviour
{
	public SimpleAnimation animation;

	public string state;

	public float speed = 1f;

	private AnimationClip clip;

	public void Initialize(SimpleAnimation animation, string state, float speed)
	{
		this.animation = animation;
		this.state = state;
		this.speed = speed;
		InitAnimationClip();
	}

	private void InitAnimationClip()
	{
		if (clip != null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.state))
		{
			this.state = "Default";
			clip = animation.clip;
			return;
		}
		SimpleAnimation.State state = animation.GetState(this.state);
		if (state != null)
		{
			clip = state.clip;
			return;
		}
		SimpleAnimation.EditorState editorStates = animation.GetEditorStates(this.state);
		if (editorStates != null)
		{
			clip = editorStates.clip;
		}
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (!(animation == null))
		{
			animation.Stop();
			animation.Play(state);
			SimpleAnimation.State obj = animation.GetState(state);
			obj.speed = speed;
			obj.time = (float)playable.GetTime();
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		animation.SetStateSpeed(state, 0f);
	}
}
