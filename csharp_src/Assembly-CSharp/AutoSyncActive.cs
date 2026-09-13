using UnityEngine;

public class AutoSyncActive : MonoBehaviour
{
	public GameObject ctrl;

	public GameObject target;

	private void Update()
	{
		if (ctrl != null && target != null && ctrl.activeSelf != target.activeSelf)
		{
			ctrl.SetActive(target.activeSelf);
		}
	}

	public void SetCtrlAndTarget(GameObject ctrl, GameObject target)
	{
		this.ctrl = ctrl;
		this.target = target;
	}
}
