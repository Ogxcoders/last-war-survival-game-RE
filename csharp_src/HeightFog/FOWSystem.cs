using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class FOWSystem : MonoBehaviour
{
	public enum LOSChecks
	{
		None,
		OnlyOnce,
		EveryUpdate
	}

	public class Revealer
	{
		public bool isActive;

		public LOSChecks los;

		public Vector3 pos = Vector3.zero;

		public float inner;

		public float outer;

		public bool[] cachedBuffer;

		public int cachedSize;

		public int cachedX;

		public int cachedY;
	}

	public enum State
	{
		Blending,
		NeedUpdate,
		UpdateTexture0,
		UpdateTexture1
	}

	public static float screenScale;

	public static FOWSystem instance;

	protected static int[,] mHeights;

	protected Transform mTrans;

	protected Vector3 mOrigin = Vector3.zero;

	protected Vector3 mSize = Vector3.one;

	public static List<Revealer> mRevealers = new List<Revealer>();

	private static List<Revealer> mAdded = new List<Revealer>();

	private static List<Revealer> mRemoved = new List<Revealer>();

	protected Color32[] mBuffer0;

	protected Color32[] mBuffer1;

	protected Color32[] mBuffer2;

	protected Texture2D mTexture0;

	protected Texture2D mTexture1;

	protected volatile float mBlendFactor;

	protected float mNextUpdate;

	protected int mScreenHeight;

	protected volatile State mState;

	private object mLock = new object();

	private AutoResetEvent wakeupEvent = new AutoResetEvent(initialState: false);

	private Thread mThread;

	private const float ThreadWorkingTimeMin = 3f;

	private float mThreadWorkingTimeMin = 3f;

	public int worldSize = 256;

	public int textureSize = 128;

	public float updateFrequency = 0.1f;

	public float textureBlendTime = 0.5f;

	public int blurIterations = 2;

	public Vector2 heightRange = new Vector2(0f, 10f);

	public LayerMask raycastMask = -1;

	public float raycastRadius;

	public float margin = 0.4f;

	public bool debug;

	private volatile bool isRun = true;

	private Action OnComplete;

	public bool NeedDrawFogUpdateRange;

	public int WorldFogUpdateRange = 100;

	private UniversalRenderPipelineAsset pipeline;

	private bool dirty;

	private float mElapsed;

	private Color32 white = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public bool needUpdateCenterPos;

	public volatile bool subThreadUpdateFinish;

	public Vector3 newCenterPos;

	public Texture2D texture0 => mTexture0;

	public Texture2D texture1 => mTexture1;

	public float blendFactor => mBlendFactor;

	public bool isRevealRect { get; set; }

	public static Revealer CreateRevealer()
	{
		Revealer revealer = new Revealer();
		revealer.isActive = false;
		lock (mAdded)
		{
			mAdded.Add(revealer);
			return revealer;
		}
	}

	public static void AddRevealer(Revealer rev)
	{
		lock (mAdded)
		{
			mAdded.Add(rev);
		}
	}

	public static void DeleteRevealer(Revealer rev)
	{
		lock (mRemoved)
		{
			mRemoved.Add(rev);
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void Clear()
	{
		OnComplete = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ClearRevealers()
	{
		mRevealers.Clear();
	}

	private void OnEnable()
	{
		InitRender();
	}

	public float GetRenderScale()
	{
		if (pipeline == null)
		{
			return 1f;
		}
		return pipeline.renderScale;
	}

	public void InitRender()
	{
		pipeline = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		_ = pipeline != null;
	}

	public void ResetOrigin(Vector3 pos)
	{
		mOrigin = pos;
		mOrigin.x -= (float)worldSize * 0.5f;
		mOrigin.z -= (float)worldSize * 0.5f;
	}

	public void InitAllBuffer()
	{
		mTrans = base.transform;
		if (mHeights == null)
		{
			mHeights = new int[textureSize, textureSize];
		}
		mOrigin = mTrans.position;
		mOrigin.x -= (float)worldSize * 0.5f;
		mOrigin.z -= (float)worldSize * 0.5f;
		int num = textureSize * textureSize;
		if (mBuffer0 == null)
		{
			mBuffer0 = new Color32[num];
		}
		if (mBuffer1 == null)
		{
			mBuffer1 = new Color32[num];
		}
		if (mBuffer2 == null)
		{
			mBuffer2 = new Color32[num];
		}
		if (mAdded.Count > 0)
		{
			lock (mAdded)
			{
				while (mAdded.Count > 0)
				{
					int index = mAdded.Count - 1;
					mRevealers.Add(mAdded[index]);
					mAdded.RemoveAt(index);
				}
			}
		}
		if (mRemoved.Count > 0)
		{
			lock (mRemoved)
			{
				while (mRemoved.Count > 0)
				{
					int index2 = mRemoved.Count - 1;
					mRevealers.Remove(mRemoved[index2]);
					mRemoved.RemoveAt(index2);
				}
			}
		}
		float worldToTex = (float)textureSize / (float)worldSize;
		for (int i = 0; i < mRevealers.Count; i++)
		{
			Revealer revealer = mRevealers[i];
			if (revealer.isActive)
			{
				if (revealer.los == LOSChecks.None)
				{
					RevealUsingRadius(revealer, worldToTex);
				}
				else if (revealer.los == LOSChecks.OnlyOnce)
				{
					RevealUsingCache(revealer, worldToTex);
				}
				else
				{
					RevealUsingLOS(revealer, worldToTex);
				}
			}
		}
		int num2 = blurIterations / 3;
		for (int j = 0; j < num2; j++)
		{
			BlurVisibility();
		}
		for (int k = 0; k < textureSize; k++)
		{
			int num3 = k * textureSize;
			for (int l = 0; l < textureSize; l++)
			{
				int num4 = l + num3;
				Color32 color = mBuffer1[num4];
				if (color.g < color.r)
				{
					color.g = color.r;
					mBuffer1[num4] = color;
				}
			}
		}
		int m = 0;
		for (int num5 = mBuffer0.Length; m < num5; m++)
		{
			mBuffer0[m] = mBuffer1[m];
		}
		UpdateTexture();
	}

	public void RegisterCompleteAction(Action onComplete)
	{
		OnComplete = onComplete;
		if (mThread != null)
		{
			onComplete?.Invoke();
		}
	}

	private void Start()
	{
		mTrans = base.transform;
		if (mHeights == null)
		{
			mHeights = new int[textureSize, textureSize];
		}
		_ = mSize;
		mOrigin = mTrans.position;
		mOrigin.x -= (float)worldSize * 0.5f;
		mOrigin.z -= (float)worldSize * 0.5f;
		int num = textureSize * textureSize;
		if (mBuffer0 == null)
		{
			mBuffer0 = new Color32[num];
		}
		if (mBuffer1 == null)
		{
			mBuffer1 = new Color32[num];
		}
		if (mBuffer2 == null)
		{
			mBuffer2 = new Color32[num];
		}
		subThreadUpdateFinish = false;
		UpdateBuffer(isStart: true);
		UpdateTexture();
		mNextUpdate = Time.time + updateFrequency;
		mThread = new Thread(ThreadUpdate);
		mThread.Start();
		if (OnComplete != null)
		{
			OnComplete();
		}
	}

	private void OnDestroy()
	{
		if (mTexture0 != null)
		{
			UnityEngine.Object.Destroy(mTexture0);
			mTexture0 = null;
		}
		if (mTexture1 != null)
		{
			UnityEngine.Object.Destroy(mTexture1);
			mTexture0 = null;
		}
		if (mThread != null)
		{
			isRun = false;
			wakeupEvent.Set();
			mThread.Join(50);
			if (mThread.IsAlive)
			{
				UnityEngine.Debug.Log("OnDestroy but timeout!!!");
				mThread.Abort("");
			}
			OnComplete = null;
			mThread = null;
			wakeupEvent.Dispose();
		}
	}

	private void Update()
	{
		bool flag = false;
		lock (mAdded)
		{
			flag |= mAdded.Count > 0;
		}
		lock (mRemoved)
		{
			flag |= mRemoved.Count > 0;
		}
		State state;
		lock (mLock)
		{
			state = mState;
			if (flag)
			{
				dirty = true;
				mBlendFactor = 0f;
				mThreadWorkingTimeMin = 3f;
			}
			float num = mBlendFactor;
			if (textureBlendTime > 0f)
			{
				mBlendFactor = Mathf.Clamp01(mBlendFactor + Time.deltaTime / textureBlendTime);
			}
			else
			{
				mBlendFactor = 1f;
			}
			dirty |= (double)Mathf.Abs(mBlendFactor - num) > 1E-05;
		}
		switch (state)
		{
		case State.Blending:
		{
			float time = Time.time;
			mNextUpdate = time + updateFrequency;
			lock (mLock)
			{
				mState = State.NeedUpdate;
			}
			break;
		}
		case State.NeedUpdate:
			if (dirty || mThreadWorkingTimeMin > 0f)
			{
				dirty = false;
				wakeupEvent.Set();
			}
			break;
		default:
			UpdateTexture();
			break;
		}
		if (mThreadWorkingTimeMin > 0f)
		{
			mThreadWorkingTimeMin -= Time.deltaTime;
		}
		if (needUpdateCenterPos && subThreadUpdateFinish)
		{
			base.gameObject.transform.position = newCenterPos;
			needUpdateCenterPos = false;
		}
	}

	private void ThreadUpdate()
	{
		Stopwatch stopwatch = new Stopwatch();
		try
		{
			while (isRun)
			{
				State state;
				lock (mLock)
				{
					state = mState;
				}
				if (state == State.NeedUpdate)
				{
					stopwatch.Reset();
					stopwatch.Start();
					subThreadUpdateFinish = false;
					UpdateBuffer(isStart: false);
					stopwatch.Stop();
					if (debug)
					{
						UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
					}
					mElapsed = 0.001f * (float)stopwatch.ElapsedMilliseconds;
					lock (mLock)
					{
						mState = State.UpdateTexture0;
					}
				}
				wakeupEvent.WaitOne();
			}
		}
		catch (ThreadAbortException)
		{
			UnityEngine.Debug.Log("thread abortException!");
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(new Vector3(0f, (heightRange.x + heightRange.y) * 0.5f, 0f), new Vector3(worldSize, heightRange.y - heightRange.x, worldSize));
		if (NeedDrawFogUpdateRange)
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(new Vector3(0f, (heightRange.x + heightRange.y) * 0.5f, 0f), new Vector3(WorldFogUpdateRange, heightRange.y - heightRange.x, WorldFogUpdateRange));
		}
	}

	private bool IsVisible(int sx, int sy, int fx, int fy, float outer, int sightHeight, int variance)
	{
		int num = Mathf.Abs(fx - sx);
		int num2 = Mathf.Abs(fy - sy);
		int num3 = ((sx < fx) ? 1 : (-1));
		int num4 = ((sy < fy) ? 1 : (-1));
		int num5 = num - num2;
		float b = sightHeight;
		float a = mHeights[fx, fy];
		float num6 = 1f / outer;
		float num7 = 0f;
		while (true)
		{
			if (sx == fx && sy == fy)
			{
				return true;
			}
			int num8 = fx - sx;
			int num9 = fy - sy;
			num7 = num6 * Mathf.Sqrt(num8 * num8 + num9 * num9);
			if ((float)mHeights[sx, sy] > Mathf.Lerp(a, b, num7) + (float)variance)
			{
				break;
			}
			int num10 = num5 << 1;
			if (num10 > -num2)
			{
				num5 -= num2;
				sx += num3;
			}
			if (num10 < num)
			{
				num5 += num;
				sy += num4;
			}
		}
		return false;
	}

	public int WorldToGridHeight(float height)
	{
		return Mathf.Clamp(Mathf.RoundToInt(height / mSize.y * 255f), 0, 255);
	}

	protected virtual void CreateGrid()
	{
		Vector3 vector = mOrigin;
		vector.y += mSize.y;
		float num = (float)worldSize / (float)textureSize;
		_ = raycastRadius;
		for (int i = 0; i < textureSize; i++)
		{
			vector.z = mOrigin.z + (float)i * num;
			for (int j = 0; j < textureSize; j++)
			{
				mHeights[j, i] = 0;
			}
		}
	}

	private void UpdateBuffer(bool isStart)
	{
		lock (mAdded)
		{
			if (mAdded.Count > 0)
			{
				while (mAdded.Count > 0)
				{
					int index = mAdded.Count - 1;
					mRevealers.Add(mAdded[index]);
					mAdded.RemoveAt(index);
				}
			}
		}
		lock (mRemoved)
		{
			if (mRemoved.Count > 0)
			{
				while (mRemoved.Count > 0)
				{
					int index2 = mRemoved.Count - 1;
					mRevealers.Remove(mRemoved[index2]);
					mRemoved.RemoveAt(index2);
				}
			}
		}
		float worldToTex = (float)textureSize / (float)worldSize;
		for (int i = 0; i < mRevealers.Count; i++)
		{
			Revealer revealer = mRevealers[i];
			if (revealer.isActive)
			{
				if (revealer.los == LOSChecks.None)
				{
					RevealUsingRadius(revealer, worldToTex);
				}
				else if (revealer.los == LOSChecks.OnlyOnce)
				{
					RevealUsingCache(revealer, worldToTex);
				}
				else
				{
					RevealUsingLOS(revealer, worldToTex);
				}
			}
		}
		int num = blurIterations;
		for (int j = 0; j < num; j++)
		{
			BlurVisibility();
		}
		RevealMap();
		float t = 1f;
		lock (mLock)
		{
			t = ((textureBlendTime > 0f) ? Mathf.Clamp01(mBlendFactor + mElapsed / textureBlendTime) : 1f);
		}
		int k = 0;
		for (int num2 = mBuffer0.Length; k < num2; k++)
		{
			mBuffer0[k] = Color32.Lerp(mBuffer0[k], mBuffer1[k], t);
			mBuffer1[k].r = 0;
		}
		if (!subThreadUpdateFinish)
		{
			subThreadUpdateFinish = true;
		}
	}

	private void RevealUsingRadius(Revealer r, float worldToTex)
	{
		Vector3 vector = r.pos - mOrigin;
		int num = Mathf.RoundToInt((vector.x - r.outer) * worldToTex);
		int num2 = Mathf.RoundToInt((vector.z - r.outer) * worldToTex);
		int num3 = Mathf.RoundToInt((vector.x + r.outer) * worldToTex);
		int num4 = Mathf.RoundToInt((vector.z + r.outer) * worldToTex);
		int value = Mathf.RoundToInt(vector.x * worldToTex);
		int value2 = Mathf.RoundToInt(vector.z * worldToTex);
		value = Mathf.Clamp(value, 0, textureSize - 1);
		value2 = Mathf.Clamp(value2, 0, textureSize - 1);
		int num5 = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
		for (int i = num2; i < num4; i++)
		{
			if (i <= -1 || i >= textureSize)
			{
				continue;
			}
			int num6 = i * textureSize;
			for (int j = num; j < num3; j++)
			{
				if (j > -1 && j < textureSize)
				{
					int num7 = j - value;
					int num8 = i - value2;
					if (num7 * num7 + num8 * num8 < num5)
					{
						mBuffer1[j + num6].r = byte.MaxValue;
					}
				}
			}
		}
	}

	private void RevealUsingLOS(Revealer r, float worldToTex)
	{
		Vector3 vector = r.pos - mOrigin;
		int num = Mathf.RoundToInt((vector.x - r.outer) * worldToTex);
		int num2 = Mathf.RoundToInt((vector.z - r.outer) * worldToTex);
		int num3 = Mathf.RoundToInt((vector.x + r.outer) * worldToTex);
		int num4 = Mathf.RoundToInt((vector.z + r.outer) * worldToTex);
		int value = Mathf.RoundToInt(vector.x * worldToTex);
		int value2 = Mathf.RoundToInt(vector.z * worldToTex);
		value = Mathf.Clamp(value, 0, textureSize - 1);
		value2 = Mathf.Clamp(value2, 0, textureSize - 1);
		int num5 = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
		int num6 = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
		int sightHeight = WorldToGridHeight(r.pos.y);
		int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255f);
		for (int i = num2; i < num4; i++)
		{
			if (i <= -1 || i >= textureSize)
			{
				continue;
			}
			for (int j = num; j < num3; j++)
			{
				if (j <= -1 || j >= textureSize)
				{
					continue;
				}
				int num7 = j - value;
				int num8 = i - value2;
				int num9 = num7 * num7 + num8 * num8;
				int num10 = j + i * textureSize;
				if (num9 < num5 || (value == j && value2 == i))
				{
					mBuffer1[num10] = white;
				}
				else if (num9 < num6)
				{
					Vector2 vector2 = new Vector2(num7, num8);
					vector2.Normalize();
					vector2 *= r.inner;
					int num11 = value + Mathf.RoundToInt(vector2.x);
					int num12 = value2 + Mathf.RoundToInt(vector2.y);
					if (num11 > -1 && num11 < textureSize && num12 > -1 && num12 < textureSize && IsVisible(num11, num12, j, i, Mathf.Sqrt(num9), sightHeight, variance))
					{
						mBuffer1[num10] = white;
					}
				}
			}
		}
	}

	private void RevealUsingCache(Revealer r, float worldToTex)
	{
		if (r.cachedBuffer == null)
		{
			if (isRevealRect)
			{
				RevealIntoCacheRect(r, worldToTex);
			}
			else
			{
				RevealIntoCache(r, worldToTex);
			}
		}
		int i = r.cachedY;
		for (int num = r.cachedY + r.cachedSize; i < num; i++)
		{
			if (i <= -1 || i >= textureSize)
			{
				continue;
			}
			int num2 = i * textureSize;
			int num3 = (i - r.cachedY) * r.cachedSize;
			int j = r.cachedX;
			for (int num4 = r.cachedX + r.cachedSize; j < num4; j++)
			{
				if (j > -1 && j < textureSize)
				{
					int num5 = j - r.cachedX + num3;
					if (r.cachedBuffer[num5])
					{
						mBuffer1[j + num2] = white;
					}
				}
			}
		}
	}

	private void RevealIntoCache(Revealer r, float worldToTex)
	{
		Vector3 vector = r.pos - mOrigin;
		int num = Mathf.RoundToInt((vector.x - r.outer) * worldToTex);
		int num2 = Mathf.RoundToInt((vector.z - r.outer) * worldToTex);
		int num3 = Mathf.RoundToInt((vector.x + r.outer) * worldToTex);
		int num4 = Mathf.RoundToInt((vector.z + r.outer) * worldToTex);
		int value = Mathf.RoundToInt(vector.x * worldToTex);
		int value2 = Mathf.RoundToInt(vector.z * worldToTex);
		value = Mathf.Clamp(value, 0, textureSize - 1);
		value2 = Mathf.Clamp(value2, 0, textureSize - 1);
		int num5 = Mathf.RoundToInt(num3 - num);
		r.cachedBuffer = new bool[num5 * num5];
		r.cachedSize = num5;
		r.cachedX = num;
		r.cachedY = num2;
		int i = 0;
		for (int num6 = num5 * num5; i < num6; i++)
		{
			r.cachedBuffer[i] = false;
		}
		int num7 = Mathf.RoundToInt(r.inner * r.inner * worldToTex * worldToTex);
		int num8 = Mathf.RoundToInt(r.outer * r.outer * worldToTex * worldToTex);
		int variance = Mathf.RoundToInt(Mathf.Clamp01(margin / (heightRange.y - heightRange.x)) * 255f);
		int sightHeight = WorldToGridHeight(r.pos.y);
		for (int j = num2; j < num4; j++)
		{
			if (j <= -1 || j >= textureSize)
			{
				continue;
			}
			for (int k = num; k < num3; k++)
			{
				if (k <= -1 || k >= textureSize)
				{
					continue;
				}
				int num9 = k - value;
				int num10 = j - value2;
				int num11 = num9 * num9 + num10 * num10;
				if (num11 < num7 || (value == k && value2 == j))
				{
					r.cachedBuffer[k - num + (j - num2) * num5] = true;
				}
				else if (num11 < num8)
				{
					Vector2 vector2 = new Vector2(num9, num10);
					vector2.Normalize();
					vector2 *= r.inner;
					int num12 = value + Mathf.RoundToInt(vector2.x);
					int num13 = value2 + Mathf.RoundToInt(vector2.y);
					if (num12 > -1 && num12 < textureSize && num13 > -1 && num13 < textureSize && IsVisible(num12, num13, k, j, Mathf.Sqrt(num11), sightHeight, variance))
					{
						r.cachedBuffer[k - num + (j - num2) * num5] = true;
					}
				}
			}
		}
	}

	private void RevealIntoCacheRect(Revealer r, float worldToTex)
	{
		Vector3 vector = r.pos - mOrigin;
		float num = 0f;
		int num2 = Mathf.RoundToInt((vector.x - r.outer - num) * worldToTex);
		int num3 = Mathf.RoundToInt((vector.z - r.outer - num) * worldToTex);
		int num4 = Mathf.RoundToInt((vector.x + r.outer - num) * worldToTex);
		int num5 = Mathf.RoundToInt((vector.z + r.outer - num) * worldToTex);
		int value = Mathf.RoundToInt(vector.x * worldToTex);
		int value2 = Mathf.RoundToInt(vector.z * worldToTex);
		value = Mathf.Clamp(value, 0, textureSize - 1);
		Mathf.Clamp(value2, 0, textureSize - 1);
		int num6 = Mathf.RoundToInt(num4 - num2);
		r.cachedBuffer = new bool[num6 * num6];
		r.cachedSize = num6;
		r.cachedX = num2;
		r.cachedY = num3;
		int i = 0;
		for (int num7 = num6 * num6; i < num7; i++)
		{
			r.cachedBuffer[i] = false;
		}
		for (int j = num3; j < num5; j++)
		{
			if (j <= -1 || j >= textureSize)
			{
				continue;
			}
			for (int k = num2; k < num4; k++)
			{
				if (k > -1 && k < textureSize)
				{
					r.cachedBuffer[k - num2 + (j - num3) * num6] = true;
				}
			}
		}
	}

	private void BlurVisibility()
	{
		if (mBuffer1 == null || mBuffer2 == null)
		{
			UnityEngine.Debug.LogError("blur fail");
			return;
		}
		for (int i = 0; i < textureSize; i++)
		{
			int num = i * textureSize;
			int num2 = i - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = i + 1;
			if (num3 == textureSize)
			{
				num3 = i;
			}
			num2 *= textureSize;
			num3 *= textureSize;
			for (int j = 0; j < textureSize; j++)
			{
				int num4 = j - 1;
				if (num4 < 0)
				{
					num4 = 0;
				}
				int num5 = j + 1;
				if (num5 == textureSize)
				{
					num5 = j;
				}
				int num6 = j + num;
				int r = mBuffer1[num6].r;
				r += mBuffer1[num4 + num].r;
				r += mBuffer1[num5 + num].r;
				r += mBuffer1[j + num2].r;
				r += mBuffer1[j + num3].r;
				r += mBuffer1[num4 + num2].r;
				r += mBuffer1[num5 + num2].r;
				r += mBuffer1[num4 + num3].r;
				r += mBuffer1[num5 + num3].r;
				Color32 color = mBuffer2[num6];
				color.r = (byte)(r / 9);
				mBuffer2[num6] = color;
			}
		}
		Color32[] array = mBuffer1;
		mBuffer1 = mBuffer2;
		mBuffer2 = array;
	}

	private void RevealMap()
	{
		for (int i = 0; i < textureSize; i++)
		{
			int num = i * textureSize;
			for (int j = 0; j < textureSize; j++)
			{
				int num2 = j + num;
				Color32 color = mBuffer1[num2];
				if (color.g < color.r)
				{
					color.g = color.r;
					mBuffer1[num2] = color;
				}
			}
		}
	}

	private void UpdateTexture()
	{
		State state;
		lock (mLock)
		{
			state = mState;
		}
		if (mScreenHeight != Screen.height || mTexture0 == null)
		{
			mScreenHeight = Screen.height;
			if (mTexture0 != null)
			{
				UnityEngine.Object.Destroy(mTexture0);
			}
			if (mTexture1 != null)
			{
				UnityEngine.Object.Destroy(mTexture1);
			}
			mTexture0 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, mipChain: false);
			mTexture1 = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, mipChain: false);
			mTexture0.wrapMode = TextureWrapMode.Clamp;
			mTexture1.wrapMode = TextureWrapMode.Clamp;
			mTexture0.SetPixels32(mBuffer0);
			mTexture0.Apply();
			mTexture1.SetPixels32(mBuffer1);
			mTexture1.Apply();
			lock (mLock)
			{
				mState = State.Blending;
				return;
			}
		}
		switch (state)
		{
		case State.UpdateTexture0:
			mTexture0.SetPixels32(mBuffer0);
			mTexture0.Apply();
			lock (mLock)
			{
				mState = State.Blending;
				break;
			}
		case State.UpdateTexture1:
			lock (mLock)
			{
				mState = State.Blending;
				break;
			}
		}
	}

	public bool IsVisible(Vector3 pos)
	{
		if (mBuffer0 == null)
		{
			return false;
		}
		pos -= mOrigin;
		float num = (float)textureSize / (float)worldSize;
		int value = Mathf.RoundToInt(pos.x * num);
		int value2 = Mathf.RoundToInt(pos.z * num);
		int num2 = Mathf.Clamp(value, 0, textureSize - 1);
		value2 = Mathf.Clamp(value2, 0, textureSize - 1);
		int num3 = num2 + value2 * textureSize;
		if (mBuffer1[num3].r <= 0)
		{
			return mBuffer0[num3].r > 0;
		}
		return true;
	}

	public bool IsExplored(Vector3 pos)
	{
		if (mBuffer0 == null)
		{
			return false;
		}
		pos -= mOrigin;
		float num = (float)textureSize / (float)worldSize;
		int value = Mathf.RoundToInt(pos.x * num);
		int value2 = Mathf.RoundToInt(pos.z * num);
		value = Mathf.Clamp(value, 0, textureSize - 1);
		value2 = Mathf.Clamp(value2, 0, textureSize - 1);
		return mBuffer0[value + value2 * textureSize].g > 0;
	}

	public bool CanShowWhiteFog()
	{
		return true;
	}
}
