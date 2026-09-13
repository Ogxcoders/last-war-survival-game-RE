using UnityEngine;

public class SimulateSwitchToBackground : MonoBehaviour
{
	private void sendApplicationPauseMessage(bool isPause)
	{
		Transform[] array = Object.FindObjectsOfType<Transform>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SendMessage("OnApplicationPause", isPause, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void sendApplicationFocusMessage(bool isFocus)
	{
		Transform[] array = Object.FindObjectsOfType<Transform>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SendMessage("OnApplicationFocus", isFocus, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void sendEnterBackgroundMessage()
	{
		sendApplicationPauseMessage(isPause: true);
		sendApplicationFocusMessage(isFocus: false);
		Time.timeScale = 0f;
	}

	public void sendEnterFoegroundMessage()
	{
		sendApplicationFocusMessage(isFocus: true);
		sendApplicationPauseMessage(isPause: false);
		ApplicationLaunch.Instance.DisconnectRetry();
		Time.timeScale = 1f;
	}
}
