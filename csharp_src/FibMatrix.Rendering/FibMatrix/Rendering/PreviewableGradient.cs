using System.IO;
using UnityEngine;

namespace FibMatrix.Rendering;

public class PreviewableGradient : MonoBehaviour
{
	[SerializeField]
	private Gradient gradient = new Gradient
	{
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(Color.black, 0f),
			new GradientColorKey(Color.white, 1f)
		}
	};

	[SerializeField]
	public Vector2Int textureSize;

	[SerializeField]
	public string directory;

	[SerializeField]
	public string filename;

	[SerializeField]
	public Vector2Int textureSizePreview;

	public bool showAlpha;

	private Texture2D texture;

	private void Save()
	{
		if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(filename))
		{
			Debug.LogError("文件名不能为空");
			return;
		}
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		Texture2D tex = null;
		TextureUtilities.GradientToTexture2D(gradient, ref tex, textureSize, TextureFormat.ARGB32);
		TextureUtilities.SaveTexture(Path.Combine(directory, filename), tex);
		tex = null;
	}

	private void Start()
	{
		OnValueChanged();
	}

	private void OnValueChanged()
	{
		TextureUtilities.GradientToTexture2D(gradient, ref texture, textureSizePreview, showAlpha ? TextureFormat.ARGB32 : TextureFormat.RGB24);
	}

	private void OnDestroy()
	{
		texture = null;
	}
}
