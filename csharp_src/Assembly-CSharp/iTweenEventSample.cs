using UnityEngine;

public class iTweenEventSample : MonoBehaviour
{
	public void OnStartEvent()
	{
		Debug.Log("Tween Started!");
	}

	public void OnCompleteEvent()
	{
		Debug.Log("Tween Completed!");
	}
}
