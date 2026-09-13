using UnityEngine;

public class UISelfExplosion : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUIEx titleTxt;

	[SerializeField]
	private TextMeshProUGUIEx timeTxt;

	private float timeCount = 1f;

	private long expireTime;

	public void SetExpireTime(long expireTime)
	{
		this.expireTime = expireTime;
		titleTxt.text = GameEntry.Localization.GetString("season_mastery_tips_27");
		long serverTime = GameEntry.Timer.GetServerTime();
		long mstime = expireTime - serverTime;
		timeTxt.text = GameEntry.Timer.MillisecondsToStringWithoutHour(mstime, ":");
	}

	private void Update()
	{
		timeCount -= Time.deltaTime;
		if (timeCount < 0f)
		{
			timeCount += 1f;
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = expireTime - serverTime;
			timeTxt.text = GameEntry.Timer.MillisecondsToStringWithoutHour(num, ":");
			if (0 < num && num < 1000 && SceneManager.World != null && SceneManager.World is WorldScene worldScene)
			{
				worldScene.CreateVFX("Assets/Main/Prefabs/World/Saiji/Eff_s_S1_zibao.prefab", base.transform.position, 2f);
			}
		}
	}
}
