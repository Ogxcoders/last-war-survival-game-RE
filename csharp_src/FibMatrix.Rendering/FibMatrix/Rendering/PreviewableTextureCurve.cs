using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[Serializable]
internal class PreviewableTextureCurve
{
	[SerializeField]
	private TextureCurve textureCurve;

	private Texture2D texture;

	public string shaderPropertyName { get; set; } = "not initialize";

	private void Update()
	{
		textureCurve.SetDirty();
		texture = textureCurve.GetTexture();
	}

	public PreviewableTextureCurve()
	{
		if (textureCurve == null)
		{
			textureCurve = new TextureCurve(new Keyframe[2]
			{
				new Keyframe(0f, 0f, 1f, 1f),
				new Keyframe(1f, 1f, 1f, 1f)
			}, 0f, loop: false, new Vector2(0f, 1f));
		}
	}

	public Texture2D GetTexture2D()
	{
		texture = textureCurve.GetTexture();
		return texture;
	}

	public void SetDirty()
	{
		textureCurve.SetDirty();
	}

	public void Release()
	{
		texture = null;
		textureCurve.Release();
	}
}
