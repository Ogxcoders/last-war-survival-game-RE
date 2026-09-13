using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.ComponentModel.Design;

internal class RuntimeLicenseContext : LicenseContext
{
	private static TraceSwitch s_runtimeLicenseContextSwitch = new TraceSwitch("RuntimeLicenseContextTrace", "RuntimeLicenseContext tracing");

	private const int ReadBlock = 400;

	internal Hashtable savedLicenseKeys;

	private string GetLocalPath(string fileName)
	{
		Uri uri = new Uri(fileName);
		return uri.LocalPath + uri.Fragment;
	}

	public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
	{
		if (savedLicenseKeys == null || savedLicenseKeys[type.AssemblyQualifiedName] == null)
		{
			if (savedLicenseKeys == null)
			{
				savedLicenseKeys = new Hashtable();
			}
			if (resourceAssembly == null)
			{
				resourceAssembly = Assembly.GetEntryAssembly();
			}
			if (resourceAssembly == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					if (!assembly.IsDynamic)
					{
						string localPath = GetLocalPath(assembly.EscapedCodeBase);
						localPath = new FileInfo(localPath).Name;
						Stream stream = assembly.GetManifestResourceStream(localPath + ".licenses");
						if (stream == null)
						{
							stream = CaseInsensitiveManifestResourceStreamLookup(assembly, localPath + ".licenses");
						}
						if (stream != null)
						{
							DesigntimeLicenseContextSerializer.Deserialize(stream, localPath.ToUpper(CultureInfo.InvariantCulture), this);
							break;
						}
					}
				}
			}
			else if (!resourceAssembly.IsDynamic)
			{
				string localPath2 = GetLocalPath(resourceAssembly.EscapedCodeBase);
				localPath2 = Path.GetFileName(localPath2);
				string text = localPath2 + ".licenses";
				Stream manifestResourceStream = resourceAssembly.GetManifestResourceStream(text);
				if (manifestResourceStream == null)
				{
					string text2 = null;
					CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
					string name = resourceAssembly.GetName().Name;
					string[] manifestResourceNames = resourceAssembly.GetManifestResourceNames();
					foreach (string text3 in manifestResourceNames)
					{
						if (compareInfo.Compare(text3, text, CompareOptions.IgnoreCase) == 0 || compareInfo.Compare(text3, name + ".exe.licenses", CompareOptions.IgnoreCase) == 0 || compareInfo.Compare(text3, name + ".dll.licenses", CompareOptions.IgnoreCase) == 0)
						{
							text2 = text3;
							break;
						}
					}
					if (text2 != null)
					{
						manifestResourceStream = resourceAssembly.GetManifestResourceStream(text2);
					}
				}
				if (manifestResourceStream != null)
				{
					DesigntimeLicenseContextSerializer.Deserialize(manifestResourceStream, localPath2.ToUpper(CultureInfo.InvariantCulture), this);
				}
			}
		}
		return (string)savedLicenseKeys[type.AssemblyQualifiedName];
	}

	private Stream CaseInsensitiveManifestResourceStreamLookup(Assembly satellite, string name)
	{
		CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		string name2 = satellite.GetName().Name;
		string[] manifestResourceNames = satellite.GetManifestResourceNames();
		foreach (string text in manifestResourceNames)
		{
			if (compareInfo.Compare(text, name, CompareOptions.IgnoreCase) == 0 || compareInfo.Compare(text, name2 + ".exe.licenses") == 0 || compareInfo.Compare(text, name2 + ".dll.licenses") == 0)
			{
				name = text;
				break;
			}
		}
		return satellite.GetManifestResourceStream(name);
	}
}
