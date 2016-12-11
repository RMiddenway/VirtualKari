using AppKit;
using Foundation;

namespace VirtualKari
{
	[Register("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;

		public AppDelegate()
		{
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			mainWindowController = new MainWindowController();
			mainWindowController.Window.MakeKeyAndOrderFront(this);
			//			DownloadsFolderSearch.CreateOldFileList();
			//			DownloadsFolderSearch.CreateOldDirectoryList();
			DownloadsFolderSearch.CreateOldDownloadedItemsList();
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}
