using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Unity.Collections;
using UnityEngine;

public class PVEScShotManager : MonoBehaviour
{
	private class QueuedFrame
	{
		public byte[] raw;

		public int width;

		public int height;

		public int frameIndex;

		public float time;
	}

	private int captureWidth = 180;

	private int captureHeight = 360;

	private RenderTextureFormat rtFormat = RenderTextureFormat.RGB565;

	private int maxInFlight = 4;

	private string outputFolder = "Screenshots";

	private string pPath;

	private string filePrefix = "frame_";

	private bool writeToDisk = true;

	private RenderTexture captureRT;

	private Texture2D _texture;

	private int frameCounter;

	private int inFlightCount;

	private ConcurrentQueue<byte[]> _pool;

	private ConcurrentQueue<QueuedFrame> encodeQueue = new ConcurrentQueue<QueuedFrame>();

	private Thread workerThread;

	private AutoResetEvent workerSignal = new AutoResetEvent(initialState: false);

	private volatile bool workerStop;

	private Material _blitMaterial;

	private Camera _camera;

	private bool _upload;

	private string _uid;

	public void Init(string uid, bool upload = false, string pref = null)
	{
		_uid = uid;
		if (!string.IsNullOrEmpty(pref))
		{
			filePrefix = pref;
		}
		_upload = upload;
		SetupRenderTexture();
		Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, outputFolder));
		if (!_upload)
		{
			workerThread = new Thread(WorkerLoop)
			{
				IsBackground = true,
				Name = "ScreenshotCompressor"
			};
			workerThread.Start();
		}
		pPath = Application.persistentDataPath;
		_pool = new ConcurrentQueue<byte[]>();
		Shader shader = Shader.Find("Hidden/BlitKeepAspect");
		_blitMaterial = ((shader == null) ? null : new Material(shader));
		float num = 1f;
		if (_blitMaterial != null)
		{
			float num2 = (float)Screen.width / (float)Screen.height;
			float num3 = (float)captureWidth / (float)captureHeight;
			num = ((!(num3 > num2)) ? (num2 / num3) : (num3 / num2));
			_blitMaterial.SetFloat("_Scale", num);
		}
		_camera = Camera.main;
	}

	private void OnDestroy()
	{
		workerStop = true;
		workerSignal.Set();
		workerThread?.Join();
		if (captureRT != null)
		{
			captureRT.Release();
			UnityEngine.Object.Destroy(captureRT);
		}
		if (_texture != null)
		{
			UnityEngine.Object.Destroy(_texture);
		}
		_pool = null;
	}

	private void SetupRenderTexture()
	{
		if (captureRT != null)
		{
			captureRT.Release();
			UnityEngine.Object.Destroy(captureRT);
		}
		captureRT = new RenderTexture(captureWidth, captureHeight, 0, rtFormat);
		captureRT.antiAliasing = 1;
		captureRT.useMipMap = false;
		captureRT.Create();
	}

	public void CaptureNextFrame()
	{
		StartCoroutine(CaptureCoroutine());
	}

	private byte[] GetBytes()
	{
		if (_pool.TryDequeue(out var result))
		{
			return result;
		}
		return new byte[captureWidth * captureHeight * 3];
	}

	private IEnumerator CaptureCoroutine()
	{
		if (inFlightCount >= maxInFlight)
		{
			yield break;
		}
		inFlightCount++;
		yield return new WaitForEndOfFrame();
		if (_camera != null)
		{
			_camera.targetTexture = captureRT;
			_camera.Render();
			_camera.targetTexture = null;
		}
		try
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = captureRT;
			if (_texture == null)
			{
				_texture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, mipChain: false);
			}
			_texture.ReadPixels(new Rect(0f, 0f, captureWidth, captureHeight), 0, 0);
			NativeArray<byte> rawTextureData = _texture.GetRawTextureData<byte>();
			RenderTexture.active = active;
			inFlightCount--;
			if (_upload)
			{
				int num = frameCounter++;
				string uuid = $"{filePrefix}{num:D06}.frame";
				using MemoryStream memoryStream = new MemoryStream();
				memoryStream.WriteByte(70);
				memoryStream.WriteByte(82);
				memoryStream.WriteByte(77);
				memoryStream.WriteByte(49);
				memoryStream.Write(BitConverter.GetBytes(num), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(captureWidth), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(captureHeight), 0, 4);
				memoryStream.Write(BitConverter.GetBytes(Time.time), 0, 4);
				byte[] bytes = GetBytes();
				rawTextureData.CopyTo(bytes);
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					using (DeflateStream deflateStream = new DeflateStream(memoryStream2, System.IO.Compression.CompressionLevel.Fastest, leaveOpen: true))
					{
						deflateStream.Write(bytes, 0, bytes.Length);
					}
					byte[] array = memoryStream2.ToArray();
					memoryStream.Write(BitConverter.GetBytes(array.Length), 0, 4);
					memoryStream.Write(array, 0, array.Length);
				}
				_pool.Enqueue(bytes);
				PVELogManager.Instance.UploadPVESurfingLog(_uid, uuid, memoryStream.ToArray(), null);
				yield break;
			}
			byte[] bytes2 = GetBytes();
			rawTextureData.CopyTo(bytes2);
			QueuedFrame item = new QueuedFrame
			{
				raw = bytes2,
				width = captureWidth,
				height = captureHeight,
				frameIndex = frameCounter++,
				time = Time.time
			};
			encodeQueue.Enqueue(item);
		}
		catch (Exception)
		{
		}
	}

	private void WorkerLoop()
	{
		while (!workerStop)
		{
			if (!encodeQueue.TryDequeue(out var result))
			{
				workerSignal.WaitOne(200);
				continue;
			}
			try
			{
				string path = $"{filePrefix}-{result.frameIndex:D06}.frame";
				if (_upload)
				{
					continue;
				}
				using FileStream output = new FileStream(Path.Combine(pPath, outputFolder, path), FileMode.Create, FileAccess.Write, FileShare.None);
				using BinaryWriter binaryWriter = new BinaryWriter(output);
				binaryWriter.Write(new byte[4] { 70, 82, 77, 49 });
				binaryWriter.Write(result.frameIndex);
				binaryWriter.Write(result.width);
				binaryWriter.Write(result.height);
				binaryWriter.Write(result.time);
				using MemoryStream memoryStream = new MemoryStream();
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, System.IO.Compression.CompressionLevel.Fastest, leaveOpen: true))
				{
					deflateStream.Write(result.raw, 0, result.raw.Length);
				}
				byte[] array = memoryStream.ToArray();
				binaryWriter.Write(array.Length);
				binaryWriter.Write(array);
			}
			catch (Exception arg)
			{
				Debug.LogError($"Worker error: {arg}");
			}
			finally
			{
				_pool.Enqueue(result.raw);
			}
		}
	}
}
