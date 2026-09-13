using System.Collections;
using System.IO;

namespace System.Web.Services.Protocols;

public abstract class SoapExtension
{
	private Stream stream;

	private static ArrayList[] globalExtensions;

	public virtual Stream ChainStream(Stream stream)
	{
		return stream;
	}

	public abstract object GetInitializer(Type serviceType);

	public abstract object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute);

	public abstract void Initialize(object initializer);

	public abstract void ProcessMessage(SoapMessage message);

	internal static SoapExtension[] CreateExtensionChain(SoapExtensionRuntimeConfig[] extensionConfigs)
	{
		if (extensionConfigs == null)
		{
			return null;
		}
		SoapExtension[] array = new SoapExtension[extensionConfigs.Length];
		CreateExtensionChain(extensionConfigs, array, 0);
		return array;
	}

	internal static SoapExtension[] CreateExtensionChain(SoapExtensionRuntimeConfig[] hiPrioExts, SoapExtensionRuntimeConfig[] medPrioExts, SoapExtensionRuntimeConfig[] lowPrioExts)
	{
		int num = 0;
		if (hiPrioExts != null)
		{
			num += hiPrioExts.Length;
		}
		if (medPrioExts != null)
		{
			num += medPrioExts.Length;
		}
		if (lowPrioExts != null)
		{
			num += lowPrioExts.Length;
		}
		if (num == 0)
		{
			return null;
		}
		SoapExtension[] array = new SoapExtension[num];
		int pos = 0;
		if (hiPrioExts != null)
		{
			pos = CreateExtensionChain(hiPrioExts, array, pos);
		}
		if (medPrioExts != null)
		{
			pos = CreateExtensionChain(medPrioExts, array, pos);
		}
		if (lowPrioExts != null)
		{
			pos = CreateExtensionChain(lowPrioExts, array, pos);
		}
		return array;
	}

	private static int CreateExtensionChain(SoapExtensionRuntimeConfig[] extensionConfigs, SoapExtension[] destArray, int pos)
	{
		foreach (SoapExtensionRuntimeConfig soapExtensionRuntimeConfig in extensionConfigs)
		{
			SoapExtension soapExtension = (SoapExtension)Activator.CreateInstance(soapExtensionRuntimeConfig.Type);
			soapExtension.Initialize(soapExtensionRuntimeConfig.InitializationInfo);
			destArray[pos++] = soapExtension;
		}
		return pos;
	}

	internal static SoapExtensionRuntimeConfig[] GetMethodExtensions(LogicalMethodInfo method)
	{
		object[] customAttributes = method.GetCustomAttributes(typeof(SoapExtensionAttribute));
		SoapExtensionRuntimeConfig[] array = new SoapExtensionRuntimeConfig[customAttributes.Length];
		int[] array2 = new int[customAttributes.Length];
		for (int i = 0; i < customAttributes.Length; i++)
		{
			SoapExtensionAttribute soapExtensionAttribute = (SoapExtensionAttribute)customAttributes[i];
			SoapExtensionRuntimeConfig soapExtensionRuntimeConfig = new SoapExtensionRuntimeConfig();
			soapExtensionRuntimeConfig.Type = soapExtensionAttribute.ExtensionType;
			array2[i] = soapExtensionAttribute.Priority;
			SoapExtension soapExtension = (SoapExtension)Activator.CreateInstance(soapExtensionRuntimeConfig.Type);
			soapExtensionRuntimeConfig.InitializationInfo = soapExtension.GetInitializer(method, soapExtensionAttribute);
			array[i] = soapExtensionRuntimeConfig;
		}
		Array.Sort(array2, array);
		return array;
	}

	internal static Stream ExecuteChainStream(SoapExtension[] extensions, Stream stream)
	{
		if (extensions == null)
		{
			return stream;
		}
		Stream result = stream;
		for (int i = 0; i < extensions.Length; i++)
		{
			result = extensions[i].ChainStream(result);
		}
		return result;
	}

	internal static void ExecuteProcessMessage(SoapExtension[] extensions, SoapMessage message, Stream stream, bool inverseOrder)
	{
		if (extensions == null)
		{
			return;
		}
		message.InternalStream = stream;
		if (inverseOrder)
		{
			for (int num = extensions.Length - 1; num >= 0; num--)
			{
				extensions[num].ProcessMessage(message);
			}
		}
		else
		{
			for (int i = 0; i < extensions.Length; i++)
			{
				extensions[i].ProcessMessage(message);
			}
		}
	}
}
