using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

//using System.Drawing;


using Foundation;
using AppKit;

namespace VirtualKari
{
	public class DownloadsFolderSearch
	{
		static TimeSpan minimumFileAge = new TimeSpan(30, 0, 0, 0);
		static int allowedSizeVariance = 20;
		public enum itemTypes { DIR, FILE };
		public static List<Item> oldItems = new List<Item>();
		public static List<Item> similarItems = new List<Item>();
//		public static List<FileInfo> oldDownloadedFileList = new List<FileInfo>();
//		public static List<FileInfo> similarFiles = new List<FileInfo>();
//		public static List<DirectoryInfo> oldDownloadedDirectoryList = new List<DirectoryInfo>();
		public static FilePathAdjuster duplicatePathAdjuster;
		public static FilePathAdjuster newerPathAdjuster;
//		public static FileInfo currentFile = null;
		public static Item currentItem = null;
		public static DirectoryInfo downloadsFolder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"/Downloads"));
		public static string duplicateNote = "";

		public DownloadsFolderSearch()
		{
		}
		private static List<FileInfo> CreateOldFileList() {
			FileInfo[] files = downloadsFolder.GetFiles();
			List<FileInfo> oldFileList = new List<FileInfo>();
			foreach ( FileInfo file in files) 
			{
				if (!file.Attributes.HasFlag(FileAttributes.Hidden) && DateTime.Today - file.CreationTime > minimumFileAge){
					oldFileList.Add(file);
				}
			}
			return oldFileList;
//			currentFile = oldDownloadedFileList[0];
		}
		private static List<DirectoryInfo> CreateOldDirectoryList() { 
			DirectoryInfo[] directories = downloadsFolder.GetDirectories();
			List<DirectoryInfo> oldDirectoryList = new List<DirectoryInfo>();
			foreach (DirectoryInfo directory in directories)
			{
				if (!directory.Attributes.HasFlag(FileAttributes.Hidden) && DateTime.Today - directory.CreationTime > minimumFileAge)
				{
					oldDirectoryList.Add(directory);
				}
			}
			return oldDirectoryList;
		}
		public static void CreateOldDownloadedItemsList() 
		{
			List<FileInfo> files = CreateOldFileList();
			List<DirectoryInfo> directories = CreateOldDirectoryList();
			List<Item> oldItemsUnsorted = new List<Item>();
			foreach (FileInfo file in files)
			{
				oldItemsUnsorted.Add(new Item(file));
			}
			foreach (DirectoryInfo directory in directories)
			{
				oldItemsUnsorted.Add(new Item(directory));
			}

			oldItems = oldItemsUnsorted.OrderBy(o => o.Name).ToList();

			MainWindowController.Next();

		}
		public static Dictionary<string, string> NextItem() 
		{
			if (currentItem == null)
				currentItem = oldItems[0];
			else {
				int nextIndex = oldItems.IndexOf(currentItem) + 1;
				if (nextIndex < oldItems.Count)
					currentItem = oldItems[nextIndex];
				else
					currentItem = oldItems[0];
			}
			return CreateItemInformation(currentItem);
		}
		//public static string NextFile() {//
		//	int nextFileIndex = oldDownloadedFileList.IndexOf(currentFile) + 1;
		//	if (nextFileIndex < oldDownloadedFileList.Count)
		//		currentFile = oldDownloadedFileList[nextFileIndex];
		//	else
		//		currentFile = oldDownloadedFileList[0];
		//	return CreateFileInformation(currentFile);
		//}
		static Dictionary<string, string> CreateItemInformation(Item item)
		{
			Dictionary<string, string> itemInfo = new Dictionary<string, string>();
			itemInfo.Add("NAME", item.Name);
			itemInfo.Add("SIZE", item.Size());
			itemInfo.Add("DATE", string.Format("{0}, {1}",item.CreationTime().ToString(), item.Age()));
			itemInfo.Add("ICONPATH", item.FullName);
//			string fileInformation = item.Name + Environment.NewLine
//			                             + item.Size() + Environment.NewLine
//			                             + item.CreationTime();
			CheckForSimilarItems(item);
//			fileInformation += Environment.NewLine + duplicateNote;
			itemInfo.Add("WARNING", duplicateNote);
			//			return fileInformation;
			return itemInfo;
		}
		static void CheckForSimilarItems(Item item)
		{
			similarItems.Clear();
			string root = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			SearchDirectories(root, IdentifyDuplicate);
			if (similarItems.Count > 0)
			{
				foreach (Item duplicate in similarItems) 
				{
					if (duplicate.Size() == item.Size())
					{
						duplicatePathAdjuster = new FilePathAdjuster(duplicate.FullName, duplicate.IsDirectory);
						MainWindowController.UpdateDuplicate();
					}
					else if (duplicate.CreationTime() > item.CreationTime())
					{
						newerPathAdjuster = new FilePathAdjuster(duplicate.FullName, duplicate.IsDirectory);
						MainWindowController.UpdateNewer();
					}
				}
			}

		}
