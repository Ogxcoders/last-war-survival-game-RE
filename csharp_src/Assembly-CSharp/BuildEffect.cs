using UnityEngine;

public class BuildEffect : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation[] listAnimation;

	[SerializeField]
	private Animator[] listAnimator;

	[SerializeField]
	private ParticleSystem[] listParticle;

	[SerializeField]
	private GameObject[] listEffect;

	public void PlayAnim()
	{
		if (listAnimator != null)
		{
			for (int i = 0; i < listAnimator.Length; i++)
			{
				if (listAnimator[i] != null)
				{
					listAnimator[i].enabled = true;
				}
			}
		}
		if (listAnimation != null)
		{
			for (int j = 0; j < listAnimation.Length; j++)
			{
				if (listAnimation[j] != null)
				{
					listAnimation[j].enabled = true;
				}
			}
		}
		if (listParticle != null)
		{
			for (int k = 0; k < listParticle.Length; k++)
			{
				if (listParticle[k] != null)
				{
					listParticle[k].gameObject.SetActive(value: true);
				}
			}
		}
		if (listEffect == null)
		{
			return;
		}
		for (int l = 0; l < listEffect.Length; l++)
		{
			if (listEffect[l] != null)
			{
				listEffect[l].SetActive(value: true);
			}
		}
	}

	public void StopAnim()
	{
		if (listAnimation != null)
		{
			for (int i = 0; i < listAnimation.Length; i++)
			{
				if (listAnimation[i] != null)
				{
					listAnimation[i].enabled = false;
				}
			}
		}
		if (listAnimator != null)
		{
			for (int j = 0; j < listAnimator.Length; j++)
			{
				if (listAnimator[j] != null)
				{
					listAnimator[j].enabled = false;
				}
			}
		}
		if (listParticle != null)
		{
			for (int k = 0; k < listParticle.Length; k++)
			{
				if (listParticle[k] != null)
				{
					listParticle[k].gameObject.SetActive(value: false);
				}
			}
		}
		if (listEffect == null)
		{
			return;
		}
		for (int l = 0; l < listEffect.Length; l++)
		{
			if (listEffect[l] != null)
			{
				listEffect[l].SetActive(value: false);
			}
		}
	}
}
