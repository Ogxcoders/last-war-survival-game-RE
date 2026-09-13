using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCircleSlider : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Vector4 _bounds = new Vector4(-1f, -1f, 1f, 1f);

	[SerializeField]
	private float _angle = 90f;

	[SerializeField]
	private Sprite sprite;

	private long _startTime;

	private long _endTime;

	private MaterialPropertyBlock _propertyBlock;

	public int _waitRefresh = 20;

	private int _waitCount;

	private float _maxTime;

	private static Dictionary<Texture2D, Material> spriteMaterial = new Dictionary<Texture2D, Material>();

	private void Awake()
	{
		if (!spriteMaterial.TryGetValue(sprite.texture, out var value))
		{
			value = new Material(_renderer.material);
			value.SetTexture("_MainTex", sprite.texture);
			spriteMaterial.Add(sprite.texture, value);
		}
		_renderer.material = value;
		value.enableInstancing = true;
		_propertyBlock = new MaterialPropertyBlock();
		_propertyBlock.SetFloat("_Angle", _angle);
		_propertyBlock.SetVector("_Bounds", _bounds);
		_propertyBlock.SetVector("_Flip", new Vector4(1f, 1f, 1f, 1f));
		_propertyBlock.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
		_propertyBlock.SetFloat("_Progress", 0f);
		_propertyBlock.SetColor("_RendererColor", new Vector4(1f, 1f, 1f, 1f));
		_propertyBlock.SetVector("_MainTex_ST", new Vector4(1f, 1f, 0f, 0f));
		_renderer.SetPropertyBlock(_propertyBlock);
	}

	public void Init(long startTime, long endTime)
	{
		_startTime = startTime;
		_endTime = endTime;
		_waitCount = 0;
		_maxTime = (_endTime - _startTime).ToFloat();
		_waitRefresh = GetRefreshDuring();
		RefreshValue();
	}

	private void Update()
	{
		_waitCount++;
		if (_waitCount > _waitRefresh)
		{
			_waitCount = 0;
			RefreshValue();
		}
	}

	private void RefreshValue()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (_endTime > 0 && _endTime > serverTime)
		{
			float value = 1f - (_endTime - serverTime).ToFloat() / _maxTime;
			SetValue(value);
		}
	}

	public void SetValue(float value)
	{
		if (value < 0f)
		{
			value = 0f;
		}
		else if (value > 1f)
		{
			value = 1f;
		}
		_propertyBlock.SetFloat("_Progress", value);
		_renderer.SetPropertyBlock(_propertyBlock);
	}

	public SpriteRenderer GetSpriteRenderer()
	{
		return _renderer;
	}

	public void ClearData()
	{
		_endTime = 0L;
		_startTime = 0L;
		SetValue(0f);
	}

	private int GetRefreshDuring()
	{
		if (_maxTime < 60000f)
		{
			return 0;
		}
		if (_maxTime < 300000f)
		{
			return 10;
		}
		if (_maxTime < 600000f)
		{
			return 30;
		}
		return 60;
	}
}
