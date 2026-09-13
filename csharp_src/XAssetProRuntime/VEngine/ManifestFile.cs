using System.Collections.Generic;

namespace VEngine;

public class ManifestFile : Loadable
{
	public const string ManifestVersion = "manifest.version";

	public const string CompressPosfix = "_small";

	private static readonly List<ManifestFile> Unused = new List<ManifestFile>();

	protected ManifestVersionFile versionFile;

	public Manifest target { get; set; }

	protected string name { get; set; }

	protected override void OnLoad()
	{
		target = new Manifest
		{
			name = name,
			onReadAsset = Versions.OnReadAsset
		};
	}

	protected override void OnUnused()
	{
		Unused.Add(this);
	}

	public virtual void Override()
	{
	}

	public static ManifestFile Load(string name, bool builtin = false)
	{
		ManifestFile manifestFile = Versions.CreateManifest(name, builtin);
		manifestFile.SyncLoad();
		return manifestFile;
	}

	public static ManifestFile LoadAsync(string name, bool builtin = false)
	{
		ManifestFile manifestFile = Versions.CreateManifest(name, builtin);
		manifestFile.Load();
		return manifestFile;
	}

	internal static ManifestFile Create(string name, bool builtin)
	{
		if (builtin)
		{
			return new BuiltinManifestFile
			{
				name = name
			};
		}
		return new DownloadManifestFile
		{
			name = name
		};
	}

	public static void UpdateFiles()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			ManifestFile manifestFile = Unused[i];
			if (!Updater.busy)
			{
				if (manifestFile.isDone)
				{
					Unused.RemoveAt(i);
					i--;
					manifestFile.Unload();
				}
				continue;
			}
			break;
		}
	}

	public static string GetManifestVersion(string appVersion, string gm)
	{
		return "manifest_v" + appVersion + gm + ".version";
	}
}
