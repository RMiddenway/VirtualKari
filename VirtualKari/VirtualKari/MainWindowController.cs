using System;
using System.IO;
using System.Collections.Generic;

using Foundation;
using AppKit;

namespace VirtualKari
{
	public partial class MainWindowController : NSWindowController
	{
		public static event EventHandler updateDuplicate;
		public static event EventHandler updateNewer;
		public static event EventHandler next;

		public static void UpdateDuplicate() {
			if (updateDuplicate != null)
				updateDuplicate(new object(), new EventArgs());
		}
		public static void UpdateNewer()
		{
			if (updateNewer != null)
				updateNewer(new object(), new EventArgs());
		}
		public static void Next()
		{
			if (next != null)
				next(new object(), new EventArgs());
		}
		//TODO ADD ICON FOR NUMBER OF DUPLICATES - ADD TRASH FUNCTION = ADD SHOW IN FINDER
		//private void InitializeFilePath() { 
		//	NSString urlString = new NSString("file://localhost/");
		//	FilePath.Url = new NSUrl(urlString);
		//}
		//TODO make windows for "are you sure" dialogs, add different fields for info
		public void UpdateDuplicateFilePath(){
			DuplicateFilePath.Hidden = false;
			DownloadsFolderSearch.duplicatePathAdjuster.AdjustPath(DuplicateFilePath);
			//			FileDescription.StringValue += DownloadsFolderSearch.duplicateNote;
			WarningDescription.StringValue = DownloadsFolderSearch.duplicateNote;
		}
		public void UpdateNewerFilePath()
		{
			NewerFilePath.Hidden = false;
			DownloadsFolderSearch.newerPathAdjuster.AdjustPath(NewerFilePath);
//			FileDescription.StringValue += DownloadsFolderSearch.duplicateNote;
			WarningDescription.StringValue = DownloadsFolderSearch.duplicateNote;
		}
		public void ShowNext()
		{ 
			HidePaths();
			WarningDescription.StringValue = "";
			Dictionary<string, string> info = DownloadsFolderSearch.NextItem();
			FileDescription.StringValue = info["NAME"];
			FileSize.StringValue = info["SIZE"];
			FileDate.StringValue = info["DATE"];
			NSWorkspace ws = new NSWorkspace();
			FileIcon.Image = ws.IconForFile(info["ICONPATH"]);
//			if (info.ContainsKey("WARNING"))
//				WarningDescription.StringValue = info["WARNING"];
//			FileDescription.StringValue = DownloadsFolderSearch.NextItem();
		}
		private void HidePaths() {
			DuplicateFilePath.Hidden = true;
			NewerFilePath.Hidden = true;
		}
		public MainWindowController(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public MainWindowController(NSCoder coder) : base(coder)
		{
		}

		public MainWindowController() : base("MainWindow")
		{
		}
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			FileDescription.StringValue = "No file selected";
			updateDuplicate = (sender, e) => UpdateDuplicateFilePath();
			updateNewer = (sender, e) => UpdateNewerFilePath();
			next = (sender, e) => ShowNext();
//			next = (sender, e) => DownloadsFolderSearch.NextItem();
//			try
//			{
////				FileDescription.StringValue = DownloadsFolderSearch.NextItem();
//				//Console.WriteLine(FilePath.Cell.StringValue);
//				//Console.WriteLine(FilePath.Url.AbsoluteString);
//				///				FilePath.Cell = new NSCell();
//				//				FilePath.Url = new NSUrl("file://localhost/" );
//				//NSString urlString = new NSString("file://localhost/");
//				//DuplicateFilePath.Url = new NSUrl(urlString);
//			}
//			catch {
//				Console.WriteLine("Couldn't do it");
//			}
		}

		public new MainWindow Window
		{
			get { return (MainWindow)base.Window; }
		}
		partial void KeepButton(NSObject sender)
		{
			ShowNext();

		}
		partial void TrashButton(NSObject sender)
		{
			NSFileManager manager = new NSFileManager();
			NSUrl a = new NSUrl("");
			NSError b = new NSError();
			string fileToTrashString = "file://localhost" + DownloadsFolderSearch.currentItem.FullName;
			NSUrl fileToTrashUrl = new NSUrl(fileToTrashString);
			bool isTrashed = manager.TrashItem(fileToTrashUrl, out a, out b);

			//TODO display confirmation of trashing

			ShowNext();
		}
		partial void ShowInFinderButton(NSObject sender)
		{

		}

	}
}
