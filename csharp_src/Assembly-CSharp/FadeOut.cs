using DG.Tweening;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
	[SerializeField]
	public float delayTime = 0.5f;

	[SerializeField]
	public float duringTime = 0.5f;

	private MeshRenderer meshRender;

	private void Awake()
	{
		meshRender = GetComponentInChildren<MeshRenderer>();
	}

	private void OnDisable()
	{
		if (IsInvoking("DoFadeOut"))
		{
			CancelInvoke("DoFadeOut");
		}
		meshRender.material.DOKill();
		meshRender.material.DOFade(1f, 0f);
	}

	private void OnEnable()
	{
		Invoke("DoFadeOut", delayTime);
	}

	private void DoFadeOut()
	{
		meshRender.material.DOFade(0f, duringTime);
	}
}
