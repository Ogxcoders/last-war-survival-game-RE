using UnityEngine;

public class BuildPowerFactory : MonoBehaviour
{
	[SerializeField]
	private GameObject workerNode;

	[SerializeField]
	private GameObject effectNode;

	[SerializeField]
	private SimpleAnimation animRoot;

	public void SetWorkModeOn(bool on)
	{
		if (workerNode != null)
		{
			effectNode.SetActive(on);
		}
		if (animRoot != null)
		{
			animRoot.Rewind(on ? "work" : "Default");
			animRoot.Play(on ? "work" : "Default");
		}
	}

	public void SetWorkerStatus(int status)
	{
		if (workerNode != null)
		{
			workerNode.SetActive(status == 0 || status == 1);
		}
	}
}
