using System;
using UnityEngine;

public class BaseRocketCenter : CityBuilding
{
	[SerializeField]
	private SimpleAnimation _rocketAnim;

	[SerializeField]
	private Transform _rocketEffectRoot;

	[SerializeField]
	private Transform _effectSmokeRoot;

	public bool isPlaying;

	private InstanceRequest _fireEffect;

	private InstanceRequest _smokeEffect;

	private InstanceRequest _fireEffectFly;

	private InstanceRequest _smokeEffectFly;

	private ITimer _time;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		if (IsSelf())
		{
			GameEntry.Event.Subscribe(EventId.GetNewEarthOrder, GetNewEarthOrderSignal);
			GameEntry.Event.Subscribe(EventId.EndEarthOrder, EndEarthOrderSignal);
			GameEntry.Event.Subscribe(EventId.ViewEndEarthOrder, ViewEndEarthOrderSignal);
			if (GameEntry.Lua.IsShowEarthOrder())
			{
				_rocketAnim.gameObject.SetActive(value: true);
			}
			else
			{
				_rocketAnim.gameObject.SetActive(value: false);
			}
		}
		else
		{
			_rocketAnim.gameObject.SetActive(value: true);
		}
	}

	protected internal override void CSUninit()
	{
		RemoveTimer();
		DestroyEffect();
		GameEntry.Event.Unsubscribe(EventId.GetNewEarthOrder, GetNewEarthOrderSignal);
		GameEntry.Event.Unsubscribe(EventId.EndEarthOrder, EndEarthOrderSignal);
		GameEntry.Event.Unsubscribe(EventId.ViewEndEarthOrder, ViewEndEarthOrderSignal);
		base.CSUninit();
	}

	private void GetNewEarthOrderSignal(object userData)
	{
		if (isPlaying)
		{
			return;
		}
		GameEntry.Sound.PlayEffect("effect_rocket_land");
		_rocketAnim.gameObject.SetActive(value: true);
		float num = UIUtils.PlayAnimationReturnTime(_rocketAnim, "WM_HJ_jiangluo");
		if (num > 0f)
		{
			isPlaying = true;
			LoadEffect();
			AddTimer(num, delegate
			{
				isPlaying = false;
				DestroyEffect();
				GameEntry.Event.Fire(EventId.RefreshEarthOrder);
			});
		}
	}

	private void EndEarthOrderSignal(object userData)
	{
		if (!GameEntry.Lua.UIManager.IsWindowOpen("UIEarthOrder") && !IsArriving())
		{
			GameEntry.Sound.PlayEffect("effect_rocket");
			float num = UIUtils.PlayAnimationReturnTime(_rocketAnim, "WM_HJ_shengkong");
			if (!(num > 0f))
			{
				return;
			}
			isPlaying = true;
			LoadEffectFly();
			AddTimer(num, delegate
			{
				isPlaying = false;
				DestroyEffect();
				if (GameEntry.Lua.IsShowEarthOrder())
				{
					GetNewEarthOrderSignal(null);
				}
				else
				{
					_rocketAnim.gameObject.SetActive(value: false);
				}
			});
		}
		else
		{
			isPlaying = true;
		}
	}

	private void ViewEndEarthOrderSignal(object userData)
	{
		float num = UIUtils.PlayAnimationReturnTime(_rocketAnim, "WM_HJ_shengkong");
		if (!(num > 0f))
		{
			return;
		}
		isPlaying = true;
		LoadEffectFly();
		AddTimer(num, delegate
		{
			isPlaying = false;
			DestroyEffect();
			if (GameEntry.Lua.IsShowEarthOrder())
			{
				GetNewEarthOrderSignal(null);
			}
			else
			{
				_rocketAnim.gameObject.SetActive(value: false);
			}
		});
	}

	public bool IsArriving()
	{
		return isPlaying;
	}

	private void LoadEffect()
	{
		DestroyEffect();
		_fireEffect = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/RocketEffect/SceneRocketFireEffect.prefab");
		_fireEffect.completed += delegate
		{
			if (!(_fireEffect.gameObject == null))
			{
				GameObject obj = _fireEffect.gameObject;
				obj.transform.SetParent(_rocketEffectRoot);
				obj.transform.localPosition = new Vector3(0f, 0f, -2f);
			}
		};
		_smokeEffect = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/RocketEffect/SceneRocketSmokeEffect.prefab");
		_smokeEffect.completed += delegate
		{
			if (!(_smokeEffect.gameObject == null))
			{
				GameObject obj = _smokeEffect.gameObject;
				obj.transform.SetParent(_effectSmokeRoot);
				obj.transform.localPosition = Vector3.zero;
			}
		};
	}

	private void LoadEffectFly()
	{
		DestroyEffect();
		_fireEffectFly = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/RocketEffect/SceneRocketFireEffect.prefab");
		_fireEffectFly.completed += delegate
		{
			if (!(_fireEffectFly.gameObject == null))
			{
				GameObject obj = _fireEffectFly.gameObject;
				obj.transform.SetParent(_rocketEffectRoot);
				obj.transform.localPosition = new Vector3(0f, 0f, -2f);
			}
		};
		_smokeEffectFly = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/RocketEffect/SceneRocketSmokeEffect.prefab");
		_smokeEffectFly.completed += delegate
		{
			if (!(_smokeEffectFly.gameObject == null))
			{
				GameObject obj = _smokeEffectFly.gameObject;
				obj.transform.SetParent(_effectSmokeRoot);
				obj.transform.localPosition = Vector3.zero;
			}
		};
	}

	private void DestroyEffect()
	{
		if (_fireEffect != null)
		{
			_fireEffect.Destroy();
			_fireEffect = null;
		}
		if (_smokeEffect != null)
		{
			_smokeEffect.Destroy();
			_smokeEffect = null;
		}
		if (_fireEffectFly != null)
		{
			_fireEffectFly.Destroy();
			_fireEffectFly = null;
		}
		if (_smokeEffectFly != null)
		{
			_smokeEffectFly.Destroy();
			_smokeEffectFly = null;
		}
	}

	private void AddTimer(float time, Action callBack)
	{
		RemoveTimer();
		_time = GameEntry.Timer.RegisterTimer(time, callBack);
	}

	private void RemoveTimer()
	{
		if (_time != null)
		{
			GameEntry.Timer.CancelTimer(_time);
			_time = null;
		}
	}
}
