using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.IO.IsolatedStorage;

[ComVisible(true)]
[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
{
	private bool closed;

	private bool disposed;

	private DirectoryInfo directory;

	[CLSCompliant(false)]
	[Obsolete]
	public override ulong CurrentSize => GetDirectorySize(directory);

	[CLSCompliant(false)]
	[Obsolete]
	public override ulong MaximumSize => 9223372036854775807uL;

	internal string Root => directory.FullName;

	[ComVisible(false)]
	public override long AvailableFreeSpace
	{
		get
		{
			CheckOpen();
			return long.MaxValue;
		}
	}

	[ComVisible(false)]
	public override long Quota
	{
		get
		{
			CheckOpen();
			return (long)MaximumSize;
		}
	}

	[ComVisible(false)]
	public override long UsedSize
	{
		get
		{
			CheckOpen();
			return (long)GetDirectorySize(directory);
		}
	}

	[ComVisible(false)]
	public static bool IsEnabled => true;

	internal bool IsClosed => closed;

	internal bool IsDisposed => disposed;

	public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
	{
		Demand(scope);
		if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
		{
			throw new ArgumentException(Locale.GetText("Invalid scope, only User, User|Roaming and Machine are valid"));
		}
		return new IsolatedStorageFileEnumerator(scope, GetIsolatedStorageRoot(scope));
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Evidence domainEvidence, Type domainEvidenceType, Evidence assemblyEvidence, Type assemblyEvidenceType)
	{
		Demand(scope);
		if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None && domainEvidence == null)
		{
			throw new ArgumentNullException("domainEvidence");
		}
		if ((scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None && assemblyEvidence == null)
		{
			throw new ArgumentNullException("assemblyEvidence");
		}
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object domainIdentity, object assemblyIdentity)
	{
		Demand(scope);
		if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None && domainIdentity == null)
		{
			throw new ArgumentNullException("domainIdentity");
		}
		if ((scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None && assemblyIdentity == null)
		{
			throw new ArgumentNullException("assemblyIdentity");
		}
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile._domainIdentity = domainIdentity;
		isolatedStorageFile._assemblyIdentity = assemblyIdentity;
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
	{
		Demand(scope);
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object applicationIdentity)
	{
		Demand(scope);
		if (applicationIdentity == null)
		{
			throw new ArgumentNullException("applicationIdentity");
		}
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile._applicationIdentity = applicationIdentity;
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type applicationEvidenceType)
	{
		Demand(scope);
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile.InitStore(scope, applicationEvidenceType);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByMachine)]
	public static IsolatedStorageFile GetMachineStoreForApplication()
	{
		IsolatedStorageScope scope = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile.InitStore(scope, null);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.AssemblyIsolationByMachine)]
	public static IsolatedStorageFile GetMachineStoreForAssembly()
	{
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByMachine)]
	public static IsolatedStorageFile GetMachineStoreForDomain()
	{
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByUser)]
	public static IsolatedStorageFile GetUserStoreForApplication()
	{
		IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Application;
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
		isolatedStorageFile.InitStore(scope, null);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.AssemblyIsolationByUser)]
	public static IsolatedStorageFile GetUserStoreForAssembly()
	{
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.User | IsolatedStorageScope.Assembly);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser)]
	public static IsolatedStorageFile GetUserStoreForDomain()
	{
		IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly);
		isolatedStorageFile.PostInit();
		return isolatedStorageFile;
	}

	[ComVisible(false)]
	public static IsolatedStorageFile GetUserStoreForSite()
	{
		throw new NotSupportedException();
	}

	public static void Remove(IsolatedStorageScope scope)
	{
		string isolatedStorageRoot = GetIsolatedStorageRoot(scope);
		if (!Directory.Exists(isolatedStorageRoot))
		{
			return;
		}
		try
		{
			Directory.Delete(isolatedStorageRoot, recursive: true);
		}
		catch (IOException)
		{
			throw new IsolatedStorageException("Could not remove storage.");
		}
	}

	internal static string GetIsolatedStorageRoot(IsolatedStorageScope scope)
	{
		string text = null;
		if ((scope & IsolatedStorageScope.User) != IsolatedStorageScope.None)
		{
			text = (((scope & IsolatedStorageScope.Roaming) == 0) ? Environment.UnixGetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) : Environment.UnixGetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create));
		}
		else if ((scope & IsolatedStorageScope.Machine) != IsolatedStorageScope.None)
		{
			text = Environment.UnixGetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create);
		}
		if (text == null)
		{
			throw new IsolatedStorageException(string.Format(Locale.GetText("Couldn't access storage location for '{0}'."), scope));
		}
		return Path.Combine(text, ".isolated-storage");
	}

	private static void Demand(IsolatedStorageScope scope)
	{
	}

	private static IsolatedStorageContainment ScopeToContainment(IsolatedStorageScope scope)
	{
		return scope switch
		{
			IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly => IsolatedStorageContainment.DomainIsolationByUser, 
			IsolatedStorageScope.User | IsolatedStorageScope.Assembly => IsolatedStorageContainment.AssemblyIsolationByUser, 
			IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming => IsolatedStorageContainment.DomainIsolationByRoamingUser, 
			IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming => IsolatedStorageContainment.AssemblyIsolationByRoamingUser, 
			IsolatedStorageScope.User | IsolatedStorageScope.Application => IsolatedStorageContainment.ApplicationIsolationByUser, 
			IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine => IsolatedStorageContainment.DomainIsolationByMachine, 
			IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine => IsolatedStorageContainment.AssemblyIsolationByMachine, 
			IsolatedStorageScope.Machine | IsolatedStorageScope.Application => IsolatedStorageContainment.ApplicationIsolationByMachine, 
			IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application => IsolatedStorageContainment.ApplicationIsolationByRoamingUser, 
			_ => IsolatedStorageContainment.UnrestrictedIsolatedStorage, 
		};
	}

	internal static ulong GetDirectorySize(DirectoryInfo di)
	{
		ulong num = 0uL;
		FileInfo[] files = di.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			num += (ulong)fileInfo.Length;
		}
		DirectoryInfo[] directories = di.GetDirectories();
		foreach (DirectoryInfo di2 in directories)
		{
			num += GetDirectorySize(di2);
		}
		return num;
	}

	private IsolatedStorageFile(IsolatedStorageScope scope)
	{
		storage_scope = scope;
	}

	internal IsolatedStorageFile(IsolatedStorageScope scope, string location)
	{
		storage_scope = scope;
		directory = new DirectoryInfo(location);
		if (!directory.Exists)
		{
			throw new IsolatedStorageException(Locale.GetText("Invalid storage."));
		}
	}

	~IsolatedStorageFile()
	{
	}

	private void PostInit()
	{
		string isolatedStorageRoot = GetIsolatedStorageRoot(base.Scope);
		string text = null;
		text = "";
		isolatedStorageRoot = Path.Combine(isolatedStorageRoot, text);
		directory = new DirectoryInfo(isolatedStorageRoot);
		if (!directory.Exists)
		{
			try
			{
				directory.Create();
			}
			catch (IOException)
			{
			}
		}
	}

	public void Close()
	{
		closed = true;
	}

	public void CreateDirectory(string dir)
	{
		if (dir == null)
		{
			throw new ArgumentNullException("dir");
		}
		if (dir.IndexOfAny(Path.PathSeparatorChars) < 0)
		{
			if (directory.GetFiles(dir).Length != 0)
			{
				throw new IsolatedStorageException("Unable to create directory.");
			}
			directory.CreateSubdirectory(dir);
			return;
		}
		string[] array = dir.Split(Path.PathSeparatorChars, StringSplitOptions.RemoveEmptyEntries);
		DirectoryInfo directoryInfo = directory;
		for (int i = 0; i < array.Length; i++)
		{
			if (directoryInfo.GetFiles(array[i]).Length != 0)
			{
				throw new IsolatedStorageException("Unable to create directory.");
			}
			directoryInfo = directoryInfo.CreateSubdirectory(array[i]);
		}
	}

	[ComVisible(false)]
	public void CopyFile(string sourceFileName, string destinationFileName)
	{
		CopyFile(sourceFileName, destinationFileName, overwrite: false);
	}

	[ComVisible(false)]
	public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
	{
		if (sourceFileName == null)
		{
			throw new ArgumentNullException("sourceFileName");
		}
		if (destinationFileName == null)
		{
			throw new ArgumentNullException("destinationFileName");
		}
		if (sourceFileName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty file name is not valid.", "sourceFileName");
		}
		if (destinationFileName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty file name is not valid.", "destinationFileName");
		}
		CheckOpen();
		string text = Path.Combine(directory.FullName, sourceFileName);
		string text2 = Path.Combine(directory.FullName, destinationFileName);
		if (!IsPathInStorage(text) || !IsPathInStorage(text2))
		{
			throw new IsolatedStorageException("Operation not allowed.");
		}
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			throw new DirectoryNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
		}
		if (!File.Exists(text))
		{
			throw new FileNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
		}
		if (File.Exists(text2) && !overwrite)
		{
			throw new IsolatedStorageException("Operation not allowed.");
		}
		try
		{
			File.Copy(text, text2, overwrite);
		}
		catch (IOException inner)
		{
			throw new IsolatedStorageException("Operation not allowed.", inner);
		}
		catch (UnauthorizedAccessException inner2)
		{
			throw new IsolatedStorageException("Operation not allowed.", inner2);
		}
	}

	[ComVisible(false)]
	public IsolatedStorageFileStream CreateFile(string path)
	{
		return new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, this);
	}

	public void DeleteDirectory(string dir)
	{
		try
		{
			if (Path.IsPathRooted(dir))
			{
				dir = dir.Substring(1);
			}
			directory.CreateSubdirectory(dir).Delete();
		}
		catch
		{
			throw new IsolatedStorageException(Locale.GetText("Could not delete directory '{0}'", dir));
		}
	}

	public void DeleteFile(string file)
	{
		if (file == null)
		{
			throw new ArgumentNullException("file");
		}
		if (!File.Exists(Path.Combine(directory.FullName, file)))
		{
			throw new IsolatedStorageException(Locale.GetText("Could not delete file '{0}'", file));
		}
		try
		{
			File.Delete(Path.Combine(directory.FullName, file));
		}
		catch
		{
			throw new IsolatedStorageException(Locale.GetText("Could not delete file '{0}'", file));
		}
	}

	public void Dispose()
	{
		disposed = true;
		GC.SuppressFinalize(this);
	}

	[ComVisible(false)]
	public bool DirectoryExists(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		CheckOpen();
		string path2 = Path.Combine(directory.FullName, path);
		if (!IsPathInStorage(path2))
		{
			return false;
		}
		return Directory.Exists(path2);
	}

	[ComVisible(false)]
	public bool FileExists(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		CheckOpen();
		string path2 = Path.Combine(directory.FullName, path);
		if (!IsPathInStorage(path2))
		{
			return false;
		}
		return File.Exists(path2);
	}

	[ComVisible(false)]
	public DateTimeOffset GetCreationTime(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Trim().Length == 0)
		{
			throw new ArgumentException("An empty path is not valid.");
		}
		CheckOpen();
		string path2 = Path.Combine(directory.FullName, path);
		if (File.Exists(path2))
		{
			return File.GetCreationTime(path2);
		}
		return Directory.GetCreationTime(path2);
	}

	[ComVisible(false)]
	public DateTimeOffset GetLastAccessTime(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Trim().Length == 0)
		{
			throw new ArgumentException("An empty path is not valid.");
		}
		CheckOpen();
		string path2 = Path.Combine(directory.FullName, path);
		if (File.Exists(path2))
		{
			return File.GetLastAccessTime(path2);
		}
		return Directory.GetLastAccessTime(path2);
	}

	[ComVisible(false)]
	public DateTimeOffset GetLastWriteTime(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Trim().Length == 0)
		{
			throw new ArgumentException("An empty path is not valid.");
		}
		CheckOpen();
		string path2 = Path.Combine(directory.FullName, path);
		if (File.Exists(path2))
		{
			return File.GetLastWriteTime(path2);
		}
		return Directory.GetLastWriteTime(path2);
	}

	public string[] GetDirectoryNames(string searchPattern)
	{
		if (searchPattern == null)
		{
			throw new ArgumentNullException("searchPattern");
		}
		if (searchPattern.Contains(".."))
		{
			throw new ArgumentException("Search pattern cannot contain '..' to move up directories.", "searchPattern");
		}
		string directoryName = Path.GetDirectoryName(searchPattern);
		string fileName = Path.GetFileName(searchPattern);
		DirectoryInfo[] array = null;
		if (directoryName == null || directoryName.Length == 0)
		{
			array = directory.GetDirectories(searchPattern);
		}
		else
		{
			DirectoryInfo directoryInfo = directory.GetDirectories(directoryName)[0];
			if (directoryInfo.FullName.IndexOf(directory.FullName) >= 0)
			{
				array = directoryInfo.GetDirectories(fileName);
				string[] array2 = directoryName.Split(new char[1] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
				for (int num = array2.Length - 1; num >= 0; num--)
				{
					if (directoryInfo.Name != array2[num])
					{
						array = null;
						break;
					}
					directoryInfo = directoryInfo.Parent;
				}
			}
		}
		if (array == null)
		{
			throw new SecurityException();
		}
		FileSystemInfo[] afsi = array;
		return GetNames(afsi);
	}

	[ComVisible(false)]
	public string[] GetDirectoryNames()
	{
		return GetDirectoryNames("*");
	}

	private string[] GetNames(FileSystemInfo[] afsi)
	{
		string[] array = new string[afsi.Length];
		for (int i = 0; i != afsi.Length; i++)
		{
			array[i] = afsi[i].Name;
		}
		return array;
	}

	public string[] GetFileNames(string searchPattern)
	{
		if (searchPattern == null)
		{
			throw new ArgumentNullException("searchPattern");
		}
		if (searchPattern.Contains(".."))
		{
			throw new ArgumentException("Search pattern cannot contain '..' to move up directories.", "searchPattern");
		}
		string directoryName = Path.GetDirectoryName(searchPattern);
		string fileName = Path.GetFileName(searchPattern);
		FileInfo[] array = null;
		if (directoryName == null || directoryName.Length == 0)
		{
			array = directory.GetFiles(searchPattern);
		}
		else
		{
			DirectoryInfo[] directories = directory.GetDirectories(directoryName);
			if (directories.Length != 1)
			{
				throw new SecurityException();
			}
			if (!directories[0].FullName.StartsWith(directory.FullName))
			{
				throw new SecurityException();
			}
			if (directories[0].FullName.Substring(directory.FullName.Length + 1) != directoryName)
			{
				throw new SecurityException();
			}
			array = directories[0].GetFiles(fileName);
		}
		FileSystemInfo[] afsi = array;
		return GetNames(afsi);
	}

	[ComVisible(false)]
	public string[] GetFileNames()
	{
		return GetFileNames("*");
	}

	[ComVisible(false)]
	public override bool IncreaseQuotaTo(long newQuotaSize)
	{
		if (newQuotaSize < Quota)
		{
			throw new ArgumentException();
		}
		CheckOpen();
		return false;
	}

	[ComVisible(false)]
	public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
	{
		if (sourceDirectoryName == null)
		{
			throw new ArgumentNullException("sourceDirectoryName");
		}
		if (destinationDirectoryName == null)
		{
			throw new ArgumentNullException("sourceDirectoryName");
		}
		if (sourceDirectoryName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty directory name is not valid.", "sourceDirectoryName");
		}
		if (destinationDirectoryName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty directory name is not valid.", "destinationDirectoryName");
		}
		CheckOpen();
		string text = Path.Combine(directory.FullName, sourceDirectoryName);
		string text2 = Path.Combine(directory.FullName, destinationDirectoryName);
		if (!IsPathInStorage(text) || !IsPathInStorage(text2))
		{
			throw new IsolatedStorageException("Operation not allowed.");
		}
		if (!Directory.Exists(text))
		{
			throw new DirectoryNotFoundException("Could not find a part of path '" + sourceDirectoryName + "'.");
		}
		if (!Directory.Exists(Path.GetDirectoryName(text2)))
		{
			throw new DirectoryNotFoundException("Could not find a part of path '" + destinationDirectoryName + "'.");
		}
		try
		{
			Directory.Move(text, text2);
		}
		catch (IOException inner)
		{
			throw new IsolatedStorageException("Operation not allowed.", inner);
		}
		catch (UnauthorizedAccessException inner2)
		{
			throw new IsolatedStorageException("Operation not allowed.", inner2);
		}
	}

	[ComVisible(false)]
	public void MoveFile(string sourceFileName, string destinationFileName)
	{
		if (sourceFileName == null)
		{
			throw new ArgumentNullException("sourceFileName");
		}
		if (destinationFileName == null)
		{
			throw new ArgumentNullException("sourceFileName");
		}
		if (sourceFileName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty file name is not valid.", "sourceFileName");
		}
		if (destinationFileName.Trim().Length == 0)
		{
			throw new ArgumentException("An empty file name is not valid.", "destinationFileName");
		}
		CheckOpen();
		string text = Path.Combine(directory.FullName, sourceFileName);
		string text2 = Path.Combine(directory.FullName, destinationFileName);
		if (!IsPathInStorage(text) || !IsPathInStorage(text2))
		{
			throw new IsolatedStorageException("Operation not allowed.");
		}
		if (!File.Exists(text))
		{
			throw new FileNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
		}
		if (!Directory.Exists(Path.GetDirectoryName(text2)))
		{
			throw new IsolatedStorageException("Operation not allowed.");
		}
		try
		{
			File.Move(text, text2);
		}
		catch (UnauthorizedAccessException inner)
		{
			throw new IsolatedStorageException("Operation not allowed.", inner);
		}
	}

	[ComVisible(false)]
	public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
	{
		return new IsolatedStorageFileStream(path, mode, this);
	}

	[ComVisible(false)]
	public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access)
	{
		return new IsolatedStorageFileStream(path, mode, access, this);
	}

	[ComVisible(false)]
	public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access, FileShare share)
	{
		return new IsolatedStorageFileStream(path, mode, access, share, this);
	}

	public override void Remove()
	{
		CheckOpen(checkDirExists: false);
		try
		{
			directory.Delete(recursive: true);
		}
		catch
		{
			throw new IsolatedStorageException("Could not remove storage.");
		}
		Close();
	}

	protected override IsolatedStoragePermission GetPermission(PermissionSet ps)
	{
		if (ps == null)
		{
			return null;
		}
		return (IsolatedStoragePermission)ps.GetPermission(typeof(IsolatedStorageFilePermission));
	}

	private void CheckOpen()
	{
		CheckOpen(checkDirExists: true);
	}

	private void CheckOpen(bool checkDirExists)
	{
		if (disposed)
		{
			throw new ObjectDisposedException("IsolatedStorageFile");
		}
		if (closed)
		{
			throw new InvalidOperationException("Storage needs to be open for this operation.");
		}
		if (checkDirExists && !Directory.Exists(directory.FullName))
		{
			throw new IsolatedStorageException("Isolated storage has been removed or disabled.");
		}
	}

	private bool IsPathInStorage(string path)
	{
		return Path.GetFullPath(path).StartsWith(directory.FullName);
	}
}
