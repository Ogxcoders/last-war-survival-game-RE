using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUIPing : MonoBehaviour
{
	private string Domain;

	private int Port;

	private TextMeshProUGUIEx text;

	private float PingTime;

	private float PingInterval = 5f;

	private void Start()
	{
		text = GetComponent<TextMeshProUGUIEx>();
	}

	private void Update()
	{
		if (Domain != null)
		{
			if (PingTime < 0f)
			{
				DoPing();
			}
			PingTime -= Time.deltaTime;
		}
	}

	private void DoPing()
	{
		ToGetPingAsync().ContinueWith(delegate(Task<double> ping)
		{
			MainThreadDispatcher.Instance.Enqueue(delegate
			{
				if (text != null)
				{
					int num = Mathf.CeilToInt((float)ping.Result);
					text.text = $"Ping:{(float)num + Math.Min((float)num * 0.5f, 100f)}ms";
				}
			});
		});
	}

	private async Task<double> ToGetPingAsync()
	{
		PingTime = PingInterval;
		return await FuncUdpLatency.MeasureLatencyAsync(Domain, Port, "Ping", 1, 10000);
	}

	public void SetPingInfo(int serverId)
	{
		Domain = GameEntry.Lua.GetTemplateData("season_bullet_server", serverId, "Domain");
		int.TryParse(GameEntry.ConfigCache.GetTemplateData("season_bullet_server", serverId, "Port"), out Port);
	}

	private void OnDestroy()
	{
		Domain = null;
		Port = 0;
	}
}
