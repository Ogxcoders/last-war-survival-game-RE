using System;
using System.Collections;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public abstract class DataUIEntityController : MonoBehaviour
{
	public Animator _animator;

	public bool _girl;

	protected virtual void Start()
	{
		_animator = GetComponentInChildren<Animator>();
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	private IEnumerator WaitToAction(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		callback?.Invoke();
	}

	public void PlayAnimation(string animName, int layer, float waitTime = 0f, Action callback = null)
	{
		_animator.Play(animName, layer, 0f);
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(layer);
		float time = ((waitTime <= 0f) ? currentAnimatorStateInfo.length : waitTime);
		StartCoroutine(WaitToAction(time, callback));
	}

	public virtual void Die()
	{
		if (_girl)
		{
			DataUISound.PlayerSound(5100012);
		}
		else
		{
			DataUISound.PlayerSound(5100011);
		}
	}

	public virtual void Reload(DataUIRenderLogic.RenderContext renderContext)
	{
	}

	public virtual void Fire(DataUIRenderMessage.UIEntityAction entityAction, DataUIRenderLogic.RenderContext renderContext)
	{
		DataUISound.PlayerSound(5100014);
	}

	public virtual void GameEnd(DataUIRenderMessage.UIEntityAction entityAction, DataUIRenderLogic.RenderContext renderContext)
	{
	}
}
