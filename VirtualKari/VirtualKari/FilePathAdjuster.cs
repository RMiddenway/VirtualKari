using System;
using AppKit;
using Foundation;

namespace VirtualKari

{
	public class FilePathAdjuster
	{
		public string newFilePath;
		public bool isDirectory;
		public FilePathAdjuster(string _newFilePath, bool _isDirectory)
		{
			newFilePath = _newFilePath;
			isDirectory = _isDirectory;
			Console.WriteLine("PATH ADJUSTER CREATED");
		}
		public void AdjustPath(NSPathControl path) {
			path.Url = new NSUrl("file://localhost" + newFilePath);
//			path.Url = new NSUrl(new NSString("file://localhost"));
//			Console.WriteLine("NEWPATH IS " + newFilePath);
//			path.Url.Append(newFilePath, isDirectory);
			Console.WriteLine("new path is " + path.Url);

		}
	}
}
