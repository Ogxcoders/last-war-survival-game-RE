using UnityEngine;

public class DynamicFPSHighLocker : MonoBehaviour
{
	public static int count { get; private set; }

	private void OnEnable()
	{
		count++;
	}

	private void OnDisable()
	{
		count--;
	}
}
