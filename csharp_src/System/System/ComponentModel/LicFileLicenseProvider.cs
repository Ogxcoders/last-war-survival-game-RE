using System.ComponentModel.Design;
using System.Globalization;
using System.IO;

namespace System.ComponentModel;

public class LicFileLicenseProvider : LicenseProvider
{
	private class LicFileLicense : License
	{
		private LicFileLicenseProvider _owner;

		public override string LicenseKey { get; }

		public LicFileLicense(LicFileLicenseProvider owner, string key)
		{
			_owner = owner;
			LicenseKey = key;
		}

		public override void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}

	protected virtual bool IsKeyValid(string key, Type type)
	{
		return key?.StartsWith(GetKey(type)) ?? false;
	}

	protected virtual string GetKey(Type type)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} is a licensed component.", type.FullName);
	}

	public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
	{
		LicFileLicense licFileLicense = null;
		if (context != null)
		{
			if (context.UsageMode == LicenseUsageMode.Runtime)
			{
				string savedLicenseKey = context.GetSavedLicenseKey(type, null);
				if (savedLicenseKey != null && IsKeyValid(savedLicenseKey, type))
				{
					licFileLicense = new LicFileLicense(this, savedLicenseKey);
				}
			}
			if (licFileLicense == null)
			{
				string text = null;
				if (context != null)
				{
					ITypeResolutionService typeResolutionService = (ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));
					if (typeResolutionService != null)
					{
						text = typeResolutionService.GetPathOfAssembly(type.Assembly.GetName());
					}
				}
				if (text == null)
				{
					text = type.Module.FullyQualifiedName;
				}
				string path = Path.GetDirectoryName(text) + "\\" + type.FullName + ".lic";
				if (File.Exists(path))
				{
					StreamReader streamReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
					string key = streamReader.ReadLine();
					streamReader.Close();
					if (IsKeyValid(key, type))
					{
						licFileLicense = new LicFileLicense(this, GetKey(type));
					}
				}
				if (licFileLicense != null)
				{
					context.SetSavedLicenseKey(type, licFileLicense.LicenseKey);
				}
			}
		}
		return licFileLicense;
	}
}
