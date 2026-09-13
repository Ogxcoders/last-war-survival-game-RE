using UnityEngine;
using UnityEngine.UI;

public class UIImageLightAnim : MonoBehaviour
{
	private float _animAllTime;

	private float _showAnimTime;

	private float _hideAnimTime;

	private float _time;

	private bool _isDoAnim;

	private Material _material;

	private const string PlayAnimName = "_Color_time";

	private void Awake()
	{
		_isDoAnim = false;
		_animAllTime = 0f;
	}

	private void Start()
	{
		Image component = GetComponent<Image>();
		if (component != null)
		{
			_material = component.material;
		}
		SetAnim(isShow: false);
	}

	public void Init(float allTime, float startTime, float endTime)
	{
		Image component = GetComponent<Image>();
		if (component != null)
		{
			_material = component.material;
		}
		_animAllTime = allTime;
		_showAnimTime = startTime;
		_hideAnimTime = endTime;
	}

	private void Update()
	{
		if (_animAllTime > 0f)
		{
			_time += Time.deltaTime;
			if (_time >= _animAllTime)
			{
				_time -= _animAllTime;
			}
			if (_time > _showAnimTime && !_isDoAnim)
			{
				SetAnim(isShow: true);
			}
			if (_time > _hideAnimTime && _isDoAnim)
			{
				SetAnim(isShow: false);
			}
		}
	}

	private void SetAnim(bool isShow)
	{
		_isDoAnim = isShow;
		if (_material != null)
		{
			float value = (isShow ? 1f : 0f);
			_material.SetFloat("_Color_time", value);
		}
	}
}
