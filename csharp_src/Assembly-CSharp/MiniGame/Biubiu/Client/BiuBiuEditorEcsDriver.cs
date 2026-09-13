using System;
using System.Collections;
using System.Collections.Generic;
using GameKit.Base;
using Joker;
using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class BiuBiuEditorEcsDriver : MonoBehaviour, EditorEcsDriver
{
	private class MultiPlayerRuntime : IDisposable
	{
		public GameObject Root;

		public GameBiubiuPlayerEnv Env;

		public GameBiuBiuRunTime RunTime;

		public void Prepare()
		{
			Env.Player.SendEnterRoom();
		}

		public bool IsPrepared()
		{
			return Env.Player.MultiPlayerState == EGameMultiPlayerState.Prepared;
		}

		public bool IsDone()
		{
			return RunTime.Loader.IsDone;
		}

		public void Run()
		{
			RunTime.Enter(EGameType.PvpClient, Env.Player.LevelPath, Env.Player.LevelJson);
		}

		public void Dispose()
		{
			RunTime.Dispose();
			if (Root != null)
			{
				UnityEngine.Object.Destroy(Root);
			}
			Singleton<WorldService>.Instance.DestroyWorld(Env.Player);
		}
	}

	private GameBiuBiuEditorRunTime BiuBiuEditorRunTime;

	public GameBiuBiuRunTime BiuBiuRunTime;

	public UIS5GameMain BindUI;

	public App.EMode Mode;

	public float PingJitterSimulate = 0.8f;

	public List<int> PingSimulate;

	private const string kLocalPlayer2 = "2人";

	private const string kLocalPlayer4 = "4人";

	private const string kRemovePlayer = "远程";

	private const string kP1 = "biubiu_p1";

	private const string kP2 = "biubiu_p2";

	private const string kP3 = "biubiu_p3";

	private const string kP4 = "biubiu_p4";

	private List<string> _modes = new List<string> { "远程", "2人", "4人" };

	private List<MultiPlayerRuntime> _MultiPlayerRunTimes = new List<MultiPlayerRuntime>();

	public bool Editor => BiuBiuEditorRunTime?.Loader != null;

	public GameWorld EditorWorld
	{
		get
		{
			object obj = BiuBiuEditorRunTime?.Loader?.World;
			if (obj == null)
			{
				GameBiuBiuRunTime biuBiuRunTime = BiuBiuRunTime;
				if ((object)biuBiuRunTime == null)
				{
					return null;
				}
				GameBiuBiuLoader loader = biuBiuRunTime.Loader;
				if (loader == null)
				{
					return null;
				}
				obj = loader.World;
			}
			return (GameWorld)obj;
		}
	}

	private void Start()
	{
		AppBiubiu.InitApp(Mode, PingSimulate, PingJitterSimulate);
		StartCoroutine(StartImpl());
		DataUISound.PlaySound = delegate(int soundID, string soundGroupName)
		{
			Log.Debug($"[BiuBiu] PlaySound {soundID}{soundGroupName}");
		};
	}

	private void Update()
	{
	}

	private void Stop()
	{
		if (BiuBiuEditorRunTime != null)
		{
			BiuBiuEditorRunTime.Dispose();
		}
		if (BiuBiuRunTime != null)
		{
			BiuBiuRunTime.Dispose();
		}
		foreach (MultiPlayerRuntime multiPlayerRunTime in _MultiPlayerRunTimes)
		{
			multiPlayerRunTime.Dispose();
		}
		_MultiPlayerRunTimes.Clear();
	}

	public void ToRun(string levelPath, string gameLevelJson = null)
	{
		Stop();
		BiuBiuRunTime.Enter(EGameType.PveClient, levelPath, gameLevelJson);
		BindUI.GameEnd.SetActive(value: false);
		BindUI.gameObject.SetActive(value: true);
	}

	public void ToRunMultiPlayer(string mode, string address, string room, string levelPath, string gameLevelJson = null)
	{
		StartCoroutine(RunMultiPlayerImpl(mode, address, room, levelPath, gameLevelJson));
	}

	private IEnumerator StartImpl()
	{
		while (GameBiubiuPlayerEditorSingleton.Instance == null)
		{
			yield return null;
		}
		GameBiubiuPlayerEnv loadEnv = new GameBiubiuPlayerEnv(GameBiubiuPlayerEditorSingleton.Instance, BindUI.GetGameRoot());
		BiuBiuEditorRunTime = base.gameObject.AddComponent<GameBiuBiuEditorRunTime>();
		BiuBiuEditorRunTime.BindRunEnv(BindUI, loadEnv);
		BiuBiuRunTime = base.gameObject.AddComponent<GameBiuBiuRunTime>();
		BiuBiuRunTime.BindRunEnv(BindUI, loadEnv);
		BiuBiuEditorRunTime.Config = false;
		BiuBiuEditorRunTime.Enter(EGameType.PveClient, "");
		BindUI.gameObject.SetActive(value: false);
	}

	private IEnumerator RunMultiPlayerImpl(string mode, string address, string room, string levelPath, string gameLevelJson)
	{
		Stop();
		yield return null;
		if (string.IsNullOrEmpty(room))
		{
			room = UnityEngine.Random.Range(10000, 999999).ToString();
		}
		switch (mode)
		{
		case "远程":
			yield return Create(Guid.NewGuid().ToString(), address, room, levelPath, 2, gameLevelJson, 0, 0);
			break;
		case "2人":
			yield return Create("biubiu_p1", address, room, levelPath, 2, gameLevelJson, -1, 1);
			yield return Create("biubiu_p2", address, room, levelPath, 2, gameLevelJson, 1, 1);
			break;
		case "4人":
			yield return Create("biubiu_p1", address, room, levelPath, 4, gameLevelJson, -1, 1);
			yield return Create("biubiu_p2", address, room, levelPath, 4, gameLevelJson, 1, 1);
			yield return Create("biubiu_p3", address, room, levelPath, 4, gameLevelJson, -1, -1);
			yield return Create("biubiu_p4", address, room, levelPath, 4, gameLevelJson, 1, -1);
			break;
		}
		bool prepared = false;
		while (!prepared)
		{
			prepared = true;
			for (int i = 0; i < _MultiPlayerRunTimes.Count; i++)
			{
				if (!_MultiPlayerRunTimes[i].IsPrepared())
				{
					prepared = false;
					break;
				}
			}
			yield return null;
		}
		for (int j = 0; j < _MultiPlayerRunTimes.Count; j++)
		{
			_MultiPlayerRunTimes[j].Run();
		}
		Camera camera = _MultiPlayerRunTimes[0].Env.Camera;
		camera.transform.position = Vector3.zero;
		for (int k = 1; k < _MultiPlayerRunTimes.Count; k++)
		{
			_MultiPlayerRunTimes[k].Env.Camera.gameObject.SetActive(value: false);
			_MultiPlayerRunTimes[k].Env.Camera = camera;
		}
	}

	public List<string> ListMultiPlayerRunMode()
	{
		return _modes;
	}

	public void ToEdit(string levelPath, string gameLevelJson = null)
	{
		Stop();
		BiuBiuEditorRunTime.Config = false;
		BiuBiuEditorRunTime.Enter(EGameType.PveClient, levelPath, gameLevelJson);
		BindUI.gameObject.SetActive(value: false);
	}

	public void ToConfig()
	{
		Stop();
		BiuBiuEditorRunTime.Config = true;
		BiuBiuEditorRunTime.Enter(EGameType.PveClient, "");
	}

	private IEnumerator Create(string name, string address, string room, string levelPath, int maxPlayers, string gameLevelJson, int offsetX, int offsetY)
	{
		Singleton<WorldService>.Instance.Create<GameBiubiuPlayerEditor>(ESchedulerType.Main, -1, name, new object[2] { address, room });
		GameBiubiuPlayerEditor player = null;
		while (player == null)
		{
			player = Singleton<WorldService>.Instance.GetWorld(name) as GameBiubiuPlayerEditor;
			yield return null;
		}
		while (!player.IsConnected)
		{
			yield return null;
		}
		player.SetupRoom(levelPath, gameLevelJson, string.Empty, string.Empty, maxPlayers);
		Transform transform = UnityEngine.Object.Instantiate(BindUI.GameRoot, BindUI.GameRoot.parent);
		Vector2 sizeDelta = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;
		UIS5GameMain componentInChildren = transform.GetComponentInChildren<UIS5GameMain>(includeInactive: true);
		GameBiubiuMultiRunTime orAddComponent = transform.GetOrAddComponent<GameBiubiuMultiRunTime>();
		GameBiubiuPlayerEnv gameBiubiuPlayerEnv = new GameBiubiuPlayerEnv(player, componentInChildren.GetGameRoot());
		orAddComponent.BindRunEnv(componentInChildren, gameBiubiuPlayerEnv);
		player.SetupRunTime(orAddComponent);
		if (name == "biubiu_p1")
		{
			player.PlayerID = EPlayerID.ID_1P;
		}
		else if (name == "biubiu_p2")
		{
			player.PlayerID = EPlayerID.ID_2P;
		}
		transform.name = player.PlayerSessionId;
		if (offsetX != 0 && offsetY != 0)
		{
			transform.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			transform.transform.localPosition = new Vector3(sizeDelta.x * (float)offsetX / 4f, sizeDelta.y * (float)offsetY / 4f, 0f);
		}
		componentInChildren.gameObject.SetActive(value: true);
		MultiPlayerRuntime multiPlayerRuntime = new MultiPlayerRuntime
		{
			Root = transform.gameObject,
			RunTime = orAddComponent,
			Env = gameBiubiuPlayerEnv
		};
		_MultiPlayerRunTimes.Add(multiPlayerRuntime);
		multiPlayerRuntime.Prepare();
	}
}
