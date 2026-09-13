using System.Collections;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace System.Drawing.Printing;

public class PrinterSettings : ICloneable
{
	public class PaperSizeCollection : ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public virtual PaperSize this[int index]
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		int ICollection.Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public PaperSizeCollection(PaperSize[] array)
		{
			throw new PlatformNotSupportedException();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int Add(PaperSize paperSize)
		{
			throw new PlatformNotSupportedException();
		}

		public void CopyTo(PaperSize[] paperSizes, int index)
		{
			throw new PlatformNotSupportedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}
	}

	public class PaperSourceCollection : ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public virtual PaperSource this[int index]
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		int ICollection.Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public PaperSourceCollection(PaperSource[] array)
		{
			throw new PlatformNotSupportedException();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int Add(PaperSource paperSource)
		{
			throw new PlatformNotSupportedException();
		}

		public void CopyTo(PaperSource[] paperSources, int index)
		{
			throw new PlatformNotSupportedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}
	}

	public class PrinterResolutionCollection : ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public virtual PrinterResolution this[int index]
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		int ICollection.Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public PrinterResolutionCollection(PrinterResolution[] array)
		{
			throw new PlatformNotSupportedException();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int Add(PrinterResolution printerResolution)
		{
			throw new PlatformNotSupportedException();
		}

		public void CopyTo(PrinterResolution[] printerResolutions, int index)
		{
			throw new PlatformNotSupportedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}
	}

	public class StringCollection : ICollection, IEnumerable
	{
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public virtual string this[int index]
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		int ICollection.Count
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException();
			}
		}

		public StringCollection(string[] array)
		{
			throw new PlatformNotSupportedException();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int Add(string value)
		{
			throw new PlatformNotSupportedException();
		}

		public void CopyTo(string[] strings, int index)
		{
			throw new PlatformNotSupportedException();
		}

		public IEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool CanDuplex
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool Collate
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public short Copies
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PageSettings DefaultPageSettings
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Duplex Duplex
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int FromPage
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static StringCollection InstalledPrinters
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsDefaultPrinter
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsPlotter
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsValid
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int LandscapeAngle
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int MaximumCopies
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int MaximumPage
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int MinimumPage
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PaperSizeCollection PaperSizes
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PaperSourceCollection PaperSources
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public string PrinterName
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrinterResolutionCollection PrinterResolutions
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public string PrintFileName
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrintRange PrintRange
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool PrintToFile
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool SupportsColor
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int ToPage
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrinterSettings()
	{
		throw new PlatformNotSupportedException();
	}

	public object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public Graphics CreateMeasurementGraphics()
	{
		throw new PlatformNotSupportedException();
	}

	public Graphics CreateMeasurementGraphics(bool honorOriginAtMargins)
	{
		throw new PlatformNotSupportedException();
	}

	public Graphics CreateMeasurementGraphics(PageSettings pageSettings)
	{
		throw new PlatformNotSupportedException();
	}

	public Graphics CreateMeasurementGraphics(PageSettings pageSettings, bool honorOriginAtMargins)
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr GetHdevmode()
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr GetHdevmode(PageSettings pageSettings)
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr GetHdevnames()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsDirectPrintingSupported(Image image)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsDirectPrintingSupported(ImageFormat imageFormat)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetHdevmode(IntPtr hdevmode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetHdevnames(IntPtr hdevnames)
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
