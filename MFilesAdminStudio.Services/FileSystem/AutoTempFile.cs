using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services.FileSystem
{
	internal class AutoTempFile : IDisposable
	{
		public static readonly AutoTempFile Empty = new AutoTempFile();

		public bool IsValid => Path.Length > 0;
		public string Path { get; private set; }

		private AutoTempFile()
		{
			Path = string.Empty;
		}

		~AutoTempFile()
		{
			Dispose(disposing: false);
		}

		public static AutoTempFile Create(string extension = "")
		{
			var fileName = System.IO.Path.GetTempFileName();

			File.Delete(fileName);

			fileName += DateTime.Now.Millisecond + "." + extension;

			return new AutoTempFile
			{
				Path = fileName
			};
		}

		public static AutoTempFile Attach(string path)
		{
			return new AutoTempFile
			{
				Path = path
			};
		}

		public string Detach()
		{
			string path = Path;
			Path = string.Empty;
			return path;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (Path?.Length > 0)
				{
					if (File.Exists(Path))
					{
						File.Delete(Path);
					}
					else if (Directory.Exists(Path))
					{
						Directory.Delete(Path, recursive: true);
					}
				}

				Path = string.Empty;
			}
			catch
			{

			}
		}
	}
}
