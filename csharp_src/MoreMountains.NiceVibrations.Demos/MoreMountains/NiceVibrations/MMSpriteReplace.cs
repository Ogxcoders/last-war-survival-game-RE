using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class MMSpriteReplace : MonoBehaviour
{
	[Header("Sprites")]
	public Sprite OnSprite;

	public Sprite OffSprite;

	[Header("Start settings")]
	public bool StartsOn = true;

	protected Image _image;

	protected SpriteRenderer _spriteRenderer;

	protected MMTouchButton _mmTouchButton;

	public bool CurrentValue => _image.sprite == OnSprite;

	protected virtual void Start()
	{
		Initialization();
	}

	protected virtual void Initialization()
	{
		_image = GetComponent<Image>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_mmTouchButton = GetComponent<MMTouchButton>();
		if (_mmTouchButton != null)
		{
			_mmTouchButton.ReturnToInitialSpriteAutomatically = false;
		}
		if (OnSprite == null || OffSprite == null)
		{
			return;
		}
		if (_image != null)
		{
			if (StartsOn)
			{
				_image.sprite = OnSprite;
			}
			else
			{
				_image.sprite = OffSprite;
			}
		}
		if (_spriteRenderer != null)
		{
			if (StartsOn)
			{
				_spriteRenderer.sprite = OnSprite;
			}
			else
			{
				_spriteRenderer.sprite = OffSprite;
			}
		}
	}

	public virtual void Swap()
	{
		if (_image != null)
		{
			if (_image.sprite != OnSprite)
			{
				SwitchToOnSprite();
			}
			else
			{
				SwitchToOffSprite();
			}
		}
		if (_spriteRenderer != null)
		{
			if (_spriteRenderer.sprite != OnSprite)
			{
				SwitchToOnSprite();
			}
			else
			{
				SwitchToOffSprite();
			}
		}
	}

	public virtual void SwitchToOffSprite()
	{
		if ((!(_image == null) || !(_spriteRenderer == null)) && !(OffSprite == null))
		{
			SpriteOff();
		}
	}

	protected virtual void SpriteOff()
	{
		if (_image != null)
		{
			_image.sprite = OffSprite;
		}
		if (_spriteRenderer != null)
		{
			_spriteRenderer.sprite = OffSprite;
		}
	}

	public virtual void SwitchToOnSprite()
	{
		if ((!(_image == null) || !(_spriteRenderer == null)) && !(OnSprite == null))
		{
			SpriteOn();
		}
	}

	protected virtual void SpriteOn()
	{
		if (_image != null)
		{
			_image.sprite = OnSprite;
		}
		if (_spriteRenderer != null)
		{
			_spriteRenderer.sprite = OnSprite;
		}
	}
}
