using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class TextureReferenceField : ObjectReferenceField
{
	[SerializeField]
	private RawImage referencePreview;

	protected override float HeightMultiplier => 2f;

	public override bool SupportsType(Type type)
	{
		if (!typeof(Texture).IsAssignableFrom(type))
		{
			return typeof(Sprite).IsAssignableFrom(type);
		}
		return true;
	}

	protected override void OnReferenceChanged(UnityEngine.Object reference)
	{
		base.OnReferenceChanged(reference);
		referenceNameText.gameObject.SetActive(!reference);
		Texture texture = reference.GetTexture();
		referencePreview.enabled = texture != null;
		referencePreview.texture = texture;
	}
}
