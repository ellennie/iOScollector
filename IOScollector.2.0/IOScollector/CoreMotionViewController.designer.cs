// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace CoreMotion
{
	[Register ("CoreMotionViewController")]
	partial class CoreMotionViewController
	{
		[Outlet]
		UIKit.UILabel lblX { get; set; }

		[Outlet]
		UIKit.UILabel lblY { get; set; }

		[Outlet]
		UIKit.UILabel lblZ { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StartButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StopButton { get; set; }

		[Action ("UIButton21_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton21_TouchUpInside (UIButton sender);

		[Action ("UIButton25_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton25_TouchUpInside (UIButton sender);

			
		void ReleaseDesignerOutlets ()
		{
			if (lblX != null) {
				lblX.Dispose ();
				lblX = null;
			}
			if (lblY != null) {
				lblY.Dispose ();
				lblY = null;
			}
			if (lblZ != null) {
				lblZ.Dispose ();
				lblZ = null;
			}
			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}
			if (StopButton != null) {
				StopButton.Dispose ();
				StopButton = null;
			}
		}
	}
}
