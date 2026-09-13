using UnityEngine;
using UnityEngine.UI;
using VEngine;

public class BaseSubLoadingComponent : MonoBehaviour, ISubLoadingComponent
{
	public RawImage background;

	public RawImage logoImage;

	public Slider progressBar;

	public GameObject progressContainer;

	public TextMeshProUGUIEx loadingText;

	public TextMeshProUGUIEx versionText;

	public TextMeshProUGUIEx downloadText;

	protected string _defaultBgPath = "Assets/Main/SingleSprites/cfm_loading_tu.png";

	protected string _defaultLogoPath = "Assets/Main/Loading/cfm_loading_logo.png";

	private Asset _bgAsset;

	private Asset _iconAsset;

	public TextMeshProUGUIEx LoadingText => loadingText;

	public TextMeshProUGUIEx VersionText => versionText;

	public TextMeshProUGUIEx DownloadText => downloadText;

	public virtual void CSOpen()
	{
	}

	public virtual void SetBg()
	{
	}

	public virtual void SetIcon()
	{
	}

	public virtual void SetSlider()
	{
	}

	public virtual void SetProgressBar(float value)
	{
		if ((bool)progressContainer)
		{
			progressContainer.SetActive(value > 0f);
		}
		progressBar.value = value;
	}

	protected void SetImage(string imagePath, MaskableGraphic comp, bool isSetNativeSize = false, bool isBg = true)
	{
		(isBg ? _bgAsset : _iconAsset)?.Release();
		Asset asset = null;
		if (comp is Image)
		{
			asset = GameEntry.Resource.LoadAsset(imagePath, typeof(Sprite));
			if (asset != null && !asset.isError)
			{
				AfterLoadImage(asset.asset, comp, isSetNativeSize);
			}
		}
		else if (comp is RawImage)
		{
			asset = GameEntry.Resource.LoadAsset(imagePath, typeof(Texture));
			if (asset != null && !asset.isError)
			{
				AfterLoadImage(asset.asset, comp, isSetNativeSize);
			}
		}
		if (isBg)
		{
			_bgAsset = asset;
		}
		else
		{
			_iconAsset = asset;
		}
	}

	private void AfterLoadImage(Object asset, MaskableGraphic comp, bool isSetNativeSize)
	{
		if (comp is Image image)
		{
			Sprite sprite = asset as Sprite;
			if (sprite != null)
			{
				image.sprite = sprite;
			}
			if (isSetNativeSize)
			{
				image.SetNativeSize();
			}
		}
		else if (comp is RawImage rawImage)
		{
			Texture texture = asset as Texture;
			if (texture != null)
			{
				rawImage.texture = texture;
			}
			if (isSetNativeSize)
			{
				rawImage.SetNativeSize();
			}
		}
	}

	public virtual void CSClose()
	{
		_bgAsset?.Release();
		_bgAsset = null;
		_iconAsset?.Release();
		_iconAsset = null;
	}

	public virtual void SetVersionText()
	{
		string text = $"{GameEntry.Sdk.Version}[{GameEntry.Sdk.VersionCode}]";
		string resVersion = GameEntry.Resource.GetResVersion();
		versionText.text = GameEntry.Localization.GetString("100050") + " " + text + "\n" + GameEntry.Localization.GetString("100051") + " " + resVersion;
	}
}
