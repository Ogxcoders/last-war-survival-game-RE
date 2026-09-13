using DG.Tweening;
using UnityEngine;

public class DotweenFinish : MonoBehaviour
{
	private Vector3 localScale = Vector3.one;

	private DOTweenAnimation[] animations;

	private SpriteRenderer sp;

	private void Awake()
	{
		animations = GetComponents<DOTweenAnimation>();
		sp = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		localScale = base.transform.localScale;
	}

	public void OnComplete()
	{
		for (int i = 0; i < animations.Length; i++)
		{
			animations[i].DOPause();
		}
		base.transform.localScale = localScale;
		sp.color = new Color(1f, 1f, 1f, 1f);
		base.gameObject.SetActive(value: false);
	}
}
