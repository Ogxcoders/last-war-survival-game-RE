using UnityEngine;

public class CityResConfig : MonoBehaviour
{
	public int objId;

	public int resType = 21;

	public int maxBlood = 6;

	public int refreshCd = 10;

	public int buffId;

	public float outBuffRate;

	public float superRate;

	public float outSuperBuffRate;

	public float extraParaFloat;

	public string extraParaString;

	private void Start()
	{
		objId = base.gameObject.GetInstanceID();
	}
}
