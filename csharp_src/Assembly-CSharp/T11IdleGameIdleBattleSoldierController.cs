using DG.Tweening;
using UnityEngine;

public class T11IdleGameIdleBattleSoldierController : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation mainAnim;

	[SerializeField]
	private SimpleAnimation bubbleAnim;

	[SerializeField]
	private GameObject soldierRoot;

	[SerializeField]
	private SpriteMeshRenderer bubbleImg;

	public SimpleAnimation GetMainAnim()
	{
		return mainAnim;
	}

	public SimpleAnimation GetBubbleAnim()
	{
		return bubbleAnim;
	}

	public void ShowBubble()
	{
		if (!(bubbleAnim == null))
		{
			bubbleAnim.Play("EnterBubble");
		}
	}

	public void HideBubble(bool isImmediate = false)
	{
		if (!(bubbleAnim == null))
		{
			if (isImmediate)
			{
				bubbleAnim.SampleAnimationAtTime("HideBubble", 1f);
			}
			else
			{
				bubbleAnim.Play("HideBubble");
			}
		}
	}

	public void SetBubbleImage(string imgPath)
	{
		if (!(bubbleImg == null))
		{
			bubbleImg.LoadSprite(imgPath);
		}
	}

	public void SetSoldierForward(float yAngle, float duration = 0f)
	{
		if (!(soldierRoot == null))
		{
			DOTween.Kill(soldierRoot.transform);
			if (duration > 0f)
			{
				soldierRoot.transform.DOLocalRotate(new Vector3(0f, yAngle, 0f), duration).SetEase(Ease.Linear);
			}
			else
			{
				soldierRoot.transform.Set_localEulerAngles(0f, yAngle, 0f);
			}
		}
	}

	public void SetSoldierLookAt(Vector3 targetPos)
	{
		if (!(soldierRoot == null))
		{
			soldierRoot.transform.LookAt(targetPos);
		}
	}
}
