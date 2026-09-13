using UnityEngine;
using UnityEngine.UI;

public class TimeLineButton : MonoBehaviour
{
	private Button btn;

	private Camera m_camera;

	private void Awake()
	{
		btn = GetComponent<Button>();
		m_camera = Camera.main;
		btn.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		InstanceRequest request = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/CityScene/GuideStartScene.prefab");
		request.completed += delegate
		{
			Debug.LogError("初始话bundle完成");
			GameObject gameObject = request.gameObject;
			gameObject.gameObject.SetActive(value: true);
			gameObject.transform.position = new Vector3(m_camera.transform.position.x, 0f, m_camera.transform.position.z);
			gameObject.transform.localScale = Vector3.one;
			m_camera.transform.LookAt(gameObject.transform);
		};
	}

	private void Update()
	{
	}
}
