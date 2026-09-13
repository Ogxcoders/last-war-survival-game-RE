using UnityEngine;
using UnityEngine.Networking;

namespace VEngine;

internal class WebBundle : Bundle
{
	private AsyncOperation operation;

	private UnityWebRequest request;

	protected override void OnLoad()
	{
		request = UnityWebRequestAssetBundle.GetAssetBundle(base.pathOrURL);
		operation = request.SendWebRequest();
	}

	protected override void OnUpdate()
	{
		if (base.status == LoadableStatus.Loading && request != null && operation != null)
		{
			base.progress = operation.progress;
			if (!string.IsNullOrEmpty(request.error))
			{
				Finish(request.error);
				request.Dispose();
				request = null;
				operation = null;
			}
			else if (operation.isDone)
			{
				base.assetBundle = DownloadHandlerAssetBundle.GetContent(request);
				Finish((base.assetBundle == null) ? "assetBundle == null" : null);
				request.Dispose();
				request = null;
				operation = null;
			}
		}
	}
}
