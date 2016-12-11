using System;
using System.IO;
namespace VirtualKari
{
	public class Item
	{
		private bool isFile = false;
		private bool isDirectory = false;
		private FileInfo fInfo;
		private DirectoryInfo dInfo;
		private string name;
		private string fullName;

		public Item(FileInfo _fileInfo)
		{
			isFile = true;
			fInfo = _fileInfo;
			name = _fileInfo.Name;
			fullName = _fileInfo.FullName;
		}
		public Item(DirectoryInfo _directoryInfo)
		{
			isDirectory = true;
			dInfo = _directoryInfo;
			name = _directoryInfo.Name;
			fullName = _directoryInfo.FullName;
		}
		public string Size() 
		{
			long size = 0;
			if (isFile)
			{
				size = fInfo.Length;
			}
			else if (isDirectory)
			{
				foreach (FileInfo file in dInfo.GetFiles())
					size += file.Length;
			}
			else 
			{
				Console.WriteLine("Something went wrong getting the size");
				return "";
			}
			if (size < 1000000)
				return string.Format("{0}k", size / 1000);
			else if (size < 1000000000)
				return string.Format("{0}MB", size / 1000000);
			else if (size >= 1000000000)
				return string.Format("{0}GB", size / 1000000000);
			else
				return ("NOTHING");
		}
		public DateTime CreationTime()
		{
			if (isFile)
				return fInfo.CreationTime;
			else if (isDirectory)
				return dInfo.CreationTime;
			else
				return new DateTime(0);
		}
		public string Age() {
			TimeSpan age = DateTime.Now - CreationTime();
			if (age.Days < 1)
			{
				if (age.Hours < 1)
					return string.Format("{0} minutes old", age.Minutes);
				else
					return string.Format("{0} hours old", age.Hours);
			}
			else
				return string.Format("{0} days old", age.Days);

		}
		public string Name { get { return name;}}
		public string FullName { get { return fullName; } }
		public bool IsFile { get { return isFile;} }
		public bool IsDirectory { get { return isDirectory;} }
		public FileInfo FInfo { get { return fInfo;} }
		public DirectoryInfo DInfo { get { return dInfo;} }
	}
}
