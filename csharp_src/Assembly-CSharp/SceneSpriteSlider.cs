using UnityEngine;

public class SceneSpriteSlider : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Vector4 _bounds = new Vector4(0.5f, 0.5f, 27f, 78f);

	[SerializeField]
	private float _angle;

	[SerializeField]
	private Sprite sprite;

	private MaterialPropertyBlock _propertyBlock;

	private void Awake()
	{
		_propertyBlock = new MaterialPropertyBlock();
		_propertyBlock.SetFloat("_Angle", _angle);
		_propertyBlock.SetVector("_Bounds", _bounds);
		_propertyBlock.SetFloat("_Progress", 0f);
		_propertyBlock.SetTexture("_MainTex", _renderer.sprite.texture);
		_renderer.SetPropertyBlock(_propertyBlock);
	}

	public void Init(long maxValue, long curValue)
	{
		float num = curValue.ToFloat() / (float)maxValue;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		if (_propertyBlock != null)
		{
			_propertyBlock.SetFloat("_Progress", num);
			_propertyBlock.SetTexture("_MainTex", _renderer.sprite.texture);
			_renderer.SetPropertyBlock(_propertyBlock);
		}
	}
}
