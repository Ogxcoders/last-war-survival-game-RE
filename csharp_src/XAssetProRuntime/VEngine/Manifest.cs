using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework;

namespace VEngine;

public class Manifest
{
	public bool thread;

	private const string key_version = "[Version]";

	private const string key_app_version = "[AppVersion]";

	private const string key_groups = "[Groups]";

	private const string key_paths = "[Paths]";

	private const string key_directories = "[Directories]";

	private const string key_bundles = "[Bundles]";

	private const string key_assets = "[Assets]";

	private static readonly HashSet<string> all_keys = new HashSet<string> { "[Version]", "[AppVersion]", "[Groups]", "[Paths]", "[Directories]", "[Bundles]", "[Assets]" };

	public Action<string> onReadAsset;

	internal readonly List<string> allAssetPaths = new List<string>(75000);

	private readonly List<string> directories = new List<string>(1500);

	private readonly Dictionary<string, BundleInfo> nameWithBundles = new Dictionary<string, BundleInfo>(2000);

	private readonly Dictionary<string, BundleInfo> aliasWithBundles = new Dictionary<string, BundleInfo>(2000);

	private readonly Dictionary<string, GroupInfo> nameWithGroups = new Dictionary<string, GroupInfo>();

	private readonly Dictionary<string, List<GroupInfo>> bundleName2Groups = new Dictionary<string, List<GroupInfo>>(2000);

	internal readonly Dictionary<string, AssetInfo> pathWithAssets = new Dictionary<string, AssetInfo>(65000);

	public List<AssetInfo> assets = new List<AssetInfo>(75000);

	public List<BundleInfo> bundles = new List<BundleInfo>(2000);

	public List<GroupInfo> groups = new List<GroupInfo>();

	internal readonly Dictionary<int, List<BundleInfo>> packageInfosV2 = new Dictionary<int, List<BundleInfo>>(64);

	public int version;

	public string appVersion;

	private StringBuilder sb;

	internal const bool k_FastParse = true;

	internal const bool k_StripAssetKeyInFile = true;

	private List<BundleInfo> _emptyPackageInfos = new List<BundleInfo>();

	public string name { get; set; }

	public int id { get; set; }

	public string[] AllAssetPaths => allAssetPaths.ToArray();

	[Conditional("DEBUG")]
	private void BeginSample(string sampleName)
	{
		_ = thread;
	}

	[Conditional("DEBUG")]
	private void EndSample()
	{
		_ = thread;
	}

	public void Load(string path)
	{
		Log.Info("Manifest::Load path:" + path);
		pathWithAssets.Clear();
		nameWithBundles.Clear();
		aliasWithBundles.Clear();
		nameWithGroups.Clear();
		bundleName2Groups.Clear();
		allAssetPaths.Clear();
		assets.Clear();
		bundles.Clear();
		groups.Clear();
		directories.Clear();
		packageInfosV2.Clear();
		if (!SyncReader.ExistFile(path))
		{
			Log.Error(path + " is not found!");
			return;
		}
		if (path.StartsWith("jar:file://"))
		{
			using (MemoryStream stream = new MemoryStream(SyncReader.ReadAllBytesByFilePath(path)))
			{
				using StreamReader sr = new StreamReader(stream);
				ParseManifest(sr);
				return;
			}
		}
		using StreamReader sr2 = File.OpenText(path);
		ParseManifest(sr2);
	}

	public void ParseManifest(StreamReader sr)
	{
		string parseType = string.Empty;
		sb = new StringBuilder(256);
		ArrayPool<char> shared = ArrayPool<char>.Shared;
		char[] array = shared.Rent(81920);
		try
		{
			global::StringExtensions.StreamSplitEnumerator enumerator = sr.SplitLines(array).GetEnumerator();
			while (enumerator.MoveNext())
			{
				ReadOnlySpan<char> line = enumerator.Current;
				ParseManifestLine(line, ref parseType);
			}
		}
		finally
		{
			shared.Return(array);
		}
		sb = null;
		if (assets.Count != 0)
		{
			Log.Error("asset already parsed");
		}
		else
		{
			assets.Capacity = allAssetPaths.Count;
			for (int i = 0; i < bundles.Count; i++)
			{
				BundleInfo bundleInfo = bundles[i];
				for (int j = 0; j < bundleInfo.assets.Length; j++)
				{
					int assetId = bundleInfo.assets[j];
					ReadAsset(assetId, bundleInfo.id);
				}
			}
		}
		int k = 0;
		for (int count = groups.Count; k < count; k++)
		{
			GroupInfo groupInfo = groups[k];
			int[] array2 = groupInfo.bundles;
			foreach (int bundleId in array2)
			{
				BundleInfo bundle = GetBundle(bundleId);
				if (bundle != null)
				{
					if (bundleName2Groups.TryGetValue(bundle.name, out var value))
					{
						value.Add(groupInfo);
						continue;
					}
					value = new List<GroupInfo>();
					value.Add(groupInfo);
					bundleName2Groups.Add(bundle.name, value);
				}
			}
		}
	}