//		static string CreateFileInformation(FileInfo file)
//		{
//			string fileInformation = file.Name + Environment.NewLine
//										 + file.Length + Environment.NewLine
//										 + file.CreationTime;
//			similarFiles.Clear();
//			string root = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
////			string root = "/Users/rogermiddenway";
//			Console.WriteLine("Starting at " + root);
//			SearchDirectories(root, IdentifyDuplicate);
//			if (similarFiles.Count > 0)
//			{
////				fileInformation += Environment.NewLine + String.Format("Found {0} additional copies of {1} ", similarFiles.Count, similarFiles[0].Name);
//				foreach (FileInfo duplicate in similarFiles) {
//					if (duplicate.Length == file.Length)
//					{
////						fileInformation += Environment.NewLine + "Duplicate can be found in " + duplicate.DirectoryName;

//						duplicatePathAdjuster = new FilePathAdjuster(duplicate.FullName, duplicate.Extension != null);
//						MainWindowController.UpdateDuplicate();
//					}
//					else if(duplicate.CreationTime>file.CreationTime){ 
////						fileInformation += Environment.NewLine + "Newer version can be found in " + duplicate.DirectoryName;

//						newerPathAdjuster = new FilePathAdjuster(duplicate.FullName, duplicate.Extension != null);
//						MainWindowController.UpdateNewer();
//					}
//				}
//			}
//			return fileInformation;
//		}
		static void IdentifyDuplicate(string path, itemTypes type) {
			if (type == itemTypes.FILE)
			{
				string fileName = Path.GetFileName(path);
				if (fileName == currentItem.Name)
//					similarFiles.Add(new FileInfo(path));
					similarItems.Add(new Item(new FileInfo(path)));
			}
			if (type == itemTypes.DIR)
			{
				string directoryName = new DirectoryInfo(path).Name;
				if (directoryName == currentItem.Name)
				{
					similarItems.Add(new Item(new DirectoryInfo(path)));
					if (!CheckForSimilarDirectorySize(path))
					{
						duplicateNote = string.Format("Folder sizes differ by more than {0}%", allowedSizeVariance.ToString());
						Console.WriteLine("DIFF SIZE");
					}
					else {
						duplicateNote = "";
						Console.WriteLine("SAME SIZE");
					}
				}
				
			}

				
			//TODO check folder contents too - say if you have some or all contents elsewhere
		}
		static bool CheckForSimilarDirectorySize(string path)
		{
			//var contents = Directory.EnumerateFiles(currentItem.FullName, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
			//var duplicateContents = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
			string[] contents = Directory.GetFiles(currentItem.FullName, "*", SearchOption.AllDirectories);
			string[] duplicateContents = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
			long originalSize = 0;
			long duplicateSize = 0;
			foreach (string filePath in contents) {
				FileInfo file = new FileInfo(filePath);
				originalSize += file.Length;
			}
			foreach (string filePath in duplicateContents)
			{
				FileInfo file = new FileInfo(filePath);
				duplicateSize += file.Length;
			}
			if (Math.Abs(duplicateSize / originalSize - 1) > allowedSizeVariance/100)
				return false;
			else
				return true;
		}
		static void SearchDirectories(string folder, Action<string, itemTypes> fileAction)
		{
			string[] splitPath = folder.Split('/');
			string folderName = splitPath[splitPath.Length - 1];
			DirectoryInfo currentDirectory = new DirectoryInfo(folder);
			if (folderName != "Downloads" && folderName != "Library"  && !currentDirectory.Attributes.HasFlag(FileAttributes.Hidden))
			{
				foreach (string file in Directory.GetFiles(folder))
				{
					fileAction(file, itemTypes.FILE);
				}
				foreach (string dir in Directory.GetDirectories(folder))
				{
					fileAction(dir, itemTypes.DIR);
				}
				foreach (string subDir in Directory.GetDirectories(folder))
				{
					try
					{
						SearchDirectories(subDir, fileAction);
					}
					catch
					{
						Console.WriteLine("Error: private folder");
					}

				}
			}

		}
	}
}
