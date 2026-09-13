using System;
using UnityEngine;
using VEngine;

public class MeteoriteCircleProgressBar : MonoBehaviour
{
	public enum ColorType
	{
		None,
		White,
		Green,
		Blue,
		Red
	}

	private readonly int KEY_Progress = Shader.PropertyToID("_Progress");

	private readonly int KEY_Angle = Shader.PropertyToID("_Angle");

	public MeshRenderer mrBackground;

	public MeshRenderer mrFill;

	private Material matFill;

	private Material matBackground;

	private Asset assetFillMaterial;

	private Asset assetBackground;

	public float angle;

	private float progress = -0.1f;

	private ColorType colorType;

	private MaterialPropertyBlock mpb;

	public float Progress
	{
		set
		{
			if (!Mathf.Approximately(progress, value))
			{
				progress = value;
				if (matFill != null)
				{
					MPB.SetFloat(KEY_Progress, progress);
					mrFill.SetPropertyBlock(mpb);
				}
			}
		}
	}

	private string PathBackgroundMat => colorType switch
	{
		ColorType.White => "Assets/Main/Material/MeteoriteBattle/CircleBgWhite.mat", 
		ColorType.Green => "Assets/Main/Material/MeteoriteBattle/CircleBgGreen.mat", 
		ColorType.Blue => "Assets/Main/Material/MeteoriteBattle/CircleBgBlue.mat", 
		ColorType.Red => "Assets/Main/Material/MeteoriteBattle/CircleBgRed.mat", 
		_ => string.Empty, 
	};

	private string PathFillMat => colorType switch
	{
		ColorType.White => "Assets/Main/Material/MeteoriteBattle/CircleFillWhite.mat", 
		ColorType.Green => "Assets/Main/Material/MeteoriteBattle/CircleFillGreen.mat", 
		ColorType.Blue => "Assets/Main/Material/MeteoriteBattle/CircleFillBlue.mat", 
		ColorType.Red => "Assets/Main/Material/MeteoriteBattle/CircleFillRed.mat", 
		_ => string.Empty, 
	};

	private MaterialPropertyBlock MPB
	{
		get
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
			}
			return mpb;
		}
	}

	public void SetColor(ColorType colorType)
	{
		if (colorType != this.colorType)
		{
			this.colorType = colorType;
			ResetMaterials();
		}
	}

	private void ResetMaterials()
	{
		assetBackground?.Release();
		assetFillMaterial?.Release();
		assetBackground = null;
		assetFillMaterial = null;
		string pathBackgroundMat = PathBackgroundMat;
		string pathFillMat = PathFillMat;
		if (!string.IsNullOrEmpty(pathBackgroundMat))
		{
			assetBackground = GameEntry.Resource.LoadAssetAsync(pathBackgroundMat, typeof(Material));
			Asset asset = assetBackground;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset request)
			{
				if (assetBackground != null)
				{
					if ((bool)request.asset)
					{
						matBackground = new Material(request.asset as Material);
						mrBackground.material = matBackground;
					}
					assetBackground.Release();
					assetBackground = null;
				}
			});
		}
		if (string.IsNullOrEmpty(pathFillMat))
		{
			return;
		}
		assetFillMaterial = GameEntry.Resource.LoadAssetAsync(pathFillMat, typeof(Material));
		Asset asset2 = assetFillMaterial;
		asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate(Asset request)
		{
			if (assetFillMaterial != null)
			{
				if ((bool)request.asset)
				{
					matFill = new Material(request.asset as Material);
					mrFill.material = matFill;
					MPB.SetFloat(KEY_Angle, angle);
					MPB.SetFloat(KEY_Progress, progress);
					mrFill.SetPropertyBlock(mpb);
				}
				assetFillMaterial.Release();
				assetFillMaterial = null;
			}
		});
	}

	public void Dispose()
	{
		assetBackground?.Release();
		assetFillMaterial?.Release();
		assetBackground = null;
		assetFillMaterial = null;
	}
}
