using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUIBullet : MonoBehaviour
{
	public GameObject Go_Trail;

	private void OnEnable()
	{
		Go_Trail.SetActive(value: true);
	}

	private void OnDisable()
	{
		Go_Trail.SetActive(value: false);
	}
}
