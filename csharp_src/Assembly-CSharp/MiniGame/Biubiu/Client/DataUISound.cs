using System;
using Joker;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public static class DataUISound
{
	public static Action<int, string> PlaySound;

	public static void PlayerSound(int soundID, string soundGroupName = "Effect")
	{
		PlaySound?.Invoke(soundID, soundGroupName);
		Log.Debug($"[BiuBiu] PlaySound {soundID}");
		if (GameEntry.Sound != null)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundID, "sound2");
			string[] array = templateData.Split(new char[1] { ';' });
			if (array.Length != 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				GameEntry.Sound.PlaySound(array[num], soundGroupName);
			}
			else
			{
				GameEntry.Sound.PlaySound(templateData, soundGroupName);
			}
		}
	}
}
