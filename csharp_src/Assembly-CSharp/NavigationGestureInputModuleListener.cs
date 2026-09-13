using UnityEngine;

public class NavigationGestureInputModuleListener : MonoBehaviour
{
	public NavigationGestureInputModule inputModule { get; private set; }

	private void Awake()
	{
		inputModule = new NavigationGestureInputModule();
		inputModule.Initialize(0, 0, (int)((float)Screen.height * 0.05f), (int)((float)Screen.height * 0.2f), 100, 50);
	}

	private void OnEnable()
	{
		inputModule?.SetEnable(enabled: true);
	}

	private void OnDisable()
	{
		inputModule?.SetEnable(enabled: false);
	}

	private void Update()
	{
		inputModule?.Update();
	}
}
