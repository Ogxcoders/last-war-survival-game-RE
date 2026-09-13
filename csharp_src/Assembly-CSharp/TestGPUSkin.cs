using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering;
using VEngine;

public class TestGPUSkin
{
	private const int TextureSize = 16;

	private Camera _camera;

	private MeshRenderer _renderer;

	private RenderTexture _rt;

	private static bool s_enterOnceFlag;

	private static TestGPUSkin s_instance;

	private static Asset s_testAsset;

	private static GameObject s_testAssetGo;

	public static void StartTest()
	{
		try
		{
			if (!s_enterOnceFlag)
			{
				s_enterOnceFlag = true;
				s_testAsset = GameEntry.Resource.LoadAsset("Assets/_Art_LastWar/GenGPUAnim/test_qiu_skin/TestGPUSkin.prefab", typeof(GameObject));
				s_testAssetGo = UnityEngine.Object.Instantiate(s_testAsset.asset as GameObject);
				s_testAssetGo.transform.position = new Vector3(0f, 500f, 0f);
				s_instance = new TestGPUSkin();
				s_instance._StartTest();
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}

	private void _StartTest()
	{
		_camera = s_testAssetGo.GetComponentInChildren<Camera>();
		_renderer = s_testAssetGo.GetComponentInChildren<MeshRenderer>();
		_rt = new RenderTexture(16, 16, 16, RenderTextureFormat.ARGB32, 0);
		_camera.targetTexture = _rt;
		GameEntry.Timer.RegisterTimer(4f, Test);
	}

	public void Test()
	{
		try
		{
			RenderTexture.active = _rt;
			Texture2D texture2D = new Texture2D(16, 16, TextureFormat.ARGB32, mipChain: false);
			new Rect(0f, 0f, 16f, 16f);
			texture2D.ReadPixels(new Rect(0f, 0f, 16f, 16f), 0, 0);
			texture2D.Apply();
			RenderTexture.active = null;
			Color[] pixels = texture2D.GetPixels();
			int num = 0;
			for (int i = 0; i < pixels.Length; i++)
			{
				int num2 = (int)(pixels[i].r * 255f);
				int num3 = (int)(pixels[i].g * 255f);
				int num4 = (int)(pixels[i].b * 255f);
				num += ((num2 == 255 && num3 == 0 && num4 == 0) ? 1 : 0);
			}
			bool flag = pixels.Length == 256 && num == 256;
			PostEventLog.TrackMap("GPU_SKIN_TEST", new Dictionary<string, object>
			{
				{ "pixel_pass", num },
				{ "pass", flag }
			});
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
		finally
		{
			if (s_testAssetGo != null)
			{
				UnityEngine.Object.Destroy(s_testAssetGo);
				s_testAssetGo = null;
				_camera = null;
				_renderer = null;
			}
			if (_rt != null)
			{
				UnityEngine.Object.Destroy(_rt);
				_rt = null;
			}
			s_testAsset?.Release();
			s_testAsset = null;
		}
	}

	private void OnBeginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
	{
		try
		{
			if (arg2 == _camera)
			{
				CommandBuffer commandBuffer = CommandBufferPool.Get("draw_test_qiu");
				commandBuffer.Clear();
				commandBuffer.DrawRenderer(_renderer, _renderer.sharedMaterial);
				arg1.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
				arg1.Submit();
			}
		}
		catch (Exception message)
		{
			Log.Error(message);
		}
	}
}
