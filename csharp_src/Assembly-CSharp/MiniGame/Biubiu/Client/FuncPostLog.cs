using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Joker;

namespace MiniGame.Biubiu.Client;

public static class FuncPostLog
{
	private static void PostMiniGameEvent(Dictionary<string, object> dict)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, object> item in dict)
		{
			stringBuilder.Append($"[{item.Key}: {item.Value}], ");
		}
		Log.Info($"[BiuBiu]:PostMiniGameEvent {stringBuilder}");
		PostEventLog.TrackMap("MiniGameTrackEvent", dict);
	}

	public static void PostBattleResult(SharedRuntime shared, DataUIRenderMessage.UIResult result)
	{
		int serverId = shared.InitData.ServerId;
		if (serverId == 0)
		{
			PostBattleResultImpl(shared, result, -1.0);
			return;
		}
		string templateData = GameEntry.Lua.GetTemplateData("season_bullet_server", serverId, "Domain");
		int.TryParse(GameEntry.ConfigCache.GetTemplateData("season_bullet_server", serverId, "Port"), out var result2);
		FuncUdpLatency.MeasureLatencyAsync(templateData, result2, "Ping", 1, 1000).ContinueWith(delegate(Task<double> taskPing)
		{
			MainThreadDispatcher.Instance.Enqueue(delegate
			{
				PostBattleResultImpl(shared, result, taskPing.Result);
			});
		});
	}

	private static void PostBattleResultImpl(SharedRuntime shared, DataUIRenderMessage.UIResult result, double ping)
	{
		string value = string.Empty;
		if (shared.GameResult.Statistics.ExceptionCount > 0)
		{
			value = "Possibly cheating : bullet shoot point exception ";
		}
		float num = -1f;
		string value2 = string.Empty;
		if (shared.ResourceLoader != null && ((GameLoader)shared.ResourceLoader).LoaderEnv is GameBiubiuPlayerEnv gameBiubiuPlayerEnv)
		{
			num = gameBiubiuPlayerEnv.Player.EnterRoomTime;
			value2 = gameBiubiuPlayerEnv.Player.SessionGame;
		}
		PostMiniGameEvent(new Dictionary<string, object>
		{
			{
				"battle_type",
				shared.GameType.ToString()
			},
			{ "curnetping", ping },
			{
				"disconnect_retry_count",
				shared.GameResult.Statistics.Retry_Connect_Count
			},
			{ "error", value },
			{ "is_win", result.Win },
			{
				"build_level",
				shared.InitData.LevelPath
			},
			{ "total_time", result.BattleTimeMills },
			{
				"state",
				string.IsNullOrEmpty(value) ? 3 : 4
			},
			{ "start_time", num },
			{ "uuid", value2 }
		});
	}

	public static void PostPrepareTime(float waitTime, string gameSession = "")
	{
		PostMiniGameEvent(new Dictionary<string, object>
		{
			{ "state", 2 },
			{ "start_time", waitTime },
			{ "uuid", gameSession }
		});
	}
}
