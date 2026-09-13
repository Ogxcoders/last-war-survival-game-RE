using UnityEngine;

namespace PVEBattleLogic.Effect;

public class EffectSpriteObject : EffectGameObject
{
	private const float FADING_TIME = 0.5f;

	private const float FADING_SPEED = 2f;

	private SpriteRenderer _spriteRenderer;

	private SpriteGPUMeshRenderer _spriteGPUMeshRenderer;

	private bool _fading;

	private Color _color;

	public EffectSpriteObject(string path)
		: base(path)
	{
	}

	internal override void OnLoaded()
	{
		base.OnLoaded();
		_spriteRenderer = _gameObject.GetComponentInChildren<SpriteRenderer>();
		if (_spriteRenderer != null)
		{
			_color = _spriteRenderer.color;
			return;
		}
		_spriteGPUMeshRenderer = _gameObject.GetComponentInChildren<SpriteGPUMeshRenderer>();
		_color = _spriteGPUMeshRenderer.color;
	}

	internal override void OnShow()
	{
		base.OnShow();
		_color.a = 1f;
		if (_spriteRenderer != null)
		{
			_spriteRenderer.color = _color;
		}
		if (_spriteGPUMeshRenderer != null)
		{
			_spriteGPUMeshRenderer.color = _color;
		}
		_fading = false;
	}

	public override void OnUpdate(float deltaTime)
	{
		if (!_isLoaded || _countDown <= 0f)
		{
			return;
		}
		_countDown -= deltaTime;
		if (_countDown <= 0f)
		{
			if (_fading)
			{
				EffectViewFacade.RemoveEffect(ObjId);
				return;
			}
			_fading = true;
			_countDown = 0.5f;
		}
		else if (_fading)
		{
			float num = deltaTime * 2f;
			_color.a -= num;
			if (_spriteRenderer != null)
			{
				_spriteRenderer.color = _color;
			}
			if (_spriteGPUMeshRenderer != null)
			{
				_spriteGPUMeshRenderer.color = _color;
			}
		}
	}
}
