using System;
namespace VirtualKari
{
	public class MyEventArgs : EventArgs
	{
		public string path { get; internal set; }
		public bool isDirectory { get; internal set; }
		public MyEventArgs(string _path, bool _isDirectory)
		{
			path = _path;
			isDirectory = _isDirectory;
		}
	}
}
