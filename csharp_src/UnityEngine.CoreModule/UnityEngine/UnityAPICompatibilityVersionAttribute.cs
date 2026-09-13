using System;

namespace UnityEngine;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class UnityAPICompatibilityVersionAttribute : Attribute
{
	private string _version;

	private string[] _configurationAssembliesHashes;

	public string version => _version;

	internal string[] configurationAssembliesHashes => _configurationAssembliesHashes;

	[Obsolete("This overload of the attribute has been deprecated. Use the constructor that takes the version and a boolean", true)]
	public UnityAPICompatibilityVersionAttribute(string version)
	{
		_version = version;
	}

	public UnityAPICompatibilityVersionAttribute(string version, bool checkOnlyUnityVersion)
	{
		if (!checkOnlyUnityVersion)
		{
			throw new ArgumentException("You must pass 'true' to checkOnlyUnityVersion parameter.");
		}
		_version = version;
	}

	public UnityAPICompatibilityVersionAttribute(string version, string[] configurationAssembliesHashes)
	{
		_version = version;
		_configurationAssembliesHashes = configurationAssembliesHashes;
	}
}
