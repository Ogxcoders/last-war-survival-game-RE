using TMPro;
using UnityEngine;

namespace RTLTMPro;

[ExecuteInEditMode]
public class RTLTextMeshPro3D : TextMeshPro
{
	[SerializeField]
	protected bool preserveNumbers;

	[SerializeField]
	protected bool farsi = true;

	[SerializeField]
	[TextArea(3, 10)]
	protected string originalText;

	[SerializeField]
	protected bool fixTags = true;

	[SerializeField]
	protected bool forceFix;

	protected readonly FastStringBuilder finalText = new FastStringBuilder(2048);

	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			if (!(originalText == value))
			{
				originalText = value;
				UpdateText();
			}
		}
	}

	public string OriginalText => originalText;

	public bool PreserveNumbers
	{
		get
		{
			return preserveNumbers;
		}
		set
		{
			if (preserveNumbers != value)
			{
				preserveNumbers = value;
				base.havePropertiesChanged = true;
			}
		}
	}

	public bool Farsi
	{
		get
		{
			return farsi;
		}
		set
		{
			if (farsi != value)
			{
				farsi = value;
				base.havePropertiesChanged = true;
			}
		}
	}

	public bool FixTags
	{
		get
		{
			return fixTags;
		}
		set
		{
			if (fixTags != value)
			{
				fixTags = value;
				base.havePropertiesChanged = true;
			}
		}
	}

	protected bool ForceFix
	{
		get
		{
			return forceFix;
		}
		set
		{
			if (forceFix != value)
			{
				forceFix = value;
				base.havePropertiesChanged = true;
			}
		}
	}

	protected void Update()
	{
		if (base.havePropertiesChanged)
		{
			UpdateText();
		}
	}

	public void UpdateText()
	{
		if (originalText == null)
		{
			originalText = "";
		}
		if (!ForceFix && !TextUtils.IsRTLInput(originalText))
		{
			base.isRightToLeftText = false;
			base.text = originalText;
		}
		else
		{
			base.isRightToLeftText = true;
			base.text = GetFixedText(originalText);
		}
		base.havePropertiesChanged = true;
	}

	private string GetFixedText(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		finalText.Clear();
		RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
		finalText.Reverse();
		return finalText.ToString();
	}
}
