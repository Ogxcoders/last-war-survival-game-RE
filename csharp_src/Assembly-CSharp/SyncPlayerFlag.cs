using UnityEngine;
using VEngine;

public class SyncPlayerFlag : MonoBehaviour
{
	private Material _material;

	private Asset _asset;

	private static readonly int _MainTex = Shader.PropertyToID("_MainTex");

	private void Awake()
	{
		Renderer component = base.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			_material = component.material;
		}
	}

	private void OnEnable()
	{
		XLuaManager lua = GameEntry.Lua;
		if (lua == null || GameEntry.Resource == null || _material == null)
		{
			return;
		}
		string text = lua.CallWithReturn<string>("CSharpCallLuaInterface.GetPlayerFlagPath");
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_asset = GameEntry.Resource.LoadAsset(text, typeof(Texture2D));
		if (_asset != null)
		{
			Texture2D texture2D = _asset.asset as Texture2D;
			if (!(texture2D == null))
			{
				_material.SetTexture(_MainTex, texture2D);
			}
		}
	}

	private void OnDisable()
	{
		if (_asset != null && GameEntry.Resource != null)
		{
			GameEntry.Resource.UnloadAsset(_asset);
			_asset = null;
		}
	}
}
