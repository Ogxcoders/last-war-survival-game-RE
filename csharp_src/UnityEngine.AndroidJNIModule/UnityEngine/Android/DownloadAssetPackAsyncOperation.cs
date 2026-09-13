using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Android;

public class DownloadAssetPackAsyncOperation : CustomYieldInstruction
{
	private Dictionary<string, AndroidAssetPackInfo> m_AssetPackInfos;

	public override bool keepWaiting
	{
		get
		{
			lock (m_AssetPackInfos)
			{
				foreach (AndroidAssetPackInfo value in m_AssetPackInfos.Values)
				{
					if (value == null)
					{
						return true;
					}
					if (value.status != AndroidAssetPackStatus.Canceled && value.status != AndroidAssetPackStatus.Completed && value.status != AndroidAssetPackStatus.Failed && value.status != AndroidAssetPackStatus.Unknown)
					{
						return true;
					}
				}
				return false;
			}
		}
	}

	public bool isDone => !keepWaiting;

	public float progress
	{
		get
		{
			lock (m_AssetPackInfos)
			{
				float num = 0f;
				float num2 = 0f;
				foreach (AndroidAssetPackInfo value in m_AssetPackInfos.Values)
				{
					if (value != null)
					{
						if (value.status == AndroidAssetPackStatus.Canceled || value.status == AndroidAssetPackStatus.Completed || value.status == AndroidAssetPackStatus.Failed || value.status == AndroidAssetPackStatus.Unknown)
						{
							num += 1f;
							num2 += 1f;
						}
						else
						{
							num += (float)(value.bytesDownloaded / value.size);
							num2 += value.transferProgress;
						}
					}
				}
				return Mathf.Clamp((num * 0.8f + num2 * 0.2f) / (float)m_AssetPackInfos.Count, 0f, 1f);
			}
		}
	}

	public string[] downloadedAssetPacks
	{
		get
		{
			lock (m_AssetPackInfos)
			{
				List<string> list = new List<string>();
				foreach (AndroidAssetPackInfo value in m_AssetPackInfos.Values)
				{
					if (value != null && value.status == AndroidAssetPackStatus.Completed)
					{
						list.Add(value.name);
					}
				}
				return list.ToArray();
			}
		}
	}

	public string[] downloadFailedAssetPacks
	{
		get
		{
			lock (m_AssetPackInfos)
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, AndroidAssetPackInfo> assetPackInfo in m_AssetPackInfos)
				{
					AndroidAssetPackInfo value = assetPackInfo.Value;
					if (value == null)
					{
						list.Add(assetPackInfo.Key);
					}
					else if (value.status == AndroidAssetPackStatus.Canceled || value.status == AndroidAssetPackStatus.Failed || value.status == AndroidAssetPackStatus.Unknown)
					{
						list.Add(value.name);
					}
				}
				return list.ToArray();
			}
		}
	}

	internal DownloadAssetPackAsyncOperation(string[] assetPackNames)
	{
		m_AssetPackInfos = assetPackNames.ToDictionary((string name) => name, (string name) => (AndroidAssetPackInfo)null);
	}

	internal void OnUpdate(AndroidAssetPackInfo info)
	{
		lock (m_AssetPackInfos)
		{
			m_AssetPackInfos[info.name] = info;
		}
	}
}
