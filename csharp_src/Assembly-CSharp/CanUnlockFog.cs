using UnityEngine;

public class CanUnlockFog : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation _anim;

	public void Unlock()
	{
		_anim.Play("Unlock");
	}
}
