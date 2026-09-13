using System;

namespace Spine.Unity;

public static class SkeletonDataCompatibility
{
	public enum SourceType
	{
		Json,
		Binary
	}

	[Serializable]
	public class VersionInfo
	{
		public string rawVersion;

		public int[] version;

		public SourceType sourceType;
	}

	[Serializable]
	public class CompatibilityProblemInfo
	{
		public VersionInfo actualVersion;

		public int[][] compatibleVersions;

		public string explicitProblemDescription;

		public string DescriptionString()
		{
			if (!string.IsNullOrEmpty(explicitProblemDescription))
			{
				return explicitProblemDescription;
			}
			string text = "";
			string arg = null;
			int[][] array = compatibleVersions;
			foreach (int[] array2 in array)
			{
				text += $"{arg}{array2[0]}.{array2[1]}";
				arg = " or ";
			}
			return string.Format("Skeleton data could not be loaded. Data version: {0}. Required version: {1}.\nPlease re-export skeleton data with Spine {1} or change runtime to version {2}.{3}.", actualVersion.rawVersion, text, actualVersion.version[0], actualVersion.version[1]);
		}
	}
}
