using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BrazilSubLoadingComponent : BaseSubLoadingComponent
{
	public Image runImage;

	public Sprite[] sprites;

	private const float FrameTime = 0.2f;

	private int _currentIndex;

	private Tween _loopTween;

	private void OnEnable()
	{
		if (sprites != null && sprites.Length != 0 && runImage != null)
		{
			_loopTween = DOVirtual.DelayedCall(0.2f, Loop).SetLoops(-1);
		}
	}

	private void OnDisable()
	{
		_loopTween?.Kill();
		_loopTween = null;
	}

	private void Loop()
	{
		runImage.sprite = sprites[_currentIndex];
		_currentIndex = (_currentIndex + 1) % sprites.Length;
	}
}
