using System.Collections;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUICameraShake : MonoBehaviour
{
	private Vector3 originalPosition;

	private bool isShaking;

	[Header("Shake Settings")]
	public float shakeDuration = 0.1f;

	public float shakeMagnitude = 0.5f;

	public float shakeMinMagnitude = 0.8f;

	private void Start()
	{
		originalPosition = base.transform.localPosition;
	}

	public void TriggerShake()
	{
		if (!isShaking)
		{
			originalPosition = base.transform.localPosition;
			StartCoroutine(Shake());
		}
	}

	private float CustomRandomRange(float min1, float max1, float min2, float max2)
	{
		if (Random.value < 0.5f)
		{
			return Random.Range(min1, max1);
		}
		return Random.Range(min2, max2);
	}

	private IEnumerator Shake()
	{
		isShaking = true;
		float elapsed = 0f;
		while (elapsed < shakeDuration)
		{
			float x = CustomRandomRange(0f - shakeMagnitude, 0f - shakeMinMagnitude, shakeMinMagnitude, shakeMagnitude);
			float y = CustomRandomRange(0f - shakeMagnitude, 0f - shakeMinMagnitude, shakeMinMagnitude, shakeMagnitude);
			base.transform.localPosition = originalPosition + new Vector3(x, y, 0f);
			elapsed += Time.deltaTime;
			yield return null;
		}
		base.transform.localPosition = originalPosition;
		isShaking = false;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