	private void ParseManifestLine(ReadOnlySpan<char> line, ref string parseType)
	{
		try
		{
			if (line.IsEmpty || line[0] == '#' || (line.Length >= 2 && line[0] == '/' && line[1] == '/'))
			{
				return;
			}
			if (line[0] == '[')
			{
				ReadOnlySpan<char> other = line.Trim();
				bool flag = false;
				foreach (string all_key in all_keys)
				{
					if (all_key.AsSpan().SequenceEqual(other))
					{
						parseType = all_key;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return;
				}
				Log.Error("ParseManifestLine unknown parseType " + other);
				parseType = string.Empty;
			}
			switch (parseType)
			{
			case "[Version]":
				ReadVersion(line);
				break;
			case "[AppVersion]":
				ReadAppVersion(line);
				break;
			case "[Paths]":
				ReadPath(line);
				break;
			case "[Bundles]":
				ReadBundle(line);
				break;
			case "[Directories]":
				ReadDirectory(line);
				break;
			case "[Groups]":
				ReadGroups(line);
				break;
			case "[Assets]":
				break;
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
			Log.Error("ParseManifestLine parser error!!!! " + parseType + ", " + line);
		}
	}

	private void ReadAppVersion(string line)
	{
		appVersion = line;
	}

	private void ReadAppVersion(ReadOnlySpan<char> line)
	{
		appVersion = line.ToString();
	}

	private void ReadVersion(string line)
	{
		version = line.IntValue();
	}

	private void ReadVersion(ReadOnlySpan<char> line)
	{
		version = line.ToInt();
	}

	private void ReadPath(string line)
	{
		string[] array = line.Split(new char[1] { ',' });
		int num = array[1].IntValue();
		string text = array[2];
		if (num >= 0 && num < directories.Count)
		{
			allAssetPaths.Add(directories[num] + "/" + text);
		}
		else
		{
			allAssetPaths.Add(text);
		}
	}

	private void ReadPath(ReadOnlySpan<char> line)
	{
		line.Split_to_spans(',', out var _, out var span2, out var span3);
		int num = span2.ToInt();
		if (num >= 0 && num < directories.Count)
		{
			sb.Clear();
			sb.Append(directories[num]);
			sb.Append("/");
			for (int i = 0; i < span3.Length; i++)
			{
				sb.Append(span3[i]);
			}
			allAssetPaths.Add(sb.ToString());
		}
		else
		{
			string item = span3.ToString();
			allAssetPaths.Add(item);
		}
	}

	private void ReadGroups(string line)
	{
		GroupInfo groupInfo = new GroupInfo();
		groupInfo.Deserialize(line);
		groups.Add(groupInfo);
		nameWithGroups.Add(groupInfo.name, groupInfo);
	}

	private void ReadGroups(ReadOnlySpan<char> line)
	{
		if (line[0] != '%')
		{
			GroupInfo groupInfo = new GroupInfo();
			groupInfo.Deserialize(line);
			groups.Add(groupInfo);
			nameWithGroups.Add(groupInfo.name, groupInfo);
		}
	}

	private void ReadDirectory(string line)
	{
		directories.Add(line.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
	}

	private void ReadDirectory(ReadOnlySpan<char> line)
	{
		line.Split_to_spanspan(',', out var _, out var span2);
		directories.Add(span2.ToString());
	}

	private void ReadBundle(string line)
	{
		BundleInfo bundleInfo = new BundleInfo();
		bundleInfo.Deserialize(line);
		nameWithBundles[bundleInfo.name] = bundleInfo;
		aliasWithBundles[bundleInfo.alias] = bundleInfo;
		bundles.Add(bundleInfo);
	}

	private void ReadBundle(ReadOnlySpan<char> line)
	{
		BundleInfo bundleInfo = new BundleInfo();
		bundleInfo.Deserialize(line);
		nameWithBundles[bundleInfo.name] = bundleInfo;
		aliasWithBundles[bundleInfo.alias] = bundleInfo;
		bundles.Add(bundleInfo);
		AddPackInfo(bundleInfo);
	}

	private void ReadAsset(string line)
	{
		AssetInfo assetInfo = new AssetInfo();
		assetInfo.Deserialize(line);
		assets.Add(assetInfo);
		string text = allAssetPaths[assetInfo.id];
		pathWithAssets[text] = assetInfo;
		if (onReadAsset != null)
		{
			onReadAsset(text);
		}
	}

	private void ReadAsset(ReadOnlySpan<char> line)
	{
		AssetInfo assetInfo = new AssetInfo();
		assetInfo.Deserialize(line);
		assets.Add(assetInfo);
		string text = allAssetPaths[assetInfo.id];
		pathWithAssets[text] = assetInfo;
		if (onReadAsset != null)
		{
			onReadAsset(text);
		}
	}

	private void ReadAsset(int assetId, int bundleId)
	{
		AssetInfo assetInfo = new AssetInfo();
		assetInfo.id = assetId;
		assetInfo.bundle = bundleId;
		assets.Add(assetInfo);
		string text = allAssetPaths[assetInfo.id];
		pathWithAssets[text] = assetInfo;
		if (onReadAsset != null)
		{
			onReadAsset(text);
		}
	}

	public void AddAsset(string path)
	{
		AssetInfo assetInfo = new AssetInfo
		{
			id = assets.Count
		};
		assets.Add(assetInfo);
		allAssetPaths.Add(path);
		pathWithAssets[path] = assetInfo;
		if (onReadAsset != null)
		{
			onReadAsset(path);
		}
	}

	public string Save(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		using (StreamWriter writer = new StreamWriter(File.OpenWrite(path)))
		{
			WriteVersion(writer);
			WriteDirectories(writer);
			WriteAssetPaths(writer);
			WriteBundles(writer);
			WriteGroups(writer);
		}
		return SaveVersion(path);
	}

	private void WriteAssetPaths(TextWriter writer)
	{
		directories.Clear();
		writer.WriteLine("[Paths]");
		foreach (string allAssetPath in allAssetPaths)
		{
			writer.WriteLine(allAssetPath);
		}
		writer.WriteLine();
	}

	private void WriteVersion(StreamWriter writer)
	{
		writer.WriteLine("[Version]");
		writer.WriteLine(version);
		writer.WriteLine();
	}

	private void WriteDirectories(StreamWriter writer)
	{
		writer.WriteLine("[Directories]");
		directories.Clear();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < allAssetPaths.Count; i++)
		{
			string path = allAssetPaths[i];
			string directoryName = Path.GetDirectoryName(path);
			string fileName = Path.GetFileName(path);
			int value = -1;
			if (string.IsNullOrEmpty(directoryName))
			{
				allAssetPaths[i] = $"{i},{value},{fileName}";
				continue;
			}
			directoryName = directoryName.Replace('\\', '/');
			if (!dictionary.TryGetValue(directoryName, out value))
			{
				value = directories.Count;
				dictionary.Add(directoryName, value);
				directories.Add(directoryName);
				writer.WriteLine($"{value},{directoryName}");
			}
			allAssetPaths[i] = $"{i},{value},{fileName}";
		}
		writer.WriteLine();
	}

	private void WriteAssets(StreamWriter writer)
	{
		writer.WriteLine("[Assets]");
		foreach (AssetInfo asset in assets)
		{
			writer.WriteLine(asset.Serialize());
		}
		writer.WriteLine();
	}

	private void WriteGroups(StreamWriter writer)
	{
		writer.WriteLine("[Groups]");
		foreach (GroupInfo group in groups)
		{
			writer.WriteLine(group.Serialize());
		}
	}

	private void WriteBundles(StreamWriter writer)
	{
		writer.WriteLine("[Bundles]");
		foreach (BundleInfo bundle in bundles)
		{
			writer.WriteLine(bundle.Serialize());
		}
		writer.WriteLine();
	}

	public static string GetVersionFile(string name)
	{
		return name + ".version";
	}

	private string SaveVersion(string path)
	{
		string fileName = Path.GetFileName(path);
		string directoryName = Path.GetDirectoryName(path);
		using FileStream fileStream = File.OpenRead(path);
		uint num = Utility.ComputeCRC32(fileStream);
		string path2 = directoryName + "/" + GetVersionFile(fileName);
		if (File.Exists(path2))
		{
			if (File.ReadAllText(path2).Split(new char[1] { ',' })[2].UIntValue().Equals(num))
			{
				Logger.I("Version not changed.");
			}
			File.Delete(path2);
		}
		string result = $"{fileName}_v{version}";
		string contents = $"{version},{fileStream.Length},{num}";
		File.WriteAllText(path2, contents);
		return result;
	}

	public BundleInfo GetBundle(string assetBundleName)
	{
		nameWithBundles.TryGetValue(assetBundleName, out var value);
		return value;
	}

	public BundleInfo GetBundleByAlias(string alias)
	{
		aliasWithBundles.TryGetValue(alias, out var value);
		return value;
	}

	public BundleInfo GetBundleWithoutHash(string assetBundleNameWithoutHash)
	{
		string text = assetBundleNameWithoutHash + "_";
		string pattern = "^" + Regex.Escape(text) + "[0-9a-f]{32}\\.bundle$";
		BundleInfo bundleInfo = null;
		Regex regex = new Regex(pattern, RegexOptions.Compiled);
		foreach (KeyValuePair<string, BundleInfo> nameWithBundle in nameWithBundles)
		{
			if (nameWithBundle.Key.StartsWith(text) && regex.IsMatch(nameWithBundle.Key))
			{
				if (bundleInfo != null)
				{
					Logger.E("GetBundleWithoutHash:" + assetBundleNameWithoutHash + " found more than one bundle.");
					return bundleInfo;
				}
				bundleInfo = nameWithBundle.Value;
			}
		}
		return bundleInfo;
	}

	public string GetBundleNameAppendHash(string nameWithoutHash)
	{
		BundleInfo bundleInfo = bundles.Find((BundleInfo b) => b.name.StartsWith(nameWithoutHash));
		if (bundleInfo != null)
		{
			return bundleInfo.name;
		}
		return string.Empty;
	}

	public bool ContainsBundle(string assetBundleName)
	{
		return nameWithBundles.ContainsKey(assetBundleName);
	}

	public bool ContainsBundleByAlias(string alias)
	{
		return aliasWithBundles.ContainsKey(alias);
	}

	public IEnumerable<BundleInfo> GetBundlesWithGroups(params string[] groupNames)
	{
		if (groupNames == null || groupNames.Length == 0)
		{
			return bundles.ToArray();
		}
		HashSet<BundleInfo> hashSet = new HashSet<BundleInfo>();
		foreach (string key in groupNames)
		{
			if (!nameWithGroups.TryGetValue(key, out var value))
			{
				continue;
			}
			int[] array = value.bundles;
			foreach (int bundleId in array)
			{
				BundleInfo bundle = GetBundle(bundleId);
				if (bundle != null)
				{
					hashSet.Add(bundle);
				}
			}
		}
		return hashSet;
	}

	public BundleInfo GetBundle(int bundleId)
	{
		if (bundleId >= 0 && bundleId < bundles.Count)
		{
			return bundles[bundleId];
		}
		return null;
	}

	public AssetInfo GetAsset(string path)
	{
		pathWithAssets.TryGetValue(path, out var value);
		return value;
	}

	public void SetAllAssetPaths(IEnumerable<string> assetPaths)
	{
		allAssetPaths.Clear();
		allAssetPaths.AddRange(assetPaths);
	}

	public override string ToString()
	{
		string text = $"Name: {name}, version: {version},  groups: ";
		if (text != null)
		{
			foreach (GroupInfo group in groups)
			{
				text = text + group.name + "; ";
			}
		}
		return text;
	}

	public BundleInfo[] GetBundles(AssetInfo info)
	{
		return Array.ConvertAll(bundles[info.bundle].deps, GetBundle);
	}

	public BundleInfo[] GetDependencies(BundleInfo info)
	{
		return Array.ConvertAll(info.deps, GetBundle);
	}

	public List<GroupInfo> GetGroupInfo(string bundleName)
	{
		if (bundleName2Groups.TryGetValue(bundleName, out var value))
		{
			return value;
		}
		return null;
	}

	private void AddPackInfo(BundleInfo bundleInfo)
	{
		int[] downloadModes = bundleInfo.downloadModes;
		foreach (int key in downloadModes)
		{
			if (packageInfosV2.TryGetValue(key, out var value))
			{
				value.Add(bundleInfo);
				continue;
			}
			value = new List<BundleInfo>(2048);
			value.Add(bundleInfo);
			packageInfosV2.Add(key, value);
		}
	}

	public IReadOnlyCollection<BundleInfo> GetBundlesWithPackageID(int packId)
	{
		if (packageInfosV2.TryGetValue(packId, out var value))
		{
			return value.AsReadOnly();
		}
		return _emptyPackageInfos.AsReadOnly();
	}

	public HashSet<string> GetDirectoriesHashSet()
	{
		return new HashSet<string>(directories);
	}
}
