using DG.Tweening;
using UnityEngine;

public class BountyHunterHunterItemAimController : MonoBehaviour
{
	public Transform gun1End;

	public Transform gun1Start;

	public Transform gun2End;

	public Transform gun2Start;

	public Transform rotateRoot;

	private Vector3 gunDir;

	private Vector3 aimDir;

	private Vector3 targetPos;

	private Quaternion targetRotation;

	private float elapsedTime;

	public Tween DoRotate(Vector3 tar, float duration, bool isUseGun2 = false)
	{
		Transform gunStart = (isUseGun2 ? gun2Start : gun1Start);
		Transform gunEnd = (isUseGun2 ? gun2End : gun1End);
		if (gunStart == null || gunEnd == null || rotateRoot == null)
		{
			return null;
		}
		targetPos = tar;
		elapsedTime = 0f;
		return DOTween.To(() => elapsedTime, delegate(float x)
		{
			elapsedTime = x;
		}, 1f, duration).OnUpdate(delegate
		{
			if (!(gunStart == null) && !(gunEnd == null) && !(rotateRoot == null))
			{
				gunDir = gunStart.position - gunEnd.position;
				aimDir = targetPos - gunEnd.position;
				targetRotation = Quaternion.FromToRotation(gunDir, aimDir) * rotateRoot.rotation;
				rotateRoot.rotation = Quaternion.Slerp(rotateRoot.rotation, targetRotation, elapsedTime);
			}
		}).SetEase(Ease.OutSine);
	}

	public void ResetRotation()
	{
		if (!(rotateRoot == null))
		{
			rotateRoot.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

	public void ResetMagicaPhysics()
	{
	}
}
