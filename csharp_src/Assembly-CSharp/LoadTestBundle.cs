using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using VEngine;

public class LoadTestBundle : MonoBehaviour
{
	private Button btn;

	private void Awake()
	{
		btn = GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		if ("12490896000001" == GameEntry.Data.Player.Uid)
		{
			Debug.LogError("track");
			Delay();
		}
	}

	private void Start()
	{
	}

	private void Delay()
	{
		Asset request = GameEntry.Resource.LoadAssetAsync("Assets/Main/Prefabs/CityScene/GuideStartScene.prefab", typeof(GameObject));
		Debug.LogError("go");
		Asset asset = request;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			Debug.LogError("初始话bundle完成");
			GameObject gameObject = UnityEngine.Object.Instantiate(request.asset as GameObject);
			gameObject.gameObject.SetActive(value: true);
			gameObject.transform.position = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.GetComponentInChildren<PlayableDirector>().Play();
			Camera.main.transform.LookAt(gameObject.transform);
		});
	}

	private void Update()
	{
	}
}
