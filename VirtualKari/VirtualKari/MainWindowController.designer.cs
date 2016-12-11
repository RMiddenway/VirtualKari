// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace VirtualKari
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSPathControl DuplicateFilePath { get; set; }

		[Outlet]
		AppKit.NSTextField FileDate { get; set; }

		[Outlet]
		AppKit.NSTextField FileDescription { get; set; }

		[Outlet]
		AppKit.NSImageView FileIcon { get; set; }

		[Outlet]
		AppKit.NSTextField FileSize { get; set; }

		[Outlet]
		AppKit.NSPathControl NewerFilePath { get; set; }

		[Outlet]
		AppKit.NSTextField WarningDescription { get; set; }

		[Action ("KeepButton:")]
		partial void KeepButton (Foundation.NSObject sender);

		[Action ("ShowInFinderButton:")]
		partial void ShowInFinderButton (Foundation.NSObject sender);

		[Action ("TrashButton:")]
		partial void TrashButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (DuplicateFilePath != null) {
				DuplicateFilePath.Dispose ();
				DuplicateFilePath = null;
			}

			if (FileDescription != null) {
				FileDescription.Dispose ();
				FileDescription = null;
			}

			if (FileSize != null) {
				FileSize.Dispose ();
				FileSize = null;
			}

			if (FileDate != null) {
				FileDate.Dispose ();
				FileDate = null;
			}

			if (WarningDescription != null) {
				WarningDescription.Dispose ();
				WarningDescription = null;
			}

			if (FileIcon != null) {
				FileIcon.Dispose ();
				FileIcon = null;
			}

			if (NewerFilePath != null) {
				NewerFilePath.Dispose ();
				NewerFilePath = null;
			}
		}
	}
}
