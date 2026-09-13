using System;
using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.PerfTools;

[Serializable]
public class OverdrawMonitorCfg
{
	public MonitorAutoActivationMode autoActMode;

	[Tooltip("screenspace的ui只能由一个相机渲染，需要识别出来特殊处理才能统计到overdraw")]
	public List<string> rootUICanvasPath = new List<string> { "UIContainer" };

	[Range(0.01f, 0.05f)]
	public float fontSize = 0.015f;

	[Range(0.02f, 0.2f)]
	public float chartUpdateInterval = 0.1f;

	public bool forceUpdateEveryFrame;

	[Tooltip("overdraw相机跟实际相机属性同步的方式，true是用copyFrom识别所有变化，但内部同步layer会导致F2无法重命名hierarchy物体；false是只同步pose、fov、cullmask、orthographic")]
	public bool syncCameraWithCopyFrom;

	[Tooltip("异步获取数据请求队列的最长长度，队列满了后不在建立新的请求，会导致不在计算overdraw，直到队列有空出来")]
	[Range(4f, 32f)]
	public int maxRequestQueueLength = 8;

	public CDParticleMonitor particleMonitor;

	public CDChartAppearance chartAppearance;

	public CDChart particleChart = new CDChart
	{
		middlingValue = 200f,
		highValue = 400f,
		veryHighValue = 600f
	};

	public CDChart overdrawSummaryChart = new CDChart
	{
		middlingValue = 4f,
		highValue = 7f,
		veryHighValue = 10f
	};

	[Tooltip("所有相机的默认阈值范围，除非配置了自定义数据")]
	public CDOverdrawChart overdrawChartDefault;

	[Tooltip("可以设置不同相机的不同阈值范围")]
	public List<CDOverdrawChart> overdrawChartCustom = new List<CDOverdrawChart>();

	private void EnableOverdrawMonitor()
	{
		OverdrawMonitorManager.EnableMonitering();
	}

	private void DisableOverdrawMonitor()
	{
		OverdrawMonitorManager.DisableMonitering();
	}

	private void OpenDoc()
	{
		Application.OpenURL("https://rivergame.feishu.cn/wiki/wikcnbCMwpwx5OjqBlM8ZFBdVEb");
	}
}
